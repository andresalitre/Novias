using Terraria.ModLoader;
using Terraria.Localization;
using Novias.Systems;
using Novias.Items.Potions;
using Novias.Items.GirlfriendsItems.Hakari;

namespace Novias.NPCs.Misiones
{
    public static class HakariMisiones
    {
        public static MisionData[] ObtenerMisiones() => new[]
        {
            new MisionData
            {
                TituloKey          = "Mods.Novias.Misiones.Hakari.Mision1.Titulo",
                DescripcionKey     = "Mods.Novias.Misiones.Hakari.Mision1.Descripcion",
                ItemRequisito      = ModContent.ItemType<MedioRefrescoDeMelocoton>(),
                CantidadRequisito  = 1,
                ItemRecompensa     = ModContent.ItemType<PocionDeSeduccion>(),
                CantidadRecompensa = 1,
                DialogoRecompensaKey = "Mods.Novias.Misiones.Hakari.Mision1.Recompensa",
                OnMensajesCompletacion = () =>
                {
                    string nj = Terraria.Main.LocalPlayer.name;
                    Terraria.Main.NewText(Terraria.Localization.Language.GetTextValue("Mods.Novias.UI.TiendaDesbloqueada", nj, "Hakari"), 255, 215, 0);
                    Terraria.Main.NewText(Terraria.Localization.Language.GetTextValue("Mods.Novias.UI.SeguimientoDesbloqueado", nj, "Hakari"), 180, 80, 220);
                },
                DialogosPresentacion = new[]
                {
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Hakari.Mision1.Dialogo0" },
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Hakari.Mision1.Dialogo1" },
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Hakari.Mision1.Dialogo2" },
                },
                DialogosCompletacion = new[]
                {
                    new LineaDialogo { EsJugador = true,  Key = "Mods.Novias.Misiones.Hakari.Mision1.Completacion0" },
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Hakari.Mision1.Completacion1" },
                    new LineaDialogo { EsJugador = true,  Key = "Mods.Novias.Misiones.Hakari.Mision1.Completacion2" },
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Hakari.Mision1.Completacion3" },
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Hakari.Mision1.Completacion4" },
                    new LineaDialogo { EsJugador = true,  Key = "Mods.Novias.Misiones.Hakari.Mision1.Completacion5" },
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Hakari.Mision1.Completacion6" },
                    new LineaDialogo { EsJugador = true,  Key = "Mods.Novias.Misiones.Hakari.Mision1.Completacion7" },
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Hakari.Mision1.Completacion8" },
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Hakari.Mision1.Completacion9" },
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Hakari.Mision1.Completacion10" },
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Hakari.Mision1.Completacion11" },
                },
            },

            //Misiones compartidas, desbloquean beso

            Mision1HakariKarane.Obtener(),

            Mision2HakariKarane.Obtener(),

            new MisionData
            {
                TituloKey          = "Mods.Novias.Misiones.Hakari.Mision2.Titulo",
                DescripcionKey     = "Mods.Novias.Misiones.Hakari.Mision2.Descripcion",
                ItemRequisito      = 0,
                ItemRecompensa     = ModContent.ItemType<PocionDeSeduccion>(),
                CantidadRecompensa = 1,
                MensajeBloqueadoKey  = "Mods.Novias.Misiones.Hakari.Mision2.Bloqueado",
                Condicion          = () => false, // esta mision la hare despues
                DialogosPresentacion = new[]
                {
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Hakari.Mision2.Dialogo0" },
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Hakari.Mision2.Dialogo1" },
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Hakari.Mision2.Dialogo2" },
                },
            },
        };
    }
}