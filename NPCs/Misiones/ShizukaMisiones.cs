using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Novias.Systems;
using Novias.Items.Potions;
using Novias.Items.GirlfriendsItems.Shizuka;
using Novias.Players;
using Microsoft.Xna.Framework;

namespace Novias.NPCs.Misiones
{
    public static class ShizukaMisiones
    {
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
                Condicion = () => // completar mision compartida con las 2 primeras, osea desbloquear beso
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
                Condicion = () => // esta mision no desbloquea nada, es solo para avanzar en la historia
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


            //Esta mision desbloquea beso (mision 3) 

            new MisionData
            {
                TituloKey            = "Mods.Novias.Misiones.Shizuka.Mision3.Titulo",
                DescripcionKey       = "Mods.Novias.Misiones.Shizuka.Mision3.Descripcion",
                ItemRequisito        = 0,
                ItemRecompensa       = ModContent.ItemType<PocionDeEcosDeAmor>(),
                CantidadRecompensa   = 1,
                MensajeBloqueadoKey  = "Mods.Novias.Misiones.Shizuka.Mision3.Bloqueado",
                 Condicion = () => //karane sigue al jugador, ya que habra una pequeña conversacion entre shizuka y ella
                {
                    var k = Main.LocalPlayer.GetModPlayer<KaranePlayer>();
                    return k.EstaSiguiendo;
                },
                DialogosPresentacion = new[]
                {
                    new LineaDialogo { EsJugador = true, Key = "Mods.Novias.Misiones.Shizuka.Mision3.Dialogo0" },
                    new LineaDialogo { EsJugador = true, Key = "Mods.Novias.Misiones.Shizuka.Mision3.Dialogo1" },
                },
                DialogosCompletacion = new[]
                {
                    new LineaDialogo { EsJugador = true, Key = "Mods.Novias.Misiones.Shizuka.Mision3.Completacion0" },
                },
            },
        };
    }
}