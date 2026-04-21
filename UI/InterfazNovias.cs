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

        public override void Unload()
        {
            _state = null;
            _ui = null;
        }

        public override void UpdateUI(GameTime gameTime)
        {
            Player player = Main.LocalPlayer;
            if (player.talkNPC != -1)
            {
                NPC npc = Main.npc[player.talkNPC];
                if (EsEstaNovia(npc))
                {
                    if (_ui.CurrentState == null) _ui.SetState(_state);
                }
                else if (_ui.CurrentState != null)
                {
                    _ui.SetState(null);
                    _state.EstaAbierta = false;
                }
            }
            else if (_ui.CurrentState != null)
            {
                _ui.SetState(null);
                _state.EstaAbierta = false;
            }
            if (_ui?.CurrentState != null) _ui.Update(gameTime);
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            if (Main.LocalPlayer.talkNPC != -1 && EsEstaNovia(Main.npc[Main.LocalPlayer.talkNPC]))
            {
                int ic = layers.FindIndex(l => l.Name == "Vanilla: NPC / Sign Dialog");
                if (ic >= 0)
                    layers[ic] = new LegacyGameInterfaceLayer("Vanilla: NPC / Sign Dialog", () => true, InterfaceScaleType.UI);
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

        private string _textoFull = "";
        private string _textoVis = "";
        private int _timerLetra;
        private const int VEL_LETRA = 2;
        private bool TextoListo => _textoVis.Length >= _textoFull.Length;

        private PantallaUI _pantalla = PantallaUI.Menu;
        private int _dialogoIndex = 0;
        private int _completacionIndex = 0;
        private bool _completacionPendiente = false;

        private List<(string nombre, string textoFull, bool esJugador)> _lineasMostradas = new();

        private Rectangle _pPx;
        private float _s;

        private const float W = 580f;
        private const float H = 220f;
        private const float PAD = 18f;
        private const float T_H = 38f;
        private const float B_H = 34f;
        private const float GAP = 6f;
        private const float SB_W = 8f;
        private const float SB_PAD = 4f;

        protected virtual Color ColorFondo => new Color(28, 16, 38);
        protected virtual Color ColorBorde => new Color(200, 90, 150);
        protected virtual Color ColorTitulo => new Color(255, 190, 230);

        protected abstract NoviasPlayerBase ObtenerPlayer();
        protected abstract MisionData[] ObtenerMisiones();
        protected abstract int BuffBeso { get; }
        protected virtual string ObtenerDialogoChat() => "...";
        protected virtual string ObtenerDialogoBeso() => "...";
        protected virtual string ObtenerDialogoSeguir() => "...";
        protected virtual string ObtenerDialogoDejarSeguir() => "...";
        protected virtual string NombreNPC => _npc?.GivenOrTypeName ?? "";

        private static readonly Color CBotonVolver = new Color(192, 40, 40);
        private static readonly Color CBotonSiguiente = new Color(200, 150, 30);
        private static readonly Color CBotonCerrar = new Color(192, 40, 40);
        private static readonly Color CBotonCompletar = new Color(59, 169, 69);
        private static readonly Color CBotonBloqueado = new Color(80, 80, 80);
        private static readonly Color CBotonTienda = new Color(89, 116, 213);
        private static readonly Color CBotonSeguir = new Color(180, 80, 180);
        private static readonly Color CBotonBeso = new Color(220, 80, 130);
        private static readonly Color CBotonMision = new Color(200, 150, 30);
        private static readonly Color CBotonFelicidad = new Color(59, 169, 69);
        private static readonly Color CBotonHablar = new Color(59, 169, 69);
        private static readonly Color ColorDialogoJugador = new Color(180, 220, 255);
        private static readonly Color ColorDialogoNPC = new Color(255, 200, 230);

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
            public Rectangle ToPx(float s) =>
                new Rectangle((int)(X * s), (int)(Y * s), (int)(W * s), (int)(H * s));
        }

        private List<Btn> _botones = new();
        private int _hover = -1, _hoverPrev = -1;
        private bool _mouseWasDown;
        private Rectangle _itemHoverRect = Rectangle.Empty;

        private FRect Panel
        {
            get
            {
                float s = Main.UIScale;
                float sw = Main.screenWidth / s;
                float sh = Main.screenHeight / s;
                return new FRect((sw - W) / 2f, sh * 0.76f - H / 2f, W, H);
            }
        }

        private LineaDialogo[] ObtenerLineas()
        {
            var ms = ObtenerMisiones();
            var p = ObtenerPlayer();
            int mi = p.MisionActual;
            if (mi < 0 || mi >= ms.Length) return System.Array.Empty<LineaDialogo>();
            return ms[mi].DialogosCompletacion;
        }

        private bool EsUltimaLinea() => _completacionIndex >= ObtenerLineas().Length - 1;

        private bool LineaActualEsJugador()
        {
            var ls = ObtenerLineas();
            if (_completacionIndex >= ls.Length) return false;
            return ls[_completacionIndex].EsJugador;
        }

        private bool SiguienteEsJugador()
        {
            var ls = ObtenerLineas();
            int next = _completacionIndex + 1;
            if (next >= ls.Length) return false;
            return ls[next].EsJugador;
        }

        private void CargarLineaDialogo()
        {
            var ls = ObtenerLineas();
            if (_completacionIndex >= ls.Length) return;

            var l = ls[_completacionIndex];
            string txt = Language.GetTextValue(l.Key, Main.LocalPlayer.name, NombreNPC);
            string pre = l.EsJugador ? Main.LocalPlayer.name : NombreNPC;

            _lineasMostradas.Add((pre, txt, l.EsJugador));
            SetTextoDialogo(txt);
        }

        private void SetTextoDialogo(string txt)
        {
            _textoFull = string.IsNullOrWhiteSpace(txt) ? "..." : txt;
            _textoVis = "";
            _timerLetra = 0;
        }

        private void AvanzarALineaSiguiente()
        {
            if (_lineasMostradas.Count > 0)
            {
                var u = _lineasMostradas[^1];
                _lineasMostradas[^1] = (u.nombre, _textoFull, u.esJugador);
            }
            _textoVis = _textoFull;
            _completacionIndex++;
            CargarLineaDialogo();
            RefreshBotones();
        }

        public void IniciarConNPC(NPC npc)
        {
            EstaAbierta = true; _npc = npc; _mouseWasDown = true;

            if (_completacionPendiente)
            {
                _pantalla = PantallaUI.MisionDialogo;
                _completacionIndex = 0;
                _lineasMostradas.Clear();
                CargarLineaDialogo();
                RefreshBotones();
                return;
            }

            _pantalla = PantallaUI.Menu; _dialogoIndex = 0;
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
            if (fase >= 1) AgregarBtn(player.EstaSiguiendo ? Language.GetTextValue("Mods.Novias.UI.DejarSeguir") : Language.GetTextValue("Mods.Novias.UI.Seguir"), CBotonSeguir);
            if (fase >= 2) AgregarBtnKey("Mods.Novias.UI.Besar", CBotonBeso);
            if (fase < 3) AgregarBtnKey("Mods.Novias.UI.Mision", CBotonMision);
            AgregarBtnKey("Mods.Novias.UI.Felicidad", CBotonFelicidad);
            AgregarBtnKey("Mods.Novias.UI.Cerrar", CBotonCerrar);
        }

        private void CargarBotonesPresentacion()
        {
            var ms = ObtenerMisiones(); var p = ObtenerPlayer(); int i = p.MisionActual;
            if (i < 0 || i >= ms.Length) return;
            bool ul = _dialogoIndex >= ms[i].DialogosKey.Length - 1;
            AgregarBtnKey("Mods.Novias.UI.Volver", CBotonVolver);
            if (!TextoListo) AgregarBtnKey("Mods.Novias.UI.Saltar", CBotonSiguiente);
            else AgregarBtnKey(ul ? "Mods.Novias.UI.AceptarMision" : "Mods.Novias.UI.Siguiente", CBotonSiguiente);
        }

        private void CargarBotonesObjetivo()
        {
            var ms = ObtenerMisiones(); var p = ObtenerPlayer(); int i = p.MisionActual;
            if (i < 0 || i >= ms.Length) return;
            var m = ms[i];
            bool tiene = m.ItemRequisito == 0 || Main.LocalPlayer.CountItem(m.ItemRequisito) >= m.CantidadRequisito;
            AgregarBtnKey("Mods.Novias.UI.Cerrar", CBotonCerrar);
            AgregarBtnKey("Mods.Novias.UI.Completar", tiene ? CBotonCompletar : CBotonBloqueado, !tiene);
        }

        private void CargarBotonesDialogo()
        {
            AgregarBtnKey("Mods.Novias.UI.Cerrar", CBotonCerrar);

            if (!TextoListo)
            {
                AgregarBtnKey("Mods.Novias.UI.Saltar", CBotonSiguiente);
                return;
            }

            if (EsUltimaLinea())
            {
                AgregarBtnKey("Mods.Novias.UI.Completar", CBotonCompletar);
                return;
            }

            if (SiguienteEsJugador())
                AgregarBtnKey("Mods.Novias.UI.Hablar", CBotonHablar);
            else
                AgregarBtnKey("Mods.Novias.UI.Siguiente", CBotonSiguiente);
        }

        private void RecalcularBotones()
        {
            if (_botones.Count == 0) return;
            var p = Panel;
            float aw = p.W - PAD * 2;
            float bw = (aw - GAP * (_botones.Count - 1)) / _botones.Count;
            float by = p.Y + H - PAD - B_H - 4f;

            for (int i = 0; i < _botones.Count; i++)
            {
                var b = _botones[i];
                _botones[i] = new Btn(b.Texto, b.Color,
                    new FRect(p.X + PAD + i * (bw + GAP), by, bw, B_H), b.Bloqueado);
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (!EstaAbierta || Main.LocalPlayer.talkNPC == -1) return;
            NPC npc = Main.npc[Main.LocalPlayer.talkNPC];
            bool curLeft = Mouse.GetState().LeftButton == ButtonState.Pressed;

            if (!TextoListo)
            {
                _timerLetra++;
                if (_timerLetra >= VEL_LETRA)
                {
                    _timerLetra = 0; _textoVis = _textoFull[..(_textoVis.Length + 1)];
                    if (_textoVis.Length % 2 == 0) SoundEngine.PlaySound(SoundID.MenuTick with { Volume = 0.15f, Pitch = 0.5f });
                }
                if (_pantalla == PantallaUI.MisionPresentacion || _pantalla == PantallaUI.MisionDialogo) RefreshBotones();
            }
            else if (_pantalla == PantallaUI.MisionPresentacion || _pantalla == PantallaUI.MisionDialogo) RefreshBotones();

            if (_pantalla == PantallaUI.MisionObjetivo) RefreshBotones();

            RecalcularBotones();

            if (_pantalla == PantallaUI.MisionObjetivo && _itemHoverRect != Rectangle.Empty && _itemHoverRect.Contains(Main.mouseX, Main.mouseY))
            {
                var ms = ObtenerMisiones(); var p2 = ObtenerPlayer(); int mi = p2.MisionActual;
                if (mi >= 0 && mi < ms.Length && ms[mi].ItemRequisito != 0)
                {
                    Main.LocalPlayer.mouseInterface = true;
                    Item t = new Item(); t.SetDefaults(ms[mi].ItemRequisito);
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
                {
                    TerminarTexto();
                    SoundEngine.PlaySound(SoundID.MenuTick with { Volume = 0.3f, Pitch = 0.2f });
                    RefreshBotones();
                }
                else if (!TextoListo && _pantalla != PantallaUI.MisionDialogo)
                {
                    TerminarTexto();
                    SoundEngine.PlaySound(SoundID.MenuTick with { Volume = 0.3f, Pitch = 0.2f });
                    if (_pantalla == PantallaUI.MisionPresentacion) RefreshBotones();
                }
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
                    _completacionPendiente = true;
                    Main.LocalPlayer.SetTalkNPC(-1); Main.npcChatText = ""; Main.npcChatCornerItem = 0;
                    EstaAbierta = false;
                    return;
                }

                if (texto == Txt("Mods.Novias.UI.Saltar"))
                {
                    TerminarTexto();
                    if (_lineasMostradas.Count > 0)
                    {
                        var u = _lineasMostradas[^1];
                        _lineasMostradas[^1] = (u.nombre, _textoFull, u.esJugador);
                    }
                    RefreshBotones();
                    return;
                }

                if (texto == Txt("Mods.Novias.UI.Siguiente"))
                {
                    AvanzarALineaSiguiente();
                    return;
                }

                if (texto == Txt("Mods.Novias.UI.Hablar"))
                {
                    AvanzarALineaSiguiente();
                    return;
                }

                if (texto == Txt("Mods.Novias.UI.Completar"))
                {
                    CompletarMision();
                    return;
                }
                return;
            }

            if (_pantalla == PantallaUI.MisionPresentacion)
            {
                if (texto == Txt("Mods.Novias.UI.Volver")) { SoundEngine.PlaySound(SoundID.MenuClose); _pantalla = PantallaUI.Menu; _dialogoIndex = 0; SetTexto(ObtenerDialogoChat()); RefreshBotones(); return; }
                if (texto == Txt("Mods.Novias.UI.Saltar")) { TerminarTexto(); RefreshBotones(); return; }
                var ms2 = ObtenerMisiones(); var p2 = ObtenerPlayer(); int mi2 = p2.MisionActual; var m2 = ms2[mi2];
                if (texto == Txt("Mods.Novias.UI.AceptarMision")) { p2.UIAbierta = true; _pantalla = PantallaUI.MisionObjetivo; SetTexto(Language.GetTextValue(m2.DescripcionKey)); RefreshBotones(); return; }
                if (texto == Txt("Mods.Novias.UI.Siguiente")) { _dialogoIndex++; SetTexto(Language.GetTextValue(m2.DialogosKey[_dialogoIndex])); RefreshBotones(); }
                return;
            }

            if (_pantalla == PantallaUI.MisionObjetivo)
            {
                if (texto == Txt("Mods.Novias.UI.Cerrar")) { SoundEngine.PlaySound(SoundID.MenuClose); Main.LocalPlayer.SetTalkNPC(-1); Main.npcChatText = ""; Main.npcChatCornerItem = 0; EstaAbierta = false; return; }
                if (texto == Txt("Mods.Novias.UI.Completar"))
                {
                    var ms3 = ObtenerMisiones(); var p3 = ObtenerPlayer(); int mi3 = p3.MisionActual; var m3 = ms3[mi3];
                    if (m3.ItemRequisito != 0) for (int i = 0; i < Main.LocalPlayer.inventory.Length; i++) { var it = Main.LocalPlayer.inventory[i]; if (it.type == m3.ItemRequisito && it.stack >= m3.CantidadRequisito) { it.stack -= m3.CantidadRequisito; if (it.stack <= 0) it.TurnToAir(); break; } }
                    if (m3.DialogosCompletacion.Length > 0)
                    {
                        _completacionPendiente = true; _completacionIndex = 0;
                        _lineasMostradas.Clear();
                        _pantalla = PantallaUI.MisionDialogo;
                        CargarLineaDialogo(); RefreshBotones(); return;
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
                    if (_completacionPendiente)
                    {
                        _pantalla = PantallaUI.MisionDialogo; _completacionIndex = 0;
                        _lineasMostradas.Clear();
                        CargarLineaDialogo(); RefreshBotones(); return;
                    }
                    if (p4.UIAbierta) { _pantalla = PantallaUI.MisionObjetivo; SetTexto(Language.GetTextValue(ms4[mi4].DescripcionKey)); }
                    else { _pantalla = PantallaUI.MisionPresentacion; _dialogoIndex = 0; SetTexto(Language.GetTextValue(ms4[mi4].DialogosKey[0])); }
                    RefreshBotones(); return;
                }
                if (texto == Txt("Mods.Novias.UI.Seguir") || texto == Txt("Mods.Novias.UI.DejarSeguir")) { var p5 = ObtenerPlayer(); p5.EstaSiguiendo = !p5.EstaSiguiendo; SetTexto(p5.EstaSiguiendo ? ObtenerDialogoSeguir() : ObtenerDialogoDejarSeguir()); RefreshBotones(); return; }
                if (texto == Txt("Mods.Novias.UI.Besar")) { Main.LocalPlayer.AddBuff(BuffBeso, 60 * 60 * 10); SoundEngine.PlaySound(SoundID.Item17 with { Pitch = 0.3f, Volume = 0.8f }); _npc.Center = Main.LocalPlayer.Center + new Vector2(Main.LocalPlayer.direction * 20f, 0f); _npc.direction = _npc.spriteDirection = -Main.LocalPlayer.direction; _npc.AddBuff(BuffID.Lovestruck, 60 * 5); SetTexto(ObtenerDialogoBeso()); return; }
                if (texto == Txt("Mods.Novias.UI.Felicidad")) { string tf = ""; try { var m = typeof(NPC).GetMethod("GetShoppingText", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance); if (m != null) tf = m.Invoke(npc, new object[] { Main.LocalPlayer })?.ToString() ?? ""; } catch { } if (string.IsNullOrWhiteSpace(tf)) tf = Language.GetTextValue("Mods.Novias.UI.FelicidadDefault"); SetTexto(tf); return; }
                if (texto == Txt("Mods.Novias.UI.Tienda"))
                {
                    string sn = ""; npc.ModNPC.OnChatButtonClicked(true, ref sn);
                    if (!string.IsNullOrEmpty(sn)) { var f = typeof(Main).GetField("<npcShop>k__BackingField", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static); if (f != null) { var sk = NPCShopDatabase.GetShopName(npc.type, sn); if (NPCShopDatabase.TryGetNPCShop(sk, out var aS)) { var fm = aS.GetType().GetMethod("FillShop", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance, null, new System.Type[] { typeof(Item[]), typeof(NPC), typeof(bool).MakeByRefType() }, null); for (int i = 0; i < Main.instance.shop[1].item.Length; i++) Main.instance.shop[1].item[i] = new Item(); if (fm != null) { bool ov = false; fm.Invoke(aS, new object[] { Main.instance.shop[1].item, npc, ov }); } } f.SetValue(null, 1); Main.playerInventory = true; } }
                    return;
                }
                if (texto == Txt("Mods.Novias.UI.Cerrar")) { SoundEngine.PlaySound(SoundID.MenuClose); Main.LocalPlayer.SetTalkNPC(-1); Main.npcChatText = ""; Main.npcChatCornerItem = 0; }
            }
        }

        private void CompletarMision()
        {
            var ms = ObtenerMisiones(); var p = ObtenerPlayer(); int mi = p.MisionActual; var m = ms[mi];
            if (m.ItemRecompensa != 0) Main.LocalPlayer.QuickSpawnItem(Main.LocalPlayer.GetSource_FromThis(), m.ItemRecompensa, m.CantidadRecompensa);
            string nj = Main.LocalPlayer.name, nn = NombreNPC;
            if (mi == 0) { Main.NewText(Language.GetTextValue("Mods.Novias.UI.TiendaDesbloqueada", nj, nn), 255, 215, 0); Main.NewText(Language.GetTextValue("Mods.Novias.UI.SeguimientoDesbloqueado", nj, nn), 180, 80, 220); }
            if (mi == 1) Main.NewText(Language.GetTextValue("Mods.Novias.UI.BesoDesbloqueado", nj, nn), 220, 80, 130);
            _completacionPendiente = false; _completacionIndex = 0; _lineasMostradas.Clear();
            p.UIAbierta = false; p.CompletarMision();
            Main.LocalPlayer.SetTalkNPC(-1); Main.npcChatText = ""; Main.npcChatCornerItem = 0;
            SoundEngine.PlaySound(SoundID.Item4); EstaAbierta = false;
            CombatText.NewText(Main.LocalPlayer.getRect(), ColorBorde, Language.GetTextValue("Mods.Novias.UI.MisionCompleta"), dramatic: true);
        }

        private void FinalizarMision(MisionData m, NoviasPlayerBase p)
        {
            string nj = Main.LocalPlayer.name, nn = NombreNPC; int mi = p.MisionActual;
            if (mi == 0) { Main.NewText(Language.GetTextValue("Mods.Novias.UI.TiendaDesbloqueada", nj, nn), 255, 215, 0); Main.NewText(Language.GetTextValue("Mods.Novias.UI.SeguimientoDesbloqueado", nj, nn), 180, 80, 220); }
            if (mi == 1) Main.NewText(Language.GetTextValue("Mods.Novias.UI.BesoDesbloqueado", nj, nn), 220, 80, 130);
            _lineasMostradas.Clear(); p.UIAbierta = false; p.CompletarMision();
            Main.LocalPlayer.SetTalkNPC(-1); Main.npcChatText = ""; Main.npcChatCornerItem = 0;
            SoundEngine.PlaySound(SoundID.Item4); EstaAbierta = false;
            CombatText.NewText(Main.LocalPlayer.getRect(), ColorBorde, Language.GetTextValue("Mods.Novias.UI.MisionCompleta"), dramatic: true);
        }

        protected override void DrawSelf(SpriteBatch sb)
        {
            if (!EstaAbierta) return;

            _s = Main.UIScale;
            var p = Panel;
            _pPx = p.ToPx(_s);

            var px = TextureAssets.MagicPixel.Value;
            var font = FontAssets.MouseText.Value;

            sb.Draw(px, _pPx, ColorFondo * 0.97f);
            Borde(sb, px, _pPx, ColorBorde, System.Math.Max(2, (int)(2 * _s)));
            sb.Draw(px, new Rectangle((int)((p.X + PAD) * _s), (int)((p.Y + T_H) * _s), (int)((p.W - PAD * 2) * _s), System.Math.Max(1, (int)(1 * _s))), ColorBorde * 0.45f);

            string titulo = _pantalla == PantallaUI.Menu ? (_npc?.GivenOrTypeName ?? "") : ObtenerTituloMisionActual();
            float esT = 1.1f * _s, twT = font.MeasureString(titulo).X * esT;
            Utils.DrawBorderString(sb, titulo, new Vector2(_pPx.X + (_pPx.Width - twT) / 2f, _pPx.Y + 6 * _s), ColorTitulo, esT);

            if (_pantalla == PantallaUI.MisionDialogo)
                DibujarDialogoCompletacion(sb, font);
            else
            {
                string tm = _textoVis;
                if (!TextoListo && (int)(Main.GameUpdateCount / 20) % 2 == 0) tm += "|";
                DibujarTexto(sb, font, tm, new Vector2(_pPx.X + PAD * _s, _pPx.Y + (T_H + 8f) * _s), (int)((p.W - PAD * 2) * _s), 0.93f * _s);
                if (_pantalla == PantallaUI.MisionObjetivo) DibujarSpriteItem(sb);
            }

            if (TextoListo && _textoFull.Length > 1 && _pantalla != PantallaUI.MisionDialogo)
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

        private void DibujarDialogoCompletacion(SpriteBatch sb, DynamicSpriteFont font)
        {
            if (_lineasMostradas.Count == 0) return;

            var p = Panel;
            float escala = 0.93f * _s;
            float lineH = font.MeasureString("A").Y * escala;

            float xLeft = _pPx.X + PAD * _s;
            float xRight = _pPx.X + _pPx.Width - PAD * _s;
            float yTop = _pPx.Y + (T_H + 8f) * _s;
            int anchoMax = (int)((p.W - PAD * 2) * _s);

            var (nombre, _, esJugador) = _lineasMostradas[^1];
            Color colorNom = esJugador ? ColorDialogoJugador : ColorDialogoNPC;
            string label = nombre + ":";
            float labelW = font.MeasureString(label).X * escala;

            float xNombre = esJugador ? xRight - labelW : xLeft;
            Utils.DrawBorderString(sb, label,
                new Vector2(xNombre, yTop), colorNom, escala);

            string textoMostrar = _textoVis;
            if (!TextoListo && (int)(Main.GameUpdateCount / 20) % 2 == 0)
                textoMostrar += "|";

            if (esJugador)
                DibujarDerechaAbajo(sb, font, textoMostrar,
                    xRight, yTop + lineH, anchoMax, escala, Color.White);
            else
                DibujarTexto(sb, font, textoMostrar,
                    new Vector2(xLeft, yTop + lineH), anchoMax, escala);
        }


        private static int MedirLineas(DynamicSpriteFont font, string texto, float anchoMax, float escala)
        {
            if (string.IsNullOrEmpty(texto)) return 1;
            int n = 0; string l = "";
            foreach (string w in texto.Split(' '))
            {
                string pr = l.Length > 0 ? l + " " + w : w;
                if (font.MeasureString(pr).X * escala > anchoMax) { if (l.Length > 0) n++; l = w; }
                else l = pr;
            }
            if (l.Length > 0) n++;
            return System.Math.Max(1, n);
        }

        private static void DibujarDerechaAbajo(SpriteBatch sb, DynamicSpriteFont font,
            string texto, float xRight, float y0, float anchoMax, float escala, Color color)
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

        private string ObtenerTituloMisionActual()
        {
            var ms = ObtenerMisiones(); var p = ObtenerPlayer(); int i = p.MisionActual;
            if (i < 0 || i >= ms.Length) return _npc?.GivenOrTypeName ?? "";
            return Language.GetTextValue(ms[i].TituloKey);
        }

        private void DibujarSpriteItem(SpriteBatch sb)
        {
            var ms = ObtenerMisiones(); var p = ObtenerPlayer(); int i = p.MisionActual;
            if (i < 0 || i >= ms.Length) return;
            var m = ms[i]; if (m.ItemRequisito == 0) return;
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

        private static void DibujarTexto(SpriteBatch sb, DynamicSpriteFont font,
            string texto, Vector2 pos, int anchoMaxPx, float escala, Color? col = null)
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