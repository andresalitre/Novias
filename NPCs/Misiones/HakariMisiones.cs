using Terraria.ModLoader;
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
                Clave                = "Hakari_1",
                TituloKey            = "Mods.Novias.Misiones.Hakari.Mision1.Titulo",
                DescripcionKey       = "Mods.Novias.Misiones.Hakari.Mision1.Descripcion",
                DialogosKey          = new[]
                {
                    "Mods.Novias.Misiones.Hakari.Mision1.Dialogo0",
                    "Mods.Novias.Misiones.Hakari.Mision1.Dialogo1",
                    "Mods.Novias.Misiones.Hakari.Mision1.Dialogo2",
                },
                ItemRequisito        = ModContent.ItemType<MedioRefrescoDeMelocoton>(),
                CantidadRequisito    = 1,
                ItemRecompensa       = ModContent.ItemType<PocionDeSeduccion>(),
                CantidadRecompensa   = 1,
                DialogoRecompensaKey = "Mods.Novias.Misiones.Hakari.Mision1.Recompensa",
                DialogosCompletacion = new[]
                {
                    new LineaDialogo { EsJugador = true,  Key = "Mods.Novias.Misiones.Hakari.Mision1.Completacion0" },
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Hakari.Mision1.Completacion1" },
                    new LineaDialogo { EsJugador = true,  Key = "Mods.Novias.Misiones.Hakari.Mision1.Completacion2" },
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Hakari.Mision1.Completacion3" },
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Hakari.Mision1.Completacion4" },
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Hakari.Mision1.Completacion4" },
                    new LineaDialogo { EsJugador = true,  Key = "Mods.Novias.Misiones.Hakari.Mision1.Completacion5" },
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Hakari.Mision1.Completacion6" },
                    new LineaDialogo { EsJugador = true,  Key = "Mods.Novias.Misiones.Hakari.Mision1.Completacion7" },
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Hakari.Mision1.Completacion8" },
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Hakari.Mision1.Completacion9" },
                },
            },
            new MisionData
            {
                Clave                = "Hakari_2",
                TituloKey            = "Mods.Novias.Misiones.Hakari.Mision2.Titulo",
                DescripcionKey       = "Mods.Novias.Misiones.Hakari.Mision2.Descripcion",
                DialogosKey          = new[]
                {
                    "Mods.Novias.Misiones.Hakari.Mision2.Dialogo0",
                    "Mods.Novias.Misiones.Hakari.Mision2.Dialogo1",
                    "Mods.Novias.Misiones.Hakari.Mision2.Dialogo2",
                },
                ItemRequisito        = 0,
                ItemRecompensa       = ModContent.ItemType<PocionDeSeduccion>(),
                CantidadRecompensa   = 1,
                DialogoRecompensaKey = "Mods.Novias.Misiones.Hakari.Mision2.Recompensa",
            },
            new MisionData
            {
                Clave                = "Hakari_3",
                TituloKey            = "Mods.Novias.Misiones.Hakari.Mision3.Titulo",
                DescripcionKey       = "Mods.Novias.Misiones.Hakari.Mision3.Descripcion",
                DialogosKey          = new[]
                {
                    "Mods.Novias.Misiones.Hakari.Mision3.Dialogo0",
                    "Mods.Novias.Misiones.Hakari.Mision3.Dialogo1",
                    "Mods.Novias.Misiones.Hakari.Mision3.Dialogo2",
                },
                ItemRequisito        = 0,
                ItemRecompensa       = ModContent.ItemType<PocionDeSeduccion>(),
                CantidadRecompensa   = 1,
                DialogoRecompensaKey = "Mods.Novias.Misiones.Hakari.Mision3.Recompensa",
            },
        };
    }
}