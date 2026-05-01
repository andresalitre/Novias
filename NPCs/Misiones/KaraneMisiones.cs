using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Novias.Systems;
using Novias.Items.Potions;
using Novias.Items.GirlfriendsItems.Karane;
using Novias.Players;

namespace Novias.NPCs.Misiones
{
    public static class KaraneMisiones
    {
        public static MisionData[] ObtenerMisiones() => new[]
        {
            new MisionData
            {
                TituloKey          = "Mods.Novias.Misiones.Karane.Mision1.Titulo",
                DescripcionKey     = "Mods.Novias.Misiones.Karane.Mision1.Descripcion",
                ItemRequisito      = ModContent.ItemType<GatitoDePeluche>(),
                CantidadRequisito  = 1,
                ItemRecompensa     = ModContent.ItemType<PocionDeTsundere>(),
                CantidadRecompensa = 1,
                DialogoRecompensaKey = "Mods.Novias.Misiones.Karane.Mision1.Recompensa",
                MensajeBloqueadoKey  = "Mods.Novias.Misiones.Karane.Mision1.Bloqueado",
                Condicion = () => Main.LocalPlayer.GetModPlayer<HakariPlayer>().MisionActual >= 1, //completar hakari 1
                OnMensajesCompletacion = () =>
                {
                    string nj = Main.LocalPlayer.name;
                    Main.NewText(Language.GetTextValue("Mods.Novias.UI.TiendaDesbloqueada", nj, "Karane"), 255, 215, 0);
                    Main.NewText(Language.GetTextValue("Mods.Novias.UI.SeguimientoDesbloqueado", nj, "Karane"), 180, 80, 220);
                },
                DialogosPresentacion = new[]
                {
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Karane.Mision1.Dialogo0" },
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Karane.Mision1.Dialogo1" },
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Karane.Mision1.Dialogo2" },
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Karane.Mision1.Dialogo3" },
                },
                DialogosCompletacion = new[]
                {
                    new LineaDialogo { EsJugador = true,  Key = "Mods.Novias.Misiones.Karane.Mision1.Completacion0" },
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Karane.Mision1.Completacion1" },
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Karane.Mision1.Completacion2" },
                    new LineaDialogo { EsJugador = true, Key = "Mods.Novias.Misiones.Karane.Mision1.Completacion3" },
                    new LineaDialogo { EsJugador = false,  Key = "Mods.Novias.Misiones.Karane.Mision1.Completacion4" },
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Karane.Mision1.Completacion5" },
                },
            },

            //Misiones compartidas, desbloquean beso

            Mision1HakariKarane.Obtener(),

            Mision2HakariKarane.Obtener(),

            new MisionData
            {
                TituloKey          = "Mods.Novias.Misiones.Karane.Mision2.Titulo",
                DescripcionKey     = "Mods.Novias.Misiones.Karane.Mision2.Descripcion",
                ItemRequisito      = 0,
                ItemRecompensa     = ModContent.ItemType<PocionDeTsundere>(),
                CantidadRecompensa = 1,
                MensajeBloqueadoKey  = "Mods.Novias.Misiones.Karane.Mision2.Bloqueado",
                Condicion          = () => false, //mision para despues
                DialogosPresentacion = new[]
                {
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Karane.Mision2.Dialogo0" },
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Karane.Mision2.Dialogo1" },
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Karane.Mision2.Dialogo2" },
                },
            },
        };
    }
}