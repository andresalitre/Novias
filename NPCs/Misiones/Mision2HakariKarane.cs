using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Novias.Systems;
using Novias.Players;
using Novias.NPCs.Novias;
using Novias.Items.Potions;
using Novias.Buffs;

namespace Novias.NPCs.Misiones
{
    public static class Mision2HakariKarane
    {
        static readonly Color CHakari = new Color(255, 190, 230);
        static readonly Color CKarane = new Color(239, 178, 97);
        static readonly Color CAmbas = new Color(255, 255, 255);

        const int metaDeEnemigos = 50;

        public static MisionData Obtener() => new MisionData
        {
            Clave = "CompartidaHakariKarane_2",
            TituloKey = "Mods.Novias.Misiones.CompartidaHakariKarane.Mision2.Titulo",
            DescripcionKey = "Mods.Novias.Misiones.CompartidaHakariKarane.Mision2.Descripcion",
            MensajeBloqueadoKey = "Mods.Novias.Misiones.CompartidaHakariKarane.Mision2.Bloqueado",
            ItemRequisito = 0,
            ItemRecompensa = 0,

            ObtenerContador = () =>
            {
                var h = Main.LocalPlayer.GetModPlayer<HakariPlayer>();
                return $"Enemigos eliminados: {h.ContadorEnemigosMision2} / {metaDeEnemigos}";
            },

            Condicion = () =>
            {
                var h = Main.LocalPlayer.GetModPlayer<HakariPlayer>();
                var k = Main.LocalPlayer.GetModPlayer<KaranePlayer>();
                return h.Mision1CompartidaCompletada && k.Mision1CompartidaCompletada
                    && h.EstaSiguiendo && k.EstaSiguiendo;
            },

            CondicionCompletar = () =>
            {
                var h = Main.LocalPlayer.GetModPlayer<HakariPlayer>();
                var k = Main.LocalPlayer.GetModPlayer<KaranePlayer>();
                return h.EstaSiguiendo && k.EstaSiguiendo
                    && h.ContadorEnemigosMision2 >= metaDeEnemigos;
            },

            YaFueCompletada = () =>
            {
                var h = Main.LocalPlayer.GetModPlayer<HakariPlayer>();
                var k = Main.LocalPlayer.GetModPlayer<KaranePlayer>();
                return h.Fase >= 2 && k.Fase >= 2;
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

                foreach (NPC npc in Main.npc)
                {
                    if (!npc.active) continue;
                    if (npc.ModNPC is HakariHanazono)
                    {
                        npc.AddBuff(BuffID.Lovestruck, 60 * 5);
                        npc.Center = player.Center + new Vector2(player.direction * 20f, 0f);
                        npc.direction = npc.spriteDirection = -player.direction;
                    }
                    if (npc.ModNPC is KaraneInda)
                    {
                        npc.AddBuff(BuffID.Lovestruck, 60 * 5);
                        npc.Center = player.Center + new Vector2(-player.direction * 20f, 0f);
                        npc.direction = npc.spriteDirection = player.direction;
                    }
                }

                SoundEngine.PlaySound(SoundID.Item17 with { Pitch = 0.3f, Volume = 0.8f });

                player.GetModPlayer<HakariPlayer>().CompletarMision();
                player.GetModPlayer<KaranePlayer>().CompletarMision();

                player.GetModPlayer<HakariPlayer>().ContadorEnemigosMision2 = 0;
                player.GetModPlayer<KaranePlayer>().ContadorEnemigosMision2 = 0;

                string nj = player.name;
                Main.NewText(Language.GetTextValue("Mods.Novias.UI.BesoDesbloqueado", nj, "Hakari"), 255, 190, 230);
                Main.NewText(Language.GetTextValue("Mods.Novias.UI.BesoDesbloqueado", nj, "Karane"), 239, 178, 97);

                player.GetModPlayer<HakariPlayer>().CompletacionPendiente = false;
                player.GetModPlayer<KaranePlayer>().CompletacionPendiente = false;
                player.GetModPlayer<HakariPlayer>().UIAbierta = false;
                player.GetModPlayer<KaranePlayer>().UIAbierta = false;
            },

            DialogosPresentacion = new[]
            {
                new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.CompartidaHakariKarane.Mision2.Dialogo0", NombreNPC = "Hakari", ColorNombre = CHakari },
                new LineaDialogo { EsJugador = true,  Key = "Mods.Novias.Misiones.CompartidaHakariKarane.Mision2.Dialogo1" },
                new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.CompartidaHakariKarane.Mision2.Dialogo2", NombreNPC = "Hakari y Karane", ColorNombre = CAmbas },
                new LineaDialogo { EsJugador = true,  Key = "Mods.Novias.Misiones.CompartidaHakariKarane.Mision2.Dialogo3" }, 
                new LineaDialogo { EsJugador = true,  Key = "Mods.Novias.Misiones.CompartidaHakariKarane.Mision2.Dialogo4" },
                new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.CompartidaHakariKarane.Mision2.Dialogo5", NombreNPC = "Hakari", ColorNombre = CHakari },
                new LineaDialogo { EsJugador = true,  Key = "Mods.Novias.Misiones.CompartidaHakariKarane.Mision2.Dialogo6" },
                new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.CompartidaHakariKarane.Mision2.Dialogo7", NombreNPC = "Karane", ColorNombre = CKarane },
                new LineaDialogo { EsJugador = true,  Key = "Mods.Novias.Misiones.CompartidaHakariKarane.Mision2.Dialogo8" },
                new LineaDialogo { EsJugador = true,  Key = "Mods.Novias.Misiones.CompartidaHakariKarane.Mision2.Dialogo9" },

            },

            DialogosCompletacion = new[]
            {
                new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.CompartidaHakariKarane.Mision2.Completacion0", NombreNPC = "Hakari", ColorNombre = CHakari },
                new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.CompartidaHakariKarane.Mision2.Completacion1", NombreNPC = "Hakari", ColorNombre = CHakari },
                new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.CompartidaHakariKarane.Mision2.Completacion2", NombreNPC = "Karane", ColorNombre = CKarane },
                new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.CompartidaHakariKarane.Mision2.Completacion3", NombreNPC = "Karane", ColorNombre = CKarane },
                new LineaDialogo { EsJugador = true,  Key = "Mods.Novias.Misiones.CompartidaHakariKarane.Mision2.Completacion4" },
                new LineaDialogo { EsJugador = true,  Key = "Mods.Novias.Misiones.CompartidaHakariKarane.Mision2.Completacion5" },
                new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.CompartidaHakariKarane.Mision2.Completacion6", NombreNPC = "Hakari y Karane", ColorNombre = CAmbas },
                new LineaDialogo { EsJugador = true,  Key = "Mods.Novias.Misiones.CompartidaHakariKarane.Mision2.Completacion7" },
                new LineaDialogo { EsJugador = true,  Key = "Mods.Novias.Misiones.CompartidaHakariKarane.Mision2.Completacion8" },
                new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.CompartidaHakariKarane.Mision2.Completacion9", NombreNPC = "Karane", ColorNombre = CKarane },
                new LineaDialogo { EsJugador = false, Key = "Mods.Novias.Misiones.CompartidaHakariKarane.Mision2.Completacion10", NombreNPC = "Karane", ColorNombre = CKarane },
            },
        };
    }
}