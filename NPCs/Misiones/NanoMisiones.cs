using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Novias.Systems;
using Novias.Items.Potions;
using Novias.Items.GirlfriendsItems.Nano;
using Novias.Players;
using Microsoft.Xna.Framework;
using Novias.NPCs.Novias;

namespace Novias.NPCs.Misiones
{
    public static class NanoMisiones
    {
        static string PensamientoNano => Language.GetTextValue("Mods.Novias.Misiones.PensamientoNano");
        static string Pensamiento => Language.GetTextValue("Mods.Novias.Misiones.Pensamiento");

        static NanoPlayer Nano => Main.LocalPlayer.GetModPlayer<NanoPlayer>();

        static bool SoloNanoSigue()
        {
            var s = Main.LocalPlayer.GetModPlayer<ShizukaPlayer>();
            var k = Main.LocalPlayer.GetModPlayer<KaranePlayer>();
            var h = Main.LocalPlayer.GetModPlayer<HakariPlayer>();
            return Nano.EstaSiguiendo && !s.EstaSiguiendo && !k.EstaSiguiendo && !h.EstaSiguiendo;
        }

        public static MisionData[] ObtenerMisiones() => new[]
        {
            new MisionData //MISION 1 NANO
            {
                TituloKey            = "Mods.Novias.Misiones.Nano.Mision1.Titulo",
                DescripcionKey       = "Mods.Novias.Misiones.Nano.Mision1.Descripcion",
                DialogoRecompensaKey = "Mods.Novias.Misiones.Nano.Mision1.Recompensa",
                MensajeBloqueadoKey  = "Mods.Novias.Misiones.Nano.Mision1.Bloqueado",
                Condicion = () => Main.LocalPlayer.GetModPlayer<ShizukaPlayer>().MisionActual >= 3,
                DialogosPresentacion = new[]
                {
                    new LineaDialogo { EsJugador = true,  Key = "Mods.Novias.Misiones.Nano.Mision1.Dialogo0" },
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Nano.Mision1.Dialogo1" },
                    new LineaDialogo { EsJugador = true,  Key = "Mods.Novias.Misiones.Nano.Mision1.Dialogo2", NombreNPC = Pensamiento },
                },
                DialogosCompletacion = new[]
                {
                    new LineaDialogo { EsJugador = false,  Key = "Mods.Novias.Misiones.Nano.Mision1.Completacion0" },
                    new LineaDialogo { EsJugador = false,  Key = "Mods.Novias.Misiones.Nano.Mision1.Completacion1" },
                    new LineaDialogo { EsJugador = true,   Key = "Mods.Novias.Misiones.Nano.Mision1.Completacion2" },
                    new LineaDialogo { EsJugador = false,  Key = "Mods.Novias.Misiones.Nano.Mision1.Completacion3" },
                    new LineaDialogo { EsJugador = true,   Key = "Mods.Novias.Misiones.Nano.Mision1.Completacion4" },
                    new LineaDialogo { EsJugador = false,  Key = "Mods.Novias.Misiones.Nano.Mision1.Completacion5" },
                    new LineaDialogo { EsJugador = false,  Key = "Mods.Novias.Misiones.Nano.Mision1.Completacion6" },
                    new LineaDialogo { EsJugador = false,  Key = "Mods.Novias.Misiones.Nano.Mision1.Completacion7" },
                    new LineaDialogo { EsJugador = false,  Key = "Mods.Novias.Misiones.Nano.Mision1.Completacion8" },
                    new LineaDialogo { EsJugador = false,  Key = "Mods.Novias.Misiones.Nano.Mision1.Completacion9" },
                    new LineaDialogo { EsJugador = true,   Key = "Mods.Novias.Misiones.Nano.Mision1.Completacion10" },
                    new LineaDialogo { EsJugador = false,  Key = "Mods.Novias.Misiones.Nano.Mision1.Completacion11" },
                    new LineaDialogo { EsJugador = true,   Key = "Mods.Novias.Misiones.Nano.Mision1.Completacion12" },
                    new LineaDialogo { EsJugador = false,  Key = "Mods.Novias.Misiones.Nano.Mision1.Completacion13" },
                    new LineaDialogo { EsJugador = true,   Key = "Mods.Novias.Misiones.Nano.Mision1.Completacion14" },
                    new LineaDialogo { EsJugador = false,  Key = "Mods.Novias.Misiones.Nano.Mision1.Completacion15", NombreNPC = PensamientoNano },
                    new LineaDialogo { EsJugador = true,   Key = "Mods.Novias.Misiones.Nano.Mision1.Completacion16" },
                    new LineaDialogo { EsJugador = true,   Key = "Mods.Novias.Misiones.Nano.Mision1.Completacion17" },
                    new LineaDialogo { EsJugador = false,  Key = "Mods.Novias.Misiones.Nano.Mision1.Completacion18" },
                    new LineaDialogo { EsJugador = true,   Key = "Mods.Novias.Misiones.Nano.Mision1.Completacion19" },
                },
                OnMensajesCompletacion = () =>
                {
                    string nj = Main.LocalPlayer.name;
                    Main.NewText(Language.GetTextValue("Mods.Novias.UI.SeguimientoDesbloqueado", nj, "Nano"), 180, 80, 220);
                },
            },

            new MisionData // MISION 2 NANO, IR AL INFRAMUNDO Y A LA NIEVE (Sacar fotos con la camara)
            {
                TituloKey            = "Mods.Novias.Misiones.Nano.Mision2.Titulo",
                DescripcionKey       = "Mods.Novias.Misiones.Nano.Mision2.Descripcion",
                ItemRecompensa       = ModContent.ItemType<PocionDeEficiencia>(),
                CantidadRecompensa   = 1,
                DialogoRecompensaKey = "Mods.Novias.Misiones.Nano.Mision2.Recompensa",
                MensajeBloqueadoKey  = "Mods.Novias.Misiones.Nano.Mision2.Bloqueado",
                Condicion = () => SoloNanoSigue(),
                ItemsDisplay = new[] {
                    ModContent.ItemType<Camara>(),
                    ModContent.ItemType<FotoInframundo>(),
                    ModContent.ItemType<FotoNieve>()
                },
                AvanzaFase           = false,
                DialogosPresentacion = new[]
                {
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Nano.Mision2.Dialogo0" },
                    new LineaDialogo { EsJugador = true,  Key = "Mods.Novias.Misiones.Nano.Mision2.Dialogo1" },
                    new LineaDialogo { EsJugador = true,  Key = "Mods.Novias.Misiones.Nano.Mision2.Dialogo2", NombreNPC = Pensamiento},
                    new LineaDialogo { EsJugador = true,  Key = "Mods.Novias.Misiones.Nano.Mision2.Dialogo3", NombreNPC = Pensamiento},
                },
                DialogosCompletacion = new[]
                {
                    new LineaDialogo { EsJugador = true,  Key = "Mods.Novias.Misiones.Nano.Mision2.Completacion0" },
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Nano.Mision2.Completacion1" },
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Nano.Mision2.Completacion2" },
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Nano.Mision2.Completacion3" },
                    new LineaDialogo { EsJugador = true, Key = "Mods.Novias.Misiones.Nano.Mision2.Completacion4", ItemsMostrar = new[] { ModContent.ItemType<Camara>() } },
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Nano.Mision2.Completacion5" },
                    new LineaDialogo { EsJugador = true,  Key = "Mods.Novias.Misiones.Nano.Mision2.Completacion6" },
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Nano.Mision2.Completacion7" },
                    new LineaDialogo { EsJugador = true,  Key = "Mods.Novias.Misiones.Nano.Mision2.Completacion8", NombreNPC = Pensamiento},
                    new LineaDialogo { EsJugador = true,  Key = "Mods.Novias.Misiones.Nano.Mision2.Completacion9" },
                    new LineaDialogo { EsJugador = true,  Key = "Mods.Novias.Misiones.Nano.Mision2.Completacion10", ItemsMostrar = new[] { ModContent.ItemType<FotoInframundo>(), ModContent.ItemType<FotoNieve>() } },
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Nano.Mision2.Completacion11" },
                },
                CondicionCompletar = () =>
                {
                    if (!SoloNanoSigue()) return false;
                    bool tieneInframundo = Main.LocalPlayer.CountItem(ModContent.ItemType<FotoInframundo>()) >= 1;
                    bool tieneNieve = Main.LocalPlayer.CountItem(ModContent.ItemType<FotoNieve>()) >= 1;
                    return tieneInframundo && tieneNieve;
                },
            },

            new MisionData // MISION 3 NANO, foto del cielo (Estar en el cielo y sacar la foto con la camara)
            {
                TituloKey            = "Mods.Novias.Misiones.Nano.Mision3.Titulo",
                DescripcionKey       = "Mods.Novias.Misiones.Nano.Mision3.Descripcion",
                ItemRecompensa       = ModContent.ItemType<PocionDeEficiencia>(),
                CantidadRecompensa   = 1,
                DialogoRecompensaKey = "Mods.Novias.Misiones.Nano.Mision3.Recompensa",
                MensajeBloqueadoKey  = "Mods.Novias.Misiones.Nano.Mision3.Bloqueado",
                Condicion = () => SoloNanoSigue(),
                ItemsDisplay = new[] {
                    ModContent.ItemType<Camara>(),
                    ModContent.ItemType<FotoCielo>()
                },
                AvanzaFase           = false,
                DialogosPresentacion = new[]
                {
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Nano.Mision3.Dialogo0" },
                    new LineaDialogo { EsJugador = true,  Key = "Mods.Novias.Misiones.Nano.Mision3.Dialogo1" },
                    new LineaDialogo { EsJugador = true,  Key = "Mods.Novias.Misiones.Nano.Mision3.Dialogo2", NombreNPC = Pensamiento },
                },
                DialogosCompletacion = new[]
                {
                    new LineaDialogo { EsJugador = true,  Key = "Mods.Novias.Misiones.Nano.Mision3.Completacion0" },
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Nano.Mision3.Completacion1" },
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Nano.Mision3.Completacion2" },
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Nano.Mision3.Completacion3" },
                    new LineaDialogo { EsJugador = true,  Key = "Mods.Novias.Misiones.Nano.Mision3.Completacion4" },
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Nano.Mision3.Completacion5" },
                    new LineaDialogo { EsJugador = true,  Key = "Mods.Novias.Misiones.Nano.Mision3.Completacion6" },
                },
                CondicionCompletar = () =>
                {
                    if (!SoloNanoSigue()) return false;
                    if (Main.LocalPlayer.CountItem(ModContent.ItemType<FotoCielo>()) < 1) return false;
                    foreach (NPC npc in Main.npc)
                    {
                        if (!npc.active) continue;
                        if (npc.ModNPC is NanoEiai && npc.ai[2] == 1f)
                            return true;
                    }
                    return false;
                },
            },

            new MisionData // MISION 4, conversacion final, desbloquea beso
            {
                TituloKey          = "Mods.Novias.Misiones.Nano.Mision4.Titulo",
                DescripcionKey     = "Mods.Novias.Misiones.Nano.Mision4.Descripcion",
                ItemRecompensa     = ModContent.ItemType<PocionDeEficiencia>(),
                CantidadRecompensa = 1,
                MensajeBloqueadoKey = "Mods.Novias.Misiones.Nano.Mision4.Bloqueado",
                DialogosPresentacion = new[]
                {
                    new LineaDialogo { EsJugador = true,  Key = "Mods.Novias.Misiones.Nano.Mision4.Dialogo0" },
                    new LineaDialogo { EsJugador = true,  Key = "Mods.Novias.Misiones.Nano.Mision4.Dialogo1" },
                    new LineaDialogo { EsJugador = true,  Key = "Mods.Novias.Misiones.Nano.Mision4.Dialogo2" },
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Nano.Mision4.Dialogo3" },
                },
                DialogosCompletacion = new[]
                {
                    new LineaDialogo { EsJugador = true,  Key = "Mods.Novias.Misiones.Nano.Mision4.Completacion0" },
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Nano.Mision4.Completacion1" },
                    new LineaDialogo { EsJugador = true,  Key = "Mods.Novias.Misiones.Nano.Mision4.Completacion2" },
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Nano.Mision4.Completacion3" },
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Nano.Mision4.Completacion4" },
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Nano.Mision4.Completacion5" },
                    new LineaDialogo { EsJugador = true,  Key = "Mods.Novias.Misiones.Nano.Mision4.Completacion6" },
                    new LineaDialogo { EsJugador = true,  Key = "Mods.Novias.Misiones.Nano.Mision4.Completacion7" },
                    new LineaDialogo { EsJugador = true,  Key = "Mods.Novias.Misiones.Nano.Mision4.Completacion8" },
                    new LineaDialogo { EsJugador = true,  Key = "Mods.Novias.Misiones.Nano.Mision4.Completacion9" },
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Nano.Mision4.Completacion10" },
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Nano.Mision4.Completacion11" },
                    new LineaDialogo { EsJugador = true,  Key = "Mods.Novias.Misiones.Nano.Mision4.Completacion12" },
                    new LineaDialogo { EsJugador = true,  Key = "Mods.Novias.Misiones.Nano.Mision4.Completacion13" },
                    new LineaDialogo { EsJugador = true,  Key = "Mods.Novias.Misiones.Nano.Mision4.Completacion14" },
                    new LineaDialogo { EsJugador = true,  Key = "Mods.Novias.Misiones.Nano.Mision4.Completacion15" },
                    new LineaDialogo { EsJugador = true,  Key = "Mods.Novias.Misiones.Nano.Mision4.Completacion16" },
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Nano.Mision4.Completacion17" },
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Nano.Mision4.Completacion18" },
                    new LineaDialogo { EsJugador = true,  Key = "Mods.Novias.Misiones.Nano.Mision4.Completacion19" },
                    new LineaDialogo { EsJugador = true,  Key = "Mods.Novias.Misiones.Nano.Mision4.Completacion20" },
                    new LineaDialogo { EsJugador = true,  Key = "Mods.Novias.Misiones.Nano.Mision4.Completacion21" },
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Nano.Mision4.Completacion22" },
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Nano.Mision4.Completacion23" },
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Nano.Mision4.Completacion24" },
                    new LineaDialogo { EsJugador = true,  Key = "Mods.Novias.Misiones.Nano.Mision4.Completacion25" },
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Nano.Mision4.Completacion26" },
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Nano.Mision4.Completacion27" },
                    new LineaDialogo { EsJugador = true,  Key = "Mods.Novias.Misiones.Nano.Mision4.Completacion28" },
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Nano.Mision4.Completacion29", ItemsMostrar = new[] { ModContent.ItemType<FotoCielo>(), ModContent.ItemType<FotoInframundo>(), ModContent.ItemType<FotoNieve>() } },
                },
                CondicionCompletar = () =>
                {
                    bool tieneInframundo = Main.LocalPlayer.CountItem(ModContent.ItemType<FotoInframundo>()) >= 1;
                    bool tieneNieve = Main.LocalPlayer.CountItem(ModContent.ItemType<FotoNieve>()) >= 1;
                    bool tieneCielo = Main.LocalPlayer.CountItem(ModContent.ItemType<FotoCielo>()) >= 1;
                    return tieneInframundo && tieneNieve && tieneCielo;
                },
                OnMensajesCompletacion = () =>
                {
                    string nj = Main.LocalPlayer.name;
                    Main.NewText(Language.GetTextValue("Mods.Novias.UI.BesoDesbloqueado", nj, "Nano"), 210, 180, 255);
                },
                Condicion = () =>
                {
                    foreach (NPC npc in Main.npc)
                    {
                        if (!npc.active) continue;
                        if (npc.ModNPC is NanoEiai && npc.ai[2] == 1f)
                            return false;
                    }
                    return true;
                },
            },
        };
    }
}