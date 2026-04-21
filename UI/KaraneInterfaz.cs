using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Novias.Players;
using Novias.Systems;
using Novias.NPCs.Novias;
using Novias.NPCs.Misiones;
using Novias.Buffs;

namespace Novias.UI
{
    public class KaraneInterfaz : InterfazNovias
    {
        public static KaraneInterfaz Instance => ModContent.GetInstance<KaraneInterfaz>();

        protected override NoviaUIState CrearEstado() => new KaraneUIState();
        protected override bool EsEstaNovia(NPC npc) => npc.ModNPC is KaraneInda;
    }

    public class KaraneUIState : NoviaUIState
    {
        protected override Color ColorFondo => new Color(28, 16, 38);
        protected override Color ColorBorde => new Color(200, 90, 150);
        protected override Color ColorTitulo => new Color(255, 190, 230);

        protected override int BuffBeso => ModContent.BuffType<FuerzaDeTsundere>();

        protected override string NombreNPC => "Karane";

        protected override NoviasPlayerBase ObtenerPlayer() =>
            Main.LocalPlayer.GetModPlayer<KaranePlayer>();

        protected override MisionData[] ObtenerMisiones() =>
            KaraneMisiones.ObtenerMisiones();

        protected override string ObtenerDialogoChat()
        {
            var player = Main.LocalPlayer.GetModPlayer<KaranePlayer>();
            if (player.Fase < 1)
            {
                int idx = Main.rand.Next(3);
                return Language.GetTextValue($"Mods.Novias.NPCDialogue.KaraneInda.PreMision{idx}");
            }
            else
            {
                int idx = Main.rand.Next(5);
                return Language.GetTextValue($"Mods.Novias.NPCDialogue.KaraneInda.Chat{idx}");
            }
        }

        protected override string ObtenerDialogoBeso() =>
            Language.GetTextValue("Mods.Novias.NPCDialogue.KaraneInda.Beso", Main.LocalPlayer.name);

        protected override string ObtenerDialogoSeguir() =>
            Language.GetTextValue("Mods.Novias.NPCDialogue.KaraneInda.Seguir");

        protected override string ObtenerDialogoDejarSeguir() =>
            Language.GetTextValue("Mods.Novias.NPCDialogue.KaraneInda.DejarDeSeguir");
    }
}