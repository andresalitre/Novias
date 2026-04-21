using Terraria.ModLoader;
using Novias.Systems;
using Novias.Items.Potions;
using Novias.Items.GirlfriendsItems.Karane;

namespace Novias.NPCs.Misiones
{
    public static class KaraneMisiones
    {
        public static MisionData[] ObtenerMisiones() => new[]
        {
            new MisionData
            {
                Clave                = "Karane_1",
                TituloKey            = "Mods.Novias.Misiones.Karane.Mision1.Titulo",
                DescripcionKey       = "Mods.Novias.Misiones.Karane.Mision1.Descripcion",
                DialogosKey          = new[]
                {
                    "Mods.Novias.Misiones.Karane.Mision1.Dialogo0",
                    "Mods.Novias.Misiones.Karane.Mision1.Dialogo1",
                    "Mods.Novias.Misiones.Karane.Mision1.Dialogo2",
                },
                ItemRequisito        = ModContent.ItemType<GatitoDePeluche>(),
                CantidadRequisito    = 1,
                ItemRecompensa       = ModContent.ItemType<PocionDeTsundere>(),
                CantidadRecompensa   = 1,
                DialogoRecompensaKey = "Mods.Novias.Misiones.Karane.Mision1.Recompensa",
                DialogosCompletacion = new[]
                {
                    new LineaDialogo { EsJugador = true,  Key = "Mods.Novias.Misiones.Karane.Mision1.Completacion0" },
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Karane.Mision1.Completacion1" },
                },
            },
            new MisionData
            {
                Clave                = "Karane_2",
                TituloKey            = "Mods.Novias.Misiones.Karane.Mision2.Titulo",
                DescripcionKey       = "Mods.Novias.Misiones.Karane.Mision2.Descripcion",
                DialogosKey          = new[]
                {
                    "Mods.Novias.Misiones.Karane.Mision2.Dialogo0",
                    "Mods.Novias.Misiones.Karane.Mision2.Dialogo1",
                    "Mods.Novias.Misiones.Karane.Mision2.Dialogo2",
                },
                ItemRequisito        = 0,
                ItemRecompensa       = ModContent.ItemType<PocionDeTsundere>(),
                CantidadRecompensa   = 1,
                DialogoRecompensaKey = "Mods.Novias.Misiones.Karane.Mision2.Recompensa",
            },
            new MisionData
            {
                Clave                = "Karane_3",
                TituloKey            = "Mods.Novias.Misiones.Karane.Mision3.Titulo",
                DescripcionKey       = "Mods.Novias.Misiones.Karane.Mision3.Descripcion",
                DialogosKey          = new[]
                {
                    "Mods.Novias.Misiones.Karane.Mision3.Dialogo0",
                    "Mods.Novias.Misiones.Karane.Mision3.Dialogo1",
                    "Mods.Novias.Misiones.Karane.Mision3.Dialogo2",
                },
                ItemRequisito        = 0,
                ItemRecompensa       = ModContent.ItemType<PocionDeTsundere>(),
                CantidadRecompensa   = 1,
                DialogoRecompensaKey = "Mods.Novias.Misiones.Karane.Mision3.Recompensa",
            },
        };
    }
}