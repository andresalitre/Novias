using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Novias.Systems;
using Novias.Items.Potions;
using Novias.Items.GirlfriendsItems.Shizuka;
using Novias.Players;
using Terraria.Audio;
using Microsoft.Xna.Framework;
using Novias.NPCs.Novias;
using Terraria.ID;

namespace Novias.NPCs.Misiones
{
    public static class ShizukaMisiones
    {
        private static bool mision3Iniciada = false;
        public static int ContadorEnemigosMision3Shizuka = 0;
        private const int metaEnemigos = 15;

        public static MisionData[] ObtenerMisiones() => new[]
        {
            new MisionData
            {
                TituloKey            = "Mods.Novias.Misiones.Shizuka.Mision1.Titulo",
                DescripcionKey       = "Mods.Novias.Misiones.Shizuka.Mision1.Descripcion",
                ItemRequisito        = ModContent.ItemType<TelefonoDeShizuka>(),
                CantidadRequisito    = 1,
                ItemRecompensa       = ModContent.ItemType<PocionDeEcosDeAmor>(),
                CantidadRecompensa   = 1,
                DialogoRecompensaKey = "Mods.Novias.Misiones.Shizuka.Mision1.Recompensa",
                MensajeBloqueadoKey  = "Mods.Novias.Misiones.Shizuka.Mision1.Bloqueado",
                Condicion = () =>
                {
                    var h = Main.LocalPlayer.GetModPlayer<HakariPlayer>();
                    var k = Main.LocalPlayer.GetModPlayer<KaranePlayer>();
                    return h.Fase >= 2 && k.Fase >= 2;
                },
                OnMensajesCompletacion = () =>
                {
                    string nj = Main.LocalPlayer.name;
                    Main.NewText(Language.GetTextValue("Mods.Novias.UI.TiendaDesbloqueada", nj, "Shizuka"), 255, 215, 0);
                    Main.NewText(Language.GetTextValue("Mods.Novias.UI.SeguimientoDesbloqueado", nj, "Shizuka"), 180, 80, 220);
                },
                DialogosPresentacion = new[]
                {
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Shizuka.Mision1.Dialogo0" },
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Shizuka.Mision1.Dialogo1" },
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Shizuka.Mision1.Dialogo2" },
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Shizuka.Mision1.Dialogo3" },
                    new LineaDialogo { EsJugador = true,  Key = "Mods.Novias.Misiones.Shizuka.Mision1.Dialogo4" },
                    new LineaDialogo { EsJugador = true,  Key = "Mods.Novias.Misiones.Shizuka.Mision1.Dialogo5" },
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Shizuka.Mision1.Dialogo6" },
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Shizuka.Mision1.Dialogo7" },
                    new LineaDialogo { EsJugador = true,  Key = "Mods.Novias.Misiones.Shizuka.Mision1.Dialogo8" },
                },
                DialogosCompletacion = new[]
                {
                    new LineaDialogo { EsJugador = true,  Key = "Mods.Novias.Misiones.Shizuka.Mision1.Completacion0" },
                    new LineaDialogo { EsJugador = true,  Key = "Mods.Novias.Misiones.Shizuka.Mision1.Completacion1" },
                    new LineaDialogo { EsJugador = true,  Key = "Mods.Novias.Misiones.Shizuka.Mision1.Completacion2" },
                    new LineaDialogo { EsJugador = true,  Key = "Mods.Novias.Misiones.Shizuka.Mision1.Completacion3" },
                    new LineaDialogo { EsJugador = true,  Key = "Mods.Novias.Misiones.Shizuka.Mision1.Completacion4" },
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Shizuka.Mision1.Completacion5" },
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Shizuka.Mision1.Completacion6" },
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Shizuka.Mision1.Completacion7" },
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Shizuka.Mision1.Completacion8" },
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Shizuka.Mision1.Completacion9" },
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Shizuka.Mision1.Completacion10" },
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Shizuka.Mision1.Completacion11" },
                    new LineaDialogo { EsJugador = true,  Key = "Mods.Novias.Misiones.Shizuka.Mision1.Completacion12" },
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Shizuka.Mision1.Completacion13" },
                    new LineaDialogo { EsJugador = true,  Key = "Mods.Novias.Misiones.Shizuka.Mision1.Completacion14" },
                    new LineaDialogo { EsJugador = true,  Key = "Mods.Novias.Misiones.Shizuka.Mision1.Completacion15" },
                    new LineaDialogo { EsJugador = true,  Key = "Mods.Novias.Misiones.Shizuka.Mision1.Completacion16" },
                },
            },

            new MisionData
            {
                TituloKey            = "Mods.Novias.Misiones.Shizuka.Mision2.Titulo",
                DescripcionKey       = "Mods.Novias.Misiones.Shizuka.Mision2.Descripcion",
                ItemRequisito        = 0,
                ItemRecompensa       = ModContent.ItemType<PocionDeEcosDeAmor>(),
                CantidadRecompensa   = 1,
                MensajeBloqueadoKey  = "Mods.Novias.Misiones.Shizuka.Mision2.Bloqueado",
                AvanzaFase           = false,
                Condicion = () =>
                {
                    var h = Main.LocalPlayer.GetModPlayer<HakariPlayer>();
                    var k = Main.LocalPlayer.GetModPlayer<KaranePlayer>();
                    return h.EstaSiguiendo && k.EstaSiguiendo
                        && Main.LocalPlayer.GetModPlayer<ShizukaPlayer>().Fase >= 1;
                },
                DialogosPresentacion = new[]
                {
                    new LineaDialogo { EsJugador = true, Key = "Mods.Novias.Misiones.Shizuka.Mision2.Dialogo0" },
                    new LineaDialogo { EsJugador = true, Key = "Mods.Novias.Misiones.Shizuka.Mision2.Dialogo1" },
                },
                DialogosCompletacion = new[]
                {
                    new LineaDialogo { EsJugador = true,  Key = "Mods.Novias.Misiones.Shizuka.Mision2.Completacion0" },
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Shizuka.Mision2.Completacion1",  NombreNPC = "Hakari", ColorNombre = new Color(255, 190, 230) },
                    new LineaDialogo { EsJugador = true,  Key = "Mods.Novias.Misiones.Shizuka.Mision2.Completacion2" },
                    new LineaDialogo { EsJugador = true,  Key = "Mods.Novias.Misiones.Shizuka.Mision2.Completacion3" },
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Shizuka.Mision2.Completacion4",  NombreNPC = "Karane", ColorNombre = new Color(239, 178, 97) },
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Shizuka.Mision2.Completacion5",  NombreNPC = "Hakari", ColorNombre = new Color(255, 190, 230) },
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Shizuka.Mision2.Completacion6",  NombreNPC = "Hakari", ColorNombre = new Color(255, 190, 230) },
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Shizuka.Mision2.Completacion7",  NombreNPC = "Karane", ColorNombre = new Color(239, 178, 97) },
                    new LineaDialogo { EsJugador = true,  Key = "Mods.Novias.Misiones.Shizuka.Mision2.Completacion8" },
                    new LineaDialogo { EsJugador = true,  Key = "Mods.Novias.Misiones.Shizuka.Mision2.Completacion9" },
                    new LineaDialogo { EsJugador = true,  Key = "Mods.Novias.Misiones.Shizuka.Mision2.Completacion10" },
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Shizuka.Mision2.Completacion11", NombreNPC = "Karane", ColorNombre = new Color(239, 178, 97) },
                    new LineaDialogo { EsJugador = true,  Key = "Mods.Novias.Misiones.Shizuka.Mision2.Completacion12" },
                    new LineaDialogo { EsJugador = true,  Key = "Mods.Novias.Misiones.Shizuka.Mision2.Completacion13" },
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Shizuka.Mision2.Completacion14", NombreNPC = "Karane", ColorNombre = new Color(239, 178, 97) },
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Shizuka.Mision2.Completacion15", NombreNPC = "Hakari", ColorNombre = new Color(255, 190, 230) },
                    new LineaDialogo { EsJugador = true,  Key = "Mods.Novias.Misiones.Shizuka.Mision2.Completacion16" },
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Shizuka.Mision2.Completacion17", NombreNPC = "Hakari", ColorNombre = new Color(255, 190, 230) },
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Shizuka.Mision2.Completacion18", NombreNPC = "Karane", ColorNombre = new Color(239, 178, 97) },
                    new LineaDialogo { EsJugador = true,  Key = "Mods.Novias.Misiones.Shizuka.Mision2.Completacion19" },
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Shizuka.Mision2.Completacion20", NombreNPC = "Hakari", ColorNombre = new Color(255, 190, 230) },
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Shizuka.Mision2.Completacion21", NombreNPC = "Karane", ColorNombre = new Color(239, 178, 97) },
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Shizuka.Mision2.Completacion22" },
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Shizuka.Mision2.Completacion23", NombreNPC = "Hakari y Karane", ColorNombre = new Color(255, 255, 255) },
                },
            },

            new MisionData
            {
                TituloKey            = "Mods.Novias.Misiones.Shizuka.Mision3.Titulo",
                DescripcionKey       = "Mods.Novias.Misiones.Shizuka.Mision3.Descripcion",
                ItemRequisito        = 0,
                ItemRecompensa       = ModContent.ItemType<PocionDeEcosDeAmor>(),
                CantidadRecompensa   = 1,
                MensajeBloqueadoKey  = "Mods.Novias.Misiones.Shizuka.Mision3.Bloqueado",
                OnMensajesCompletacion = () =>
                {
                    string nj = Main.LocalPlayer.name;
                    Main.NewText(Language.GetTextValue("Mods.Novias.UI.BesoDesbloqueado", nj, "Shizuka"), 180, 80, 220);
                },
                ObtenerContador = () => $"Enemigos eliminados: {ContadorEnemigosMision3Shizuka} / {metaEnemigos}",
                Condicion = () =>
                {
                    var s = Main.LocalPlayer.GetModPlayer<ShizukaPlayer>();
                    if (!s.EstaSiguiendo) return false;
                    if (mision3Iniciada) return true;
                    var k = Main.LocalPlayer.GetModPlayer<KaranePlayer>();
                    var h = Main.LocalPlayer.GetModPlayer<HakariPlayer>();
                    return k.EstaSiguiendo && h.EstaSiguiendo;
                },
                CondicionCompletar = () =>
                {
                    var s = Main.LocalPlayer.GetModPlayer<ShizukaPlayer>();
                    return s.EstaSiguiendo && ContadorEnemigosMision3Shizuka >= metaEnemigos;
                },
                OnAceptar = () =>
                {
                    mision3Iniciada = true;
                },
                OnCompletar = () =>
                {
                    mision3Iniciada = false;
                    ContadorEnemigosMision3Shizuka = 0;

                    SoundEngine.PlaySound(SoundID.Item17 with { Pitch = 0.3f, Volume = 0.8f });
                    var player = Main.LocalPlayer;

                    foreach (NPC npc in Main.npc)
                    {
                        if (!npc.active) continue;
                        if (npc.ModNPC is ShizukaYoshimoto)
                        {
                            npc.AddBuff(BuffID.Lovestruck, 60 * 5);
                            npc.Center = player.Center + new Vector2(player.direction * 20f, 0f);
                            npc.direction = npc.spriteDirection = -player.direction;
                        }
                    }

                    player.GetModPlayer<ShizukaPlayer>().CompletarMision();
                    player.GetModPlayer<ShizukaPlayer>().CompletacionPendiente = false;
                    player.GetModPlayer<ShizukaPlayer>().UIAbierta = false;

                    string nj = player.name;
                    Main.NewText(Language.GetTextValue("Mods.Novias.UI.BesoDesbloqueado", nj, "Shizuka"), 180, 80, 220);
                },
                DialogosPresentacion = new[]
                {
                    new LineaDialogo { EsJugador = true,  Key = "Mods.Novias.Misiones.Shizuka.Mision3.Dialogo0" },
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Shizuka.Mision3.Dialogo1", NombreNPC = "Karane", ColorNombre = new Color(239, 178, 97) },
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Shizuka.Mision3.Dialogo2", NombreNPC = "Karane", ColorNombre = new Color(239, 178, 97) },
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Shizuka.Mision3.Dialogo3" },
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Shizuka.Mision3.Dialogo4", NombreNPC = "Karane", ColorNombre = new Color(239, 178, 97) },
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Shizuka.Mision3.Dialogo5", NombreNPC = "Hakari", ColorNombre = new Color(255, 190, 230) },
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Shizuka.Mision3.Dialogo6", NombreNPC = "Karane", ColorNombre = new Color(239, 178, 97) },
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Shizuka.Mision3.Dialogo7" },
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Shizuka.Mision3.Dialogo8", NombreNPC = "Karane", ColorNombre = new Color(239, 178, 97) },
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Shizuka.Mision3.Dialogo9", NombreNPC = "Karane", ColorNombre = new Color(239, 178, 97) },
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Shizuka.Mision3.Dialogo10", NombreNPC = "Hakari", ColorNombre = new Color(255, 190, 230) },
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Shizuka.Mision3.Dialogo11", NombreNPC = "Karane", ColorNombre = new Color(239, 178, 97) },
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Shizuka.Mision3.Dialogo12" },
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Shizuka.Mision3.Dialogo13", NombreNPC = "Karane", ColorNombre = new Color(239, 178, 97) },
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Shizuka.Mision3.Dialogo14" },
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Shizuka.Mision3.Dialogo15", NombreNPC = "Karane", ColorNombre = new Color(239, 178, 97) },
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Shizuka.Mision3.Dialogo16", NombreNPC = "Hakari", ColorNombre = new Color(255, 190, 230) },
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Shizuka.Mision3.Dialogo17" },
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Shizuka.Mision3.Dialogo18" },
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Shizuka.Mision3.Dialogo19" },
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Shizuka.Mision3.Dialogo20" },
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Shizuka.Mision3.Dialogo21", NombreNPC = "Karane", ColorNombre = new Color(239, 178, 97) },
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Shizuka.Mision3.Dialogo22", NombreNPC = "Hakari", ColorNombre = new Color(255, 190, 230) },
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Shizuka.Mision3.Dialogo23", NombreNPC = "Hakari y Karane", ColorNombre = new Color(255, 255, 255) },
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Shizuka.Mision3.Dialogo24" },
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Shizuka.Mision3.Dialogo25" },
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Shizuka.Mision3.Dialogo26", NombreNPC = "Karane", ColorNombre = new Color(239, 178, 97) },
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Shizuka.Mision3.Dialogo27", NombreNPC = "Karane", ColorNombre = new Color(239, 178, 97) },
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Shizuka.Mision3.Dialogo28", NombreNPC = "Hakari", ColorNombre = new Color(255, 190, 230) },
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Shizuka.Mision3.Dialogo29", NombreNPC = "Karane", ColorNombre = new Color(239, 178, 97) },
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Shizuka.Mision3.Dialogo30", NombreNPC = "Hakari", ColorNombre = new Color(255, 190, 230) },
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Shizuka.Mision3.Dialogo31", NombreNPC = "Karane", ColorNombre = new Color(239, 178, 97) },
                    new LineaDialogo { EsJugador = true,  Key = "Mods.Novias.Misiones.Shizuka.Mision3.Dialogo32" },
                    new LineaDialogo { EsJugador = true,  Key = "Mods.Novias.Misiones.Shizuka.Mision3.Dialogo33" },
                    new LineaDialogo { EsJugador = true,  Key = "Mods.Novias.Misiones.Shizuka.Mision3.Dialogo34" },
                    new LineaDialogo { EsJugador = true,  Key = "Mods.Novias.Misiones.Shizuka.Mision3.Dialogo35" },
                    new LineaDialogo { EsJugador = true,  Key = "Mods.Novias.Misiones.Shizuka.Mision3.Dialogo36" },
                    new LineaDialogo { EsJugador = true,  Key = "Mods.Novias.Misiones.Shizuka.Mision3.Dialogo37" },
                    new LineaDialogo { EsJugador = true,  Key = "Mods.Novias.Misiones.Shizuka.Mision3.Dialogo38" },
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Shizuka.Mision3.Dialogo39", NombreNPC = "Karane", ColorNombre = new Color(239, 178, 97) },
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Shizuka.Mision3.Dialogo40", NombreNPC = "Hakari", ColorNombre = new Color(255, 190, 230) },
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Shizuka.Mision3.Dialogo41" },
                },
                DialogosCompletacion = new[]
                {
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Shizuka.Mision3.Completacion0" },
                    new LineaDialogo { EsJugador = true,  Key = "Mods.Novias.Misiones.Shizuka.Mision3.Completacion1" },
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Shizuka.Mision3.Completacion2" },
                    new LineaDialogo { EsJugador = true,  Key = "Mods.Novias.Misiones.Shizuka.Mision3.Completacion3" },
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Shizuka.Mision3.Completacion4" },
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Shizuka.Mision3.Completacion5" },
                    new LineaDialogo { EsJugador = true,  Key = "Mods.Novias.Misiones.Shizuka.Mision3.Completacion6" },
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Shizuka.Mision3.Completacion7" },
                    new LineaDialogo { EsJugador = true,  Key = "Mods.Novias.Misiones.Shizuka.Mision3.Completacion8" },
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Shizuka.Mision3.Completacion9" },
                    new LineaDialogo { EsJugador = true,  Key = "Mods.Novias.Misiones.Shizuka.Mision3.Completacion10" },
                },
            },
        };
    }
}