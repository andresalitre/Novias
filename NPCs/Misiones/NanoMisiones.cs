using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Novias.Systems;
using Novias.Items.Potions;
using Novias.Items.GirlfriendsItems.Nano;
using Novias.Players;
using Microsoft.Xna.Framework;

namespace Novias.NPCs.Misiones
{
    public static class NanoMisiones
    {
        public static MisionData[] ObtenerMisiones() => new[]
        {

            //esta mision para seguimiento
            new MisionData
            {
                TituloKey            = "Mods.Novias.Misiones.Nano.Mision1.Titulo",
                DescripcionKey       = "Mods.Novias.Misiones.Nano.Mision1.Descripcion",
                ItemRequisito        = 0,
                CantidadRequisito    = 1,
                ItemRecompensa       = ModContent.ItemType<PocionDeEficiencia>(),
                CantidadRecompensa   = 1,
                DialogoRecompensaKey = "Mods.Novias.Misiones.Nano.Mision1.Recompensa",
                MensajeBloqueadoKey  = "Mods.Novias.Misiones.Nano.Mision1.Bloqueado",
                Condicion = () => Main.LocalPlayer.GetModPlayer<ShizukaPlayer>().MisionActual >= 3, //completar el beso con shizuka
                OnMensajesCompletacion = () =>
                {
                    string nj = Main.LocalPlayer.name;
                    Main.NewText(Language.GetTextValue("Mods.Novias.UI.TiendaDesbloqueada", nj, "Nano"), 255, 215, 0);
                    Main.NewText(Language.GetTextValue("Mods.Novias.UI.SeguimientoDesbloqueado", nj, "Nano"), 180, 80, 220);
                },
                DialogosPresentacion = new[]
                {
                    new LineaDialogo { EsJugador = true, Key = "Mods.Novias.Misiones.Nano.Mision1.Dialogo0" },
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
                    new LineaDialogo { EsJugador = false,  Key = "Mods.Novias.Misiones.Nano.Mision1.Completacion15" },
                    new LineaDialogo { EsJugador = true,   Key = "Mods.Novias.Misiones.Nano.Mision1.Completacion16" },
                    new LineaDialogo { EsJugador = true,   Key = "Mods.Novias.Misiones.Nano.Mision1.Completacion17" },
                    new LineaDialogo { EsJugador = false,  Key = "Mods.Novias.Misiones.Nano.Mision1.Completacion18" },
                    new LineaDialogo { EsJugador = true,   Key = "Mods.Novias.Misiones.Nano.Mision1.Completacion19" },

                },
            },

            //Mision de beso

            new MisionData
            {
                TituloKey            = "Mods.Novias.Misiones.Nano.Mision2.Titulo",
                DescripcionKey       = "Mods.Novias.Misiones.Nano.Mision2.Descripcion",
                ItemRequisito        = 0,
                CantidadRequisito    = 1,
                ItemRecompensa       = ModContent.ItemType<PocionDeEficiencia>(),
                CantidadRecompensa   = 1,
                DialogoRecompensaKey = "Mods.Novias.Misiones.Nano.Mision2.Recompensa",
                MensajeBloqueadoKey  = "Mods.Novias.Misiones.Nano.Mision2.Bloqueado",
                OnMensajesCompletacion = () =>
                {
                    string nj = Main.LocalPlayer.name;
                    Main.NewText(Language.GetTextValue("Mods.Novias.UI.TiendaDesbloqueada", nj, "Nano"), 255, 215, 0);
                    Main.NewText(Language.GetTextValue("Mods.Novias.UI.SeguimientoDesbloqueado", nj, "Nano"), 180, 80, 220);
                },
                DialogosPresentacion = new[]
                {
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Nano.Mision2.Dialogo0" },
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Nano.Mision2.Dialogo1" },
                    new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.Nano.Mision2.Dialogo2" },
                },
                DialogosCompletacion = new[]
                {
                    new LineaDialogo { EsJugador = true,  Key = "Mods.Novias.Misiones.Nano.Mision2.Completacion0" },
                    new LineaDialogo { EsJugador = true,  Key = "Mods.Novias.Misiones.Nano.Mision2.Completacion1" },
                    new LineaDialogo { EsJugador = true,  Key = "Mods.Novias.Misiones.Nano.Mision2.Completacion2" },
                    new LineaDialogo { EsJugador = true,  Key = "Mods.Novias.Misiones.Nano.Mision2.Completacion3" },
                },
            },



        };
    }
}