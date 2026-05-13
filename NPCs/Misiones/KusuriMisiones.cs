using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Novias.Systems;
using Novias.Items.Potions;
using Microsoft.Xna.Framework;
using Novias.Players;

namespace Novias.NPCs.Misiones
{
    public static class KusuriMisiones
    {
        static string Pensamiento => Language.GetTextValue("Mods.Novias.Misiones.Pensamiento");

        public static MisionData[] ObtenerMisiones() => new[]
        {
            new MisionData
            {
                TituloKey            = "Mods.Novias.Misiones.Kusuri.Mision1.Titulo",
                DescripcionKey       = "Mods.Novias.Misiones.Kusuri.Mision1.Descripcion",
                ItemRecompensa       = 0,
                CantidadRecompensa   = 0,
                MensajeBloqueadoKey  = "Mods.Novias.Misiones.Kusuri.Mision1.Bloqueado",
                DialogosPresentacion = new[]
                {
                    new LineaDialogo { EsJugador = true, Key = "Mods.Novias.Misiones.Kusuri.Mision1.Dialogo0" },
                    new LineaDialogo { EsJugador = true, Key = "Mods.Novias.Misiones.Kusuri.Mision1.Dialogo1" },
                    new LineaDialogo { EsJugador = true, Key = "Mods.Novias.Misiones.Kusuri.Mision1.Dialogo2" },
                    new LineaDialogo { EsJugador = true, Key = "Mods.Novias.Misiones.Kusuri.Mision1.Dialogo3" },
                    new LineaDialogo { EsJugador = true, Key = "Mods.Novias.Misiones.Kusuri.Mision1.Dialogo4" },
                    new LineaDialogo { EsJugador = true, Key = "Mods.Novias.Misiones.Kusuri.Mision1.Dialogo5" },
                    new LineaDialogo { EsJugador = true, Key = "Mods.Novias.Misiones.Kusuri.Mision1.Dialogo6" },
                    new LineaDialogo { EsJugador = true, Key = "Mods.Novias.Misiones.Kusuri.Mision1.Dialogo7" },
                    new LineaDialogo { EsJugador = true, Key = "Mods.Novias.Misiones.Kusuri.Mision1.Dialogo8" },
                    new LineaDialogo { EsJugador = true, Key = "Mods.Novias.Misiones.Kusuri.Mision1.Dialogo9" },
                },
                DialogosCompletacion = new[]
                {
                    new LineaDialogo { EsJugador = true,  Key = "Mods.Novias.Misiones.Kusuri.Mision1.Completacion0" },
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Kusuri.Mision1.Completacion1" },
                    new LineaDialogo { EsJugador = true,  Key = "Mods.Novias.Misiones.Kusuri.Mision1.Completacion2" },
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Kusuri.Mision1.Completacion3" },
                    new LineaDialogo { EsJugador = true,  Key = "Mods.Novias.Misiones.Kusuri.Mision1.Completacion4" },
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Kusuri.Mision1.Completacion5", OnMostrar = () => Main.LocalPlayer.GetModPlayer<KusuriPlayer>().EsMayor = true },
                    new LineaDialogo { EsJugador = true,  Key = "Mods.Novias.Misiones.Kusuri.Mision1.Completacion6" },
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Kusuri.Mision1.Completacion7" },
                    new LineaDialogo { EsJugador = true,  Key = "Mods.Novias.Misiones.Kusuri.Mision1.Completacion8" },
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Kusuri.Mision1.Completacion9" },

                },
            },

        };
    }
}