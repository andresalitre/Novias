using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ReLogic.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.UI.Chat;
using System.Collections.Generic;
using Novias.Players;
using Novias.Systems;
using System.Reflection;
using Terraria.GameContent;

namespace Novias.UI
{
    public abstract class InterfazNovias : ModSystem
    {
        internal UserInterface _ui;
        internal NoviaUIState _state;

        protected abstract NoviaUIState CrearEstado();
        protected abstract bool EsEstaNovia(NPC npc);

        public static bool InterfazAbierta<T>() where T : InterfazNovias
            => ModContent.GetInstance<T>()?._state?.EstaAbierta ?? false;

        public override void Load()
        {
            if (Main.dedServ) return;
            _ui = new UserInterface();
            _state = CrearEstado();
            _state.Activate();
        }

        public override void Unload() { _state = null; _ui = null; }

        public override void UpdateUI(GameTime gameTime)
        {
            var player = Main.LocalPlayer;
            if (player.talkNPC != -1)
            {
                var npc = Main.npc[player.talkNPC];
                if (EsEstaNovia(npc)) { if (_ui.CurrentState == null) _ui.SetState(_state); }
                else if (_ui.CurrentState != null) { _ui.SetState(null); _state.EstaAbierta = false; }
            }
            else if (_ui.CurrentState != null) { _ui.SetState(null); _state.EstaAbierta = false; }
            if (_ui?.CurrentState != null) _ui.Update(gameTime);
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            if (Main.LocalPlayer.talkNPC != -1 && EsEstaNovia(Main.npc[Main.LocalPlayer.talkNPC]))
            {
                int ic = layers.FindIndex(l => l.Name == "Vanilla: NPC / Sign Dialog");
                if (ic >= 0) layers[ic] = new LegacyGameInterfaceLayer("Vanilla: NPC / Sign Dialog", () => true, InterfaceScaleType.UI);
            }
            int idx = layers.FindIndex(l => l.Name.Equals("Vanilla: Mouse Text"));
            if (idx == -1) return;
            layers.Insert(idx, new LegacyGameInterfaceLayer("Novias: " + GetType().Name, () =>
            {
                if (_ui?.CurrentState != null) _ui.Draw(Main.spriteBatch, new GameTime());
                return true;
            }, InterfaceScaleType.UI));
        }
    }

    public enum PantallaUI { Menu, MisionPresentacion, MisionObjetivo, MisionDialogo }

    public abstract class NoviaUIState : UIState
    {
        public bool EstaAbierta = false;
        protected NPC _npc;

        private string _textoFull = "", _textoVis = "";
        private int _timerLetra;
        private const int VEL_LETRA = 2;
        private bool TextoListo => _textoVis.Length >= _textoFull.Length;

        private PantallaUI _pantalla = PantallaUI.Menu;
        private int _presentacionIndex = 0;
        private int _completacionIndex = 0;

        private List<(string nombre, string texto, bool esJugador)> _lineasMostradas = new();

        private Rectangle _pPx;
        private float _s;

        private const float W = 580f, H = 220f, PAD = 18f, T_H = 38f, B_H = 34f, GAP = 6f;

        protected virtual Color ColorFondo => new Color(28, 16, 38);
        protected virtual Color ColorBorde => new Color(200, 90, 150);
        protected virtual Color ColorTitulo => new Color(255, 190, 230);
        protected virtual Color ColorDialogoNPC => new Color(255, 200, 230);
        protected virtual Color ColorDialogoJugador => new Color(255, 225, 255);

        protected abstract NoviasPlayerBase ObtenerPlayer();
        protected abstract MisionData[] ObtenerMisiones();
        protected abstract int BuffBeso { get; }
        protected virtual string ObtenerDialogoChat() => "...";
        protected virtual string ObtenerDialogoBeso() => "...";
        protected virtual string ObtenerDialogoSeguir() => "...";
        protected virtual string ObtenerDialogoDejarSeguir() => "...";
        protected virtual string NombreNPC => _npc?.GivenOrTypeName ?? "";

        protected virtual Color ColorParaNombre(string nombre)
        {
            if (nombre == Main.LocalPlayer.name) return ColorDialogoJugador;
            return ColorDialogoNPC;
        }

        private static readonly Color CBotonVolver = new Color(192, 40, 40);
        private static readonly Color CBotonSiguiente = new Color(200, 150, 30);
        private static readonly Color CBotonCerrar = new Color(192, 40, 40);
        private static readonly Color CBotonCompletar = new Color(59, 169, 69);
        private static readonly Color CBotonBloqueado = new Color(80, 80, 80);
        private static readonly Color CBotonTienda = new Color(89, 116, 213);
        private static readonly Color CBotonSeguir = new Color(180, 80, 180);
        private static readonly Color CBotonBeso = new Color(220, 80, 130);
        private static readonly Color CBotonMision = new Color(200, 150, 30);
        private static readonly Color CBotonDialogo = new Color(59, 169, 69);
        private static readonly Color CBotonHablar = new Color(59, 169, 69);

        protected struct Btn
        {
            public string Texto; public Color Color; public FRect Rect; public bool Bloqueado;
            public Btn(string t, Color c, FRect r, bool b = false) { Texto = t; Color = c; Rect = r; Bloqueado = b; }
        }

        protected struct FRect
        {
            public float X, Y, W, H;
            public FRect(float x, float y, float w, float h) { X = x; Y = y; W = w; H = h; }
            public bool HitTest(int px, int py)
            {
                float s = Main.UIScale;
                return px / s >= X && px / s <= X + W && py / s >= Y && py / s <= Y + H;
            }
            public Rectangle ToPx(float s) => new Rectangle((int)(X * s), (int)(Y * s), (int)(W * s), (int)(H * s));
        }

        private List<Btn> _botones = new();
        private int _hover = -1, _hoverPrev = -1;
        private bool _mouseWasDown;
        private Rectangle _itemHoverRect = Rectangle.Empty;

        private FRect Panel
        {
            get
            {
                float s = Main.UIScale, sw = Main.screenWidth / s, sh = Main.screenHeight / s;
                return new FRect((sw - W) / 2f, sh * 0.76f - H / 2f, W, H);
            }
        }

        private MisionData MisionActualData()
        {
            var ms = ObtenerMisiones(); int mi = ObtenerPlayer().MisionActual;
            return mi >= 0 && mi < ms.Length ? ms[mi] : null;
        }

        private LineaDialogo[] LineasPresentacion()
        {
            var m = MisionActualData(); return m?.DialogosPresentacion ?? System.Array.Empty<LineaDialogo>();
        }

        private LineaDialogo[] LineasCompletacion()
        {
            var m = MisionActualData(); return m?.DialogosCompletacion ?? System.Array.Empty<LineaDialogo>();
        }

        private bool EsUltimaCompletacion() => _completacionIndex >= LineasCompletacion().Length - 1;
        private bool EsUltimaPresentacion() => _presentacionIndex >= LineasPresentacion().Length - 1;

        private bool SiguienteEsJugador(LineaDialogo[] ls, int idx)
        {
            int next = idx + 1;
            return next < ls.Length && ls[next].EsJugador;
        }

        private bool LineaActualEsJugador()
        {
            var ls = LineasCompletacion();
            return _completacionIndex < ls.Length && ls[_completacionIndex].EsJugador;
        }

        private void CargarLinea(LineaDialogo l)
        {
            string txt = Language.GetTextValue(l.Key, Main.LocalPlayer.name, NombreNPC);
            string pre = l.EsJugador
                ? (string.IsNullOrEmpty(l.NombreNPC) ? Main.LocalPlayer.name : l.NombreNPC)
                : (string.IsNullOrEmpty(l.NombreNPC) ? NombreNPC : l.NombreNPC);
            _lineasMostradas.Add((pre, txt, l.EsJugador));
            _textoFull = string.IsNullOrWhiteSpace(txt) ? "..." : txt;
            _textoVis = ""; _timerLetra = 0;
        }

        private void CargarLineaPresentacion()
        {
            var ls = LineasPresentacion();
            if (_presentacionIndex < ls.Length) CargarLinea(ls[_presentacionIndex]);
        }

        private void CargarLineaCompletacion()
        {
            var ls = LineasCompletacion();
            if (_completacionIndex < ls.Length) CargarLinea(ls[_completacionIndex]);
        }

        private void AvanzarPresentacion()
        {
            FinalizarLineaActual();
            _presentacionIndex++;
            CargarLineaPresentacion();
            RefreshBotones();
        }

        private void AvanzarCompletacion()
        {
            FinalizarLineaActual();
            _completacionIndex++;
            CargarLineaCompletacion();
            RefreshBotones();
        }

        private void FinalizarLineaActual()
        {
            _textoVis = _textoFull;
            if (_lineasMostradas.Count > 0)
            {
                var u = _lineasMostradas[^1];
                _lineasMostradas[^1] = (u.nombre, _textoFull, u.esJugador);
            }
        }

        public void IniciarConNPC(NPC npc)
        {
            EstaAbierta = true; _npc = npc; _mouseWasDown = true;

            var p = ObtenerPlayer();
            if (p.CompletacionPendiente)
            {
                var m = MisionActualData();
                if (m == null || (m.YaFueCompletada != null && m.YaFueCompletada()))
                {
                    p.CompletacionPendiente = false; p.UIAbierta = false;
                    EstaAbierta = false; return;
                }
                _pantalla = PantallaUI.MisionDialogo;
                _completacionIndex = 0; _lineasMostradas.Clear();
                CargarLineaCompletacion(); RefreshBotones(); return;
            }

            _pantalla = PantallaUI.Menu;
            SetTexto(ObtenerDialogoChat()); RefreshBotones();
        }

        private void SetTexto(string t) { _textoFull = string.IsNullOrWhiteSpace(t) ? "..." : t; _textoVis = ""; _timerLetra = 0; }
        private void TerminarTexto() => _textoVis = _textoFull;

        private void AgregarBtn(string lit, Color c, bool b = false) => _botones.Add(new Btn(lit, c, default, b));
        private void AgregarBtnKey(string k, Color c, bool b = false) => AgregarBtn(Language.GetTextValue(k), c, b);

        private void RefreshBotones()
        {
            _botones.Clear();
            switch (_pantalla)
            {
                case PantallaUI.Menu: CargarBotonesMenu(); break;
                case PantallaUI.MisionPresentacion: CargarBotonesPresentacion(); break;
                case PantallaUI.MisionObjetivo: CargarBotonesObjetivo(); break;
                case PantallaUI.MisionDialogo: CargarBotonesDialogo(); break;
            }
            RecalcularBotones();
        }

        private void CargarBotonesMenu()
        {
            var player = ObtenerPlayer(); int fase = player.Fase;
            AgregarBtnKey("Mods.Novias.UI.Tienda", CBotonTienda);
            if (fase >= 1) AgregarBtn(player.EstaSiguiendo
                ? Language.GetTextValue("Mods.Novias.UI.DejarSeguir")
                : Language.GetTextValue("Mods.Novias.UI.Seguir"), CBotonSeguir);
            if (fase >= 2) AgregarBtnKey("Mods.Novias.UI.Besar", CBotonBeso);
            var ms = ObtenerMisiones(); int mIdx = player.MisionActual;
            if (mIdx >= 0 && mIdx < ms.Length)
            {
                bool disp = ms[mIdx].EstaDisponible();
                AgregarBtnKey("Mods.Novias.UI.Mision", disp ? CBotonMision : CBotonBloqueado, false);
            }
            AgregarBtnKey("Mods.Novias.UI.Dialogo", CBotonDialogo);
            AgregarBtnKey("Mods.Novias.UI.Cerrar", CBotonCerrar);
        }

        private void CargarBotonesPresentacion()
        {
            var ls = LineasPresentacion();
            AgregarBtnKey("Mods.Novias.UI.Volver", CBotonVolver);
            if (!TextoListo)
                AgregarBtnKey("Mods.Novias.UI.Saltar", CBotonSiguiente);
            else if (EsUltimaPresentacion())
                AgregarBtnKey("Mods.Novias.UI.AceptarMision", CBotonSiguiente);
            else if (SiguienteEsJugador(ls, _presentacionIndex))
                AgregarBtnKey("Mods.Novias.UI.Hablar", CBotonHablar);
            else
                AgregarBtnKey("Mods.Novias.UI.Siguiente", CBotonSiguiente);
        }

        private void CargarBotonesObjetivo()
        {
            var m = MisionActualData(); if (m == null) return;
            bool tiene = m.CondicionCompletar != null ? m.PuedeCompletar()
                       : (m.ItemRequisito == 0 || Main.LocalPlayer.CountItem(m.ItemRequisito) >= m.CantidadRequisito);
            AgregarBtnKey("Mods.Novias.UI.Cerrar", CBotonCerrar);
            AgregarBtnKey("Mods.Novias.UI.Completar", tiene ? CBotonCompletar : CBotonBloqueado, !tiene);
        }

        private void CargarBotonesDialogo()
        {
            AgregarBtnKey("Mods.Novias.UI.Cerrar", CBotonCerrar);
            if (!TextoListo) { AgregarBtnKey("Mods.Novias.UI.Saltar", CBotonSiguiente); return; }
            if (EsUltimaCompletacion()) { AgregarBtnKey("Mods.Novias.UI.Completar", CBotonCompletar); return; }
            var ls = LineasCompletacion();
            if (SiguienteEsJugador(ls, _completacionIndex)) AgregarBtnKey("Mods.Novias.UI.Hablar", CBotonHablar);
            else AgregarBtnKey("Mods.Novias.UI.Siguiente", CBotonSiguiente);
        }

        private void RecalcularBotones()
        {
            if (_botones.Count == 0) return;
            var p = Panel;
            float bw = (p.W - PAD * 2 - GAP * (_botones.Count - 1)) / _botones.Count;
            float by = p.Y + H - PAD - B_H - 4f;
            for (int i = 0; i < _botones.Count; i++)
            {
                var b = _botones[i];
                _botones[i] = new Btn(b.Texto, b.Color, new FRect(p.X + PAD + i * (bw + GAP), by, bw, B_H), b.Bloqueado);
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (!EstaAbierta || Main.LocalPlayer.talkNPC == -1) return;
            NPC npc = Main.npc[Main.LocalPlayer.talkNPC];
            bool curLeft = Mouse.GetState().LeftButton == ButtonState.Pressed;

            bool esDialogo = _pantalla == PantallaUI.MisionPresentacion || _pantalla == PantallaUI.MisionDialogo;

            if (!TextoListo)
            {
                _timerLetra++;
                if (_timerLetra >= VEL_LETRA)
                {
                    _timerLetra = 0; _textoVis = _textoFull[..(_textoVis.Length + 1)];
                    if (_textoVis.Length % 2 == 0) SoundEngine.PlaySound(SoundID.MenuTick with { Volume = 0.15f, Pitch = 0.5f });
                }
                if (esDialogo) RefreshBotones();
            }
            else if (esDialogo || _pantalla == PantallaUI.MisionObjetivo) RefreshBotones();

            RecalcularBotones();

            if (_pantalla == PantallaUI.MisionObjetivo && _itemHoverRect != Rectangle.Empty && _itemHoverRect.Contains(Main.mouseX, Main.mouseY))
            {
                var m = MisionActualData();
                if (m?.ItemRequisito != 0)
                {
                    Main.LocalPlayer.mouseInterface = true;
                    Item t = new Item(); t.SetDefaults(m.ItemRequisito);
                    Main.HoverItem = t; Main.hoverItemName = t.Name; Main.mouseText = true;
                }
            }

            _hoverPrev = _hover; _hover = -1;
            for (int i = 0; i < _botones.Count; i++)
                if (!_botones[i].Bloqueado && _botones[i].Rect.HitTest(Main.mouseX, Main.mouseY)) _hover = i;
            if (_hover != _hoverPrev && _hover >= 0) SoundEngine.PlaySound(SoundID.MenuTick with { Volume = 0.22f, Pitch = 0.15f });
            if (_hover >= 0) { Main.LocalPlayer.mouseInterface = true; Main.mouseLeft = false; Main.mouseRight = false; Main.mouseLeftRelease = false; }

            if (curLeft && !_mouseWasDown)
            {
                if (_hover >= 0) { SoundEngine.PlaySound(SoundID.MenuTick); TerminarTexto(); OnClick(_botones[_hover].Texto, npc); }
                else if (!TextoListo && _pantalla == PantallaUI.MisionDialogo && !LineaActualEsJugador())
                { TerminarTexto(); SoundEngine.PlaySound(SoundID.MenuTick with { Volume = 0.3f, Pitch = 0.2f }); RefreshBotones(); }
                else if (!TextoListo && esDialogo)
                { TerminarTexto(); SoundEngine.PlaySound(SoundID.MenuTick with { Volume = 0.3f, Pitch = 0.2f }); RefreshBotones(); }
            }
            _mouseWasDown = curLeft;
        }

        private void OnClick(string texto, NPC npc)
        {
            string Txt(string k) => Language.GetTextValue(k);

            if (_pantalla == PantallaUI.MisionDialogo)
            {
                if (texto == Txt("Mods.Novias.UI.Cerrar"))
                {
                    SoundEngine.PlaySound(SoundID.MenuClose);
                    ObtenerPlayer().CompletacionPendiente = true;
                    CerrarChat(); return;
                }
                if (texto == Txt("Mods.Novias.UI.Saltar")) { FinalizarLineaActual(); RefreshBotones(); return; }
                if (texto == Txt("Mods.Novias.UI.Siguiente") || texto == Txt("Mods.Novias.UI.Hablar")) { AvanzarCompletacion(); return; }
                if (texto == Txt("Mods.Novias.UI.Completar")) { CompletarMisionUI(); return; }
                return;
            }

            if (_pantalla == PantallaUI.MisionPresentacion)
            {
                if (texto == Txt("Mods.Novias.UI.Volver"))
                {
                    SoundEngine.PlaySound(SoundID.MenuClose);
                    _pantalla = PantallaUI.Menu; _lineasMostradas.Clear(); _presentacionIndex = 0;
                    SetTexto(ObtenerDialogoChat()); RefreshBotones(); return;
                }
                if (texto == Txt("Mods.Novias.UI.Saltar")) { FinalizarLineaActual(); RefreshBotones(); return; }
                if (texto == Txt("Mods.Novias.UI.Siguiente") || texto == Txt("Mods.Novias.UI.Hablar")) { AvanzarPresentacion(); return; }
                if (texto == Txt("Mods.Novias.UI.AceptarMision"))
                {
                    var p2 = ObtenerPlayer(); var m2 = MisionActualData();
                    _lineasMostradas.Clear(); _presentacionIndex = 0;
                    p2.UIAbierta = true; m2?.OnAceptar?.Invoke();
                    _pantalla = PantallaUI.MisionObjetivo;
                    SetTexto(Language.GetTextValue(m2.DescripcionKey));
                    RefreshBotones(); return;
                }
                return;
            }

            if (_pantalla == PantallaUI.MisionObjetivo)
            {
                if (texto == Txt("Mods.Novias.UI.Cerrar")) { SoundEngine.PlaySound(SoundID.MenuClose); CerrarChat(); return; }
                if (texto == Txt("Mods.Novias.UI.Completar"))
                {
                    var m3 = MisionActualData(); var p3 = ObtenerPlayer();
                    if (m3.OnCompletar == null && m3.ItemRequisito != 0)
                        for (int i = 0; i < Main.LocalPlayer.inventory.Length; i++)
                        {
                            var it = Main.LocalPlayer.inventory[i];
                            if (it.type == m3.ItemRequisito && it.stack >= m3.CantidadRequisito)
                            { it.stack -= m3.CantidadRequisito; if (it.stack <= 0) it.TurnToAir(); break; }
                        }
                    if (m3.DialogosCompletacion.Length > 0)
                    {
                        p3.CompletacionPendiente = true; m3.OnIniciarCompletacion?.Invoke();
                        _completacionIndex = 0; _lineasMostradas.Clear();
                        _pantalla = PantallaUI.MisionDialogo;
                        CargarLineaCompletacion(); RefreshBotones(); return;
                    }
                    FinalizarMision(m3, p3);
                }
                return;
            }

            if (_pantalla == PantallaUI.Menu)
            {
                if (texto == Txt("Mods.Novias.UI.Mision"))
                {
                    var ms4 = ObtenerMisiones(); var p4 = ObtenerPlayer(); int mi4 = p4.MisionActual;
                    if (mi4 < 0 || mi4 >= ms4.Length) return;
                    if (!ms4[mi4].EstaDisponible())
                    {
                        string k = ms4[mi4].MensajeBloqueadoKey;
                        SetTexto(!string.IsNullOrEmpty(k) ? Language.GetTextValue(k) : Language.GetTextValue("Mods.Novias.UI.MisionBloqueada"));
                        return;
                    }
                    if (p4.CompletacionPendiente)
                    {
                        _completacionIndex = 0; _lineasMostradas.Clear();
                        _pantalla = PantallaUI.MisionDialogo;
                        CargarLineaCompletacion(); RefreshBotones(); return;
                    }
                    if (p4.UIAbierta)
                    {
                        _pantalla = PantallaUI.MisionObjetivo;
                        SetTexto(Language.GetTextValue(ms4[mi4].DescripcionKey));
                    }
                    else
                    {
                        _pantalla = PantallaUI.MisionPresentacion;
                        _presentacionIndex = 0; _lineasMostradas.Clear();
                        CargarLineaPresentacion();
                    }
                    RefreshBotones(); return;
                }
                if (texto == Txt("Mods.Novias.UI.Seguir") || texto == Txt("Mods.Novias.UI.DejarSeguir"))
                {
                    var p5 = ObtenerPlayer(); p5.EstaSiguiendo = !p5.EstaSiguiendo;
                    SetTexto(p5.EstaSiguiendo ? ObtenerDialogoSeguir() : ObtenerDialogoDejarSeguir());
                    RefreshBotones(); return;
                }
                if (texto == Txt("Mods.Novias.UI.Besar"))
                {
                    Main.LocalPlayer.AddBuff(BuffBeso, 60 * 60 * 10);
                    SoundEngine.PlaySound(SoundID.Item17 with { Pitch = 0.3f, Volume = 0.8f });
                    _npc.Center = Main.LocalPlayer.Center + new Vector2(Main.LocalPlayer.direction * 20f, 0f);
                    _npc.direction = _npc.spriteDirection = -Main.LocalPlayer.direction;
                    _npc.AddBuff(BuffID.Lovestruck, 60 * 5);
                    SetTexto(ObtenerDialogoBeso()); return;
                }
                if (texto == Txt("Mods.Novias.UI.Dialogo"))
                {
                    if (npc != null && npc.active)
                    {
                        string npcName = npc.ModNPC?.GetType().Name ?? "";
                        int total = 5;
                        int idx = Main.rand.Next(total);
                        string key = $"Mods.Novias.NPCfelicidad.{npcName}.Felicidad{idx}";
                        string chat = Language.GetTextValue(key);

                        if (chat == key)
                        {
                            NPCLoader.GetChat(npc, ref chat);
                        }

                        SetTexto(chat);
                    }
                    return;
                }
                if (texto == Txt("Mods.Novias.UI.Tienda"))
                {
                    string sn = ""; npc.ModNPC.OnChatButtonClicked(true, ref sn);
                    if (!string.IsNullOrEmpty(sn))
                    {
                        var f = typeof(Main).GetField("<npcShop>k__BackingField", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
                        if (f != null)
                        {
                            var sk = NPCShopDatabase.GetShopName(npc.type, sn);
                            if (NPCShopDatabase.TryGetNPCShop(sk, out var aS))
                            {
                                var fm = aS.GetType().GetMethod("FillShop", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance, null, new System.Type[] { typeof(Item[]), typeof(NPC), typeof(bool).MakeByRefType() }, null);
                                for (int i = 0; i < Main.instance.shop[1].item.Length; i++) Main.instance.shop[1].item[i] = new Item();
                                if (fm != null) { bool ov = false; fm.Invoke(aS, new object[] { Main.instance.shop[1].item, npc, ov }); }
                            }
                            f.SetValue(null, 1); Main.playerInventory = true;
                        }
                    }
                    return;
                }
                if (texto == Txt("Mods.Novias.UI.Cerrar")) { SoundEngine.PlaySound(SoundID.MenuClose); CerrarChat(); }
            }
        }

        private void CerrarChat()
        {
            Main.LocalPlayer.SetTalkNPC(-1); Main.npcChatText = ""; Main.npcChatCornerItem = 0;
            EstaAbierta = false;
        }

        private void LimpiarEstadoCompletacion()
        {
            _completacionIndex = 0; _lineasMostradas.Clear();
        }

        private void CompletarMisionUI()
        {
            var m = MisionActualData(); var p = ObtenerPlayer();

            if (m.OnCompletar != null)
            {
                m.OnCompletar();
                LimpiarEstadoCompletacion();
                SoundEngine.PlaySound(SoundID.Item4);
                CombatText.NewText(Main.LocalPlayer.getRect(), ColorBorde, Language.GetTextValue("Mods.Novias.UI.MisionCompleta"), dramatic: true);
                CerrarChat(); return;
            }

            if (m.ItemRecompensa != 0)
                Main.LocalPlayer.QuickSpawnItem(Main.LocalPlayer.GetSource_FromThis(), m.ItemRecompensa, m.CantidadRecompensa);

            if (m.AvanzaFase) p.CompletarMision();
            else p.AvanzarMisionSinFase();

            m.OnMensajesCompletacion?.Invoke();

            p.CompletacionPendiente = false; p.UIAbierta = false;
            LimpiarEstadoCompletacion();
            SoundEngine.PlaySound(SoundID.Item4);
            CombatText.NewText(Main.LocalPlayer.getRect(), ColorBorde, Language.GetTextValue("Mods.Novias.UI.MisionCompleta"), dramatic: true);
            CerrarChat();
        }

        private void FinalizarMision(MisionData m, NoviasPlayerBase p)
        {
            m.OnMensajesCompletacion?.Invoke();
            p.UIAbierta = false;
            if (m.AvanzaFase) p.CompletarMision();
            else p.AvanzarMisionSinFase();
            _lineasMostradas.Clear();
            SoundEngine.PlaySound(SoundID.Item4);
            CombatText.NewText(Main.LocalPlayer.getRect(), ColorBorde, Language.GetTextValue("Mods.Novias.UI.MisionCompleta"), dramatic: true);
            CerrarChat();
        }

        protected override void DrawSelf(SpriteBatch sb)
        {
            if (!EstaAbierta) return;
            _s = Main.UIScale;
            var p = Panel; _pPx = p.ToPx(_s);
            var px = TextureAssets.MagicPixel.Value;
            var font = FontAssets.MouseText.Value;

            sb.Draw(px, _pPx, ColorFondo * 0.97f);
            Borde(sb, px, _pPx, ColorBorde, System.Math.Max(2, (int)(2 * _s)));
            sb.Draw(px, new Rectangle((int)((p.X + PAD) * _s), (int)((p.Y + T_H) * _s), (int)((p.W - PAD * 2) * _s), System.Math.Max(1, (int)(_s))), ColorBorde * 0.45f);

            string titulo = _pantalla == PantallaUI.Menu ? (_npc?.GivenOrTypeName ?? "") : ObtenerTituloMision();
            float esT = 1.1f * _s, twT = font.MeasureString(titulo).X * esT;
            Utils.DrawBorderString(sb, titulo, new Vector2(_pPx.X + (_pPx.Width - twT) / 2f, _pPx.Y + 6 * _s), ColorTitulo, esT);

            if (_pantalla == PantallaUI.MisionDialogo || _pantalla == PantallaUI.MisionPresentacion)
                DibujarLineas(sb, font);
            else
            {
                string tm = _textoVis + (!TextoListo && (int)(Main.GameUpdateCount / 20) % 2 == 0 ? "|" : "");
                DibujarTexto(sb, font, tm, new Vector2(_pPx.X + PAD * _s, _pPx.Y + (T_H + 8f) * _s), (int)((p.W - PAD * 2) * _s), 0.93f * _s);

                if (_pantalla == PantallaUI.MisionObjetivo)
                {
                    DibujarSpriteItem(sb);
                    var mContador = MisionActualData();
                    if (mContador?.ObtenerContador != null)
                    {
                        string lineaContador = mContador.ObtenerContador();
                        float escala = 0.93f * _s;
                        float yContador = _pPx.Y + (H - PAD - B_H - 42f) * _s;
                        Utils.DrawBorderString(sb, lineaContador, new Vector2(_pPx.X + PAD * _s, yContador), Color.White, escala);
                    }
                }
            }

            if (TextoListo && _textoFull.Length > 1 && _pantalla != PantallaUI.MisionDialogo && _pantalla != PantallaUI.MisionPresentacion)
            {
                float esI = 0.75f * _s; string ind = "▼"; float twI = font.MeasureString(ind).X * esI;
                Utils.DrawBorderString(sb, ind, new Vector2(_pPx.X + _pPx.Width - PAD * _s - twI, _pPx.Y + (H - PAD - B_H - 26f) * _s), ColorBorde * 0.8f, esI);
            }

            for (int i = 0; i < _botones.Count; i++)
            {
                var b = _botones[i]; var bPx = b.Rect.ToPx(_s);
                bool hov = i == _hover, bloq = b.Bloqueado; var col = bloq ? CBotonBloqueado : b.Color;
                sb.Draw(px, bPx, hov ? col : col * 0.75f);
                Borde(sb, px, bPx, hov && !bloq ? Color.White : col, System.Math.Max(1, (int)(2 * _s)));
                float esB = 0.88f * _s, twB = font.MeasureString(b.Texto).X * esB, thB = font.MeasureString(b.Texto).Y * esB;
                Utils.DrawBorderString(sb, b.Texto, new Vector2(bPx.X + (bPx.Width - twB) / 2f, bPx.Y + (bPx.Height - thB) / 2f + 2f * _s), bloq ? Color.Gray : Color.White, esB);
            }
        }

        private void DibujarLineas(SpriteBatch sb, DynamicSpriteFont font)
        {
            if (_lineasMostradas.Count == 0) return;
            var p = Panel;
            float escala = 0.93f * _s, lineH = font.MeasureString("A").Y * escala;
            float xLeft = _pPx.X + PAD * _s, xRight = _pPx.X + _pPx.Width - PAD * _s;
            float yTop = _pPx.Y + (T_H + 8f) * _s;
            int anchoMax = (int)((p.W - PAD * 2) * _s);

            var (nombre, _, esJugador) = _lineasMostradas[^1];
            Color colorNom = ResolverColorLinea(nombre, esJugador);

            string label = nombre + ":";
            float xNombre = esJugador ? xRight - font.MeasureString(label).X * escala : xLeft;
            Utils.DrawBorderString(sb, label, new Vector2(xNombre, yTop), colorNom, escala);

            string tm = _textoVis + (!TextoListo && (int)(Main.GameUpdateCount / 20) % 2 == 0 ? "|" : "");
            if (esJugador)
                DibujarDerechaAbajo(sb, font, tm, xRight, yTop + lineH, anchoMax, escala, Color.White);
            else
                DibujarTexto(sb, font, tm, new Vector2(xLeft, yTop + lineH), anchoMax, escala);
        }

        private Color ResolverColorLinea(string nombre, bool esJugador)
        {
            if (esJugador) return ColorDialogoJugador;

            LineaDialogo[] ls;
            int idx;
            if (_pantalla == PantallaUI.MisionPresentacion)
            { ls = LineasPresentacion(); idx = _presentacionIndex; }
            else
            { ls = LineasCompletacion(); idx = _completacionIndex; }

            int lineaPos = _lineasMostradas.Count - 1;
            if (lineaPos >= 0 && lineaPos < ls.Length && ls[lineaPos].ColorNombre.HasValue)
                return ls[lineaPos].ColorNombre.Value;

            return ColorParaNombre(nombre);
        }

        private string ObtenerTituloMision()
        {
            var ms = ObtenerMisiones(); int i = ObtenerPlayer().MisionActual;
            if (i < 0 || i >= ms.Length) return _npc?.GivenOrTypeName ?? "";
            return Language.GetTextValue(ms[i].TituloKey);
        }

        private void DibujarSpriteItem(SpriteBatch sb)
        {
            var m = MisionActualData(); if (m?.ItemRequisito == 0) return;
            var tex = TextureAssets.Item[m.ItemRequisito].Value; if (tex == null) return;
            var font = FontAssets.MouseText.Value;
            string lbl = $"x{m.CantidadRequisito}"; float esL = 0.85f * _s, lW = font.MeasureString(lbl).X * esL, lH = font.MeasureString(lbl).Y * esL;
            float sc = System.Math.Min(1f, 24f / System.Math.Max(tex.Width, tex.Height)) * _s;
            int iW = (int)(tex.Width * sc), iH = (int)(tex.Height * sc);
            float x = _pPx.X + PAD * _s, y = _pPx.Y + (H - PAD - B_H - iH - 14f) * _s;
            _itemHoverRect = new Rectangle((int)x, (int)y, (int)(iW + 6 * _s + lW), iH + 4);
            sb.Draw(tex, new Vector2(x, y), null, Color.White, 0f, Vector2.Zero, sc, SpriteEffects.None, 0f);
            Utils.DrawBorderString(sb, lbl, new Vector2(x + iW + 6 * _s, y + (iH - lH) / 2f), Color.White, esL);
        }

        private static void DibujarDerechaAbajo(SpriteBatch sb, DynamicSpriteFont font, string texto, float xRight, float y0, float anchoMax, float escala, Color color)
        {
            if (string.IsNullOrEmpty(texto)) return;
            float lH = font.MeasureString("A").Y * escala, y = y0;
            var ls = new List<string>(); string l = "";
            foreach (string w in texto.Split(' ')) { string pr = l.Length > 0 ? l + " " + w : w; if (font.MeasureString(pr).X * escala > anchoMax) { if (l.Length > 0) ls.Add(l); l = w; } else l = pr; }
            if (l.Length > 0) ls.Add(l);
            foreach (string ln in ls)
            {
                float lw = font.MeasureString(ln).X * escala;
                var sn = ChatManager.ParseMessage(ln, color).ToArray(); ChatManager.ConvertNormalSnippets(sn);
                ChatManager.DrawColorCodedStringShadow(sb, font, sn, new Vector2(xRight - lw, y), Color.Black * 0.6f, 0f, Vector2.Zero, new Vector2(escala));
                ChatManager.DrawColorCodedString(sb, font, sn, new Vector2(xRight - lw, y), color, 0f, Vector2.Zero, new Vector2(escala), out _, -1f);
                y += lH;
            }
        }

        private static void DibujarTexto(SpriteBatch sb, DynamicSpriteFont font, string texto, Vector2 pos, int anchoMaxPx, float escala, Color? col = null)
        {
            if (string.IsNullOrEmpty(texto)) return;
            float lH = font.MeasureString("A").Y * escala, y = pos.Y; string l = ""; Color c = col ?? Color.White;
            void Flush() { if (string.IsNullOrEmpty(l)) return; var sn = ChatManager.ParseMessage(l, c).ToArray(); ChatManager.ConvertNormalSnippets(sn); ChatManager.DrawColorCodedStringShadow(sb, font, sn, new Vector2(pos.X, y), Color.Black * 0.6f, 0f, Vector2.Zero, new Vector2(escala)); ChatManager.DrawColorCodedString(sb, font, sn, new Vector2(pos.X, y), c, 0f, Vector2.Zero, new Vector2(escala), out _, -1f); y += lH; l = ""; }
            foreach (string w in texto.Split(' ')) { string pr = l.Length > 0 ? l + " " + w : w; if (font.MeasureString(pr).X * escala > anchoMaxPx) Flush(); l = l.Length > 0 ? l + " " + w : w; }
            Flush();
        }

        private static void Borde(SpriteBatch sb, Texture2D px, Rectangle r, Color c, int g)
        {
            sb.Draw(px, new Rectangle(r.X, r.Y, r.Width, g), c);
            sb.Draw(px, new Rectangle(r.X, r.Bottom - g, r.Width, g), c);
            sb.Draw(px, new Rectangle(r.X, r.Y, g, r.Height), c);
            sb.Draw(px, new Rectangle(r.Right - g, r.Y, g, r.Height), c);
        }
    }
}