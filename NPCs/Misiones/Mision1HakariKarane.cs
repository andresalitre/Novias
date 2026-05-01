using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Novias.Systems;
using Novias.Players;
using Novias.Items.Potions;
using Novias.Items.GirlfriendsItems.HakariKarane;

namespace Novias.NPCs.Misiones
{
    public static class Mision1HakariKarane
    {
        static readonly Color CHakari = new Color(255, 190, 230);
        static readonly Color CKarane = new Color(239, 178, 97);
        static readonly Color CAmbas = new Color(255, 255, 255);

        public static MisionData Obtener() => new MisionData
        {
            Clave = "CompartidaHakariKarane_1",
            TituloKey = "Mods.Novias.Misiones.CompartidaHakariKarane.Mision1.Titulo",
            DescripcionKey = "Mods.Novias.Misiones.CompartidaHakariKarane.Mision1.Descripcion",
            MensajeBloqueadoKey = "Mods.Novias.Misiones.CompartidaHakariKarane.Mision1.Bloqueado",
            ItemRequisito = ModContent.ItemType<TrebolDe4Hojas>(),
            CantidadRequisito = 2,
            ItemRecompensa = 0,

            Condicion = () =>
            {
                var h = Main.LocalPlayer.GetModPlayer<HakariPlayer>();
                var k = Main.LocalPlayer.GetModPlayer<KaranePlayer>();
                return h.MisionActual >= 1 && k.MisionActual >= 1
                    && h.EstaSiguiendo && k.EstaSiguiendo;
            },

            CondicionCompletar = () =>
            {
                var h = Main.LocalPlayer.GetModPlayer<HakariPlayer>();
                var k = Main.LocalPlayer.GetModPlayer<KaranePlayer>();
                return h.EstaSiguiendo && k.EstaSiguiendo
                    && Main.LocalPlayer.CountItem(ModContent.ItemType<TrebolDe4Hojas>()) >= 2;
            },

            OnAceptar = () =>
            {
                Main.LocalPlayer.GetModPlayer<HakariPlayer>().UIAbierta = true;
                Main.LocalPlayer.GetModPlayer<KaranePlayer>().UIAbierta = true;
            },

            OnIniciarCompletacion = () =>
            {
                Main.LocalPlayer.GetModPlayer<HakariPlayer>().CompletacionPendiente = true;
                Main.LocalPlayer.GetModPlayer<KaranePlayer>().CompletacionPendiente = true;
            },
            OnCompletar = () =>
            {
                var player = Main.LocalPlayer;

                int restante = 2;
                for (int i = 0; i < player.inventory.Length && restante > 0; i++)
                {
                    var it = player.inventory[i];
                    if (it.type != ModContent.ItemType<TrebolDe4Hojas>()) continue;
                    int quitar = System.Math.Min(it.stack, restante);
                    it.stack -= quitar; restante -= quitar;
                    if (it.stack <= 0) it.TurnToAir();
                }

                SoundEngine.PlaySound(SoundID.Item17 with { Pitch = 0.3f, Volume = 0.8f });

                player.GetModPlayer<HakariPlayer>().Mision1CompartidaCompletada = true;
                player.GetModPlayer<KaranePlayer>().Mision1CompartidaCompletada = true;

                player.GetModPlayer<HakariPlayer>().AvanzarMisionSinFase();
                player.GetModPlayer<KaranePlayer>().AvanzarMisionSinFase();

                player.QuickSpawnItem(player.GetSource_FromThis(), ModContent.ItemType<PocionDeSeduccion>(), 2);
                player.QuickSpawnItem(player.GetSource_FromThis(), ModContent.ItemType<PocionDeTsundere>(), 2);

                player.GetModPlayer<HakariPlayer>().CompletacionPendiente = false;
                player.GetModPlayer<KaranePlayer>().CompletacionPendiente = false;
                player.GetModPlayer<HakariPlayer>().UIAbierta = false;
                player.GetModPlayer<KaranePlayer>().UIAbierta = false;
            },

            DialogosPresentacion = new[]
            {
                new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.CompartidaHakariKarane.Mision1.Dialogo0", NombreNPC = "Hakari y Karane", ColorNombre = CAmbas },
                new LineaDialogo { EsJugador = true,  Key = "Mods.Novias.Misiones.CompartidaHakariKarane.Mision1.Dialogo1" },
                new LineaDialogo { EsJugador = true,  Key = "Mods.Novias.Misiones.CompartidaHakariKarane.Mision1.Dialogo2" },
                new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.CompartidaHakariKarane.Mision1.Dialogo3", NombreNPC = "Karane", ColorNombre = CKarane },
            },

            DialogosCompletacion = new[]
            {
                new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.CompartidaHakariKarane.Mision1.Completacion0",  NombreNPC = "Hakari", ColorNombre = CHakari },
                new LineaDialogo { EsJugador = true,  Key = "Mods.Novias.Misiones.CompartidaHakariKarane.Mision1.Completacion1"  },
                new LineaDialogo { EsJugador = true,  Key = "Mods.Novias.Misiones.CompartidaHakariKarane.Mision1.Completacion2"  },
                new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.CompartidaHakariKarane.Mision1.Completacion3",  NombreNPC = "Karane", ColorNombre = CKarane },
                new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.CompartidaHakariKarane.Mision1.Completacion4",  NombreNPC = "Karane", ColorNombre = CKarane },
                new LineaDialogo { EsJugador = true,  Key = "Mods.Novias.Misiones.CompartidaHakariKarane.Mision1.Completacion5"  },
                new LineaDialogo { EsJugador = true,  Key = "Mods.Novias.Misiones.CompartidaHakariKarane.Mision1.Completacion6"  },
                new LineaDialogo { EsJugador = true,  Key = "Mods.Novias.Misiones.CompartidaHakariKarane.Mision1.Completacion7"  },
                new LineaDialogo { EsJugador = true,  Key = "Mods.Novias.Misiones.CompartidaHakariKarane.Mision1.Completacion8"  },
                new LineaDialogo { EsJugador = true,  Key = "Mods.Novias.Misiones.CompartidaHakariKarane.Mision1.Completacion9"  },
                new LineaDialogo { EsJugador = true,  Key = "Mods.Novias.Misiones.CompartidaHakariKarane.Mision1.Completacion10" },
                new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.CompartidaHakariKarane.Mision1.Completacion11", NombreNPC = "Hakari", ColorNombre = CHakari },
                new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.CompartidaHakariKarane.Mision1.Completacion12", NombreNPC = "Karane", ColorNombre = CKarane },
                new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.CompartidaHakariKarane.Mision1.Completacion13", NombreNPC = "Karane", ColorNombre = CKarane },
                new LineaDialogo { EsJugador = true,  Key = $"[i:{ModContent.ItemType<TrebolDe4Hojas>()}]" + $"[i:{ModContent.ItemType<TrebolDe4Hojas>()}]" },
                new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.CompartidaHakariKarane.Mision1.Completacion15", NombreNPC = "Karane", ColorNombre = CKarane },
                new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.CompartidaHakariKarane.Mision1.Completacion16", NombreNPC = "Hakari", ColorNombre = CHakari },
                new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.CompartidaHakariKarane.Mision1.Completacion17", NombreNPC = "Hakari", ColorNombre = CHakari },
                new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.CompartidaHakariKarane.Mision1.Completacion18", NombreNPC = "Karane", ColorNombre = CKarane },
                new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.CompartidaHakariKarane.Mision1.Completacion19", NombreNPC = "Hakari", ColorNombre = CHakari },
                new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.CompartidaHakariKarane.Mision1.Completacion20", NombreNPC = "Karane", ColorNombre = CKarane },
                new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.CompartidaHakariKarane.Mision1.Completacion21", NombreNPC = "Karane", ColorNombre = CKarane },
                new LineaDialogo { EsJugador = true,  Key = "Mods.Novias.Misiones.CompartidaHakariKarane.Mision1.Completacion22" },
                new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.CompartidaHakariKarane.Mision1.Completacion23", NombreNPC = "Karane", ColorNombre = CKarane },
                new LineaDialogo { EsJugador = true,  Key = "Mods.Novias.Misiones.CompartidaHakariKarane.Mision1.Completacion24" },
                new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.CompartidaHakariKarane.Mision1.Completacion25", NombreNPC = "Hakari y Karane", ColorNombre = CAmbas },
                new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.CompartidaHakariKarane.Mision1.Completacion26", NombreNPC = "Hakari", ColorNombre = CHakari },
                new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.CompartidaHakariKarane.Mision1.Completacion27", NombreNPC = "Karane", ColorNombre = CKarane },
                new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.CompartidaHakariKarane.Mision1.Completacion28", NombreNPC = "Hakari", ColorNombre = CHakari },
                new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.CompartidaHakariKarane.Mision1.Completacion29", NombreNPC = "Hakari", ColorNombre = CHakari },
                new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.CompartidaHakariKarane.Mision1.Completacion30", NombreNPC = "Karane", ColorNombre = CKarane },
                new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.CompartidaHakariKarane.Mision1.Completacion31", NombreNPC = "Karane", ColorNombre = CKarane },
                new LineaDialogo { EsJugador = true,  Key = "Mods.Novias.Misiones.CompartidaHakariKarane.Mision1.Completacion32" },
            },
        };
    }
}