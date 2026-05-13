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
    public class KusuriInterfaz : InterfazNovias
    {
        public static KusuriInterfaz Instance => ModContent.GetInstance<KusuriInterfaz>();
        protected override NoviaUIState CrearEstado() => new KusuriUIState();
        protected override bool EsEstaNovia(NPC npc) => npc.ModNPC is KusuriYakuzen;
    }

    public class KusuriUIState : NoviaUIState
    {
        protected override Color ColorFondo => new Color(25, 5, 5);
        protected override Color ColorBorde => new Color(220, 40, 40);
        protected override Color ColorTitulo => new Color(255, 120, 120);
        protected override Color ColorDialogoNPC => new Color(255, 120, 120);
        protected override string NombreNPC => "Kusuri";
        protected override int BuffBeso => ModContent.BuffType<FuerzaDeTsundere>();
        protected override int MisionParaDialogo => 2;

        protected override NoviasPlayerBase ObtenerPlayer() =>
            Main.LocalPlayer.GetModPlayer<KusuriPlayer>();

        protected override MisionData[] ObtenerMisiones() =>
            KusuriMisiones.ObtenerMisiones();

        protected override string ObtenerDialogoChat()
        {
            var p = Main.LocalPlayer.GetModPlayer<KusuriPlayer>();
            int idx = p.Fase < 1 ? Main.rand.Next(3) : Main.rand.Next(5);
            string cat = p.Fase < 1 ? "PreMision" : "Chat";
            return Language.GetTextValue($"Mods.Novias.NPCDialogue.KusuriYakuzen.{cat}{idx}");
        }

        protected override string ObtenerDialogoBeso() =>
            Language.GetTextValue("Mods.Novias.NPCDialogue.KusuriYakuzen.Beso", Main.LocalPlayer.name);

        protected override string ObtenerDialogoSeguir() =>
            Language.GetTextValue("Mods.Novias.NPCDialogue.KusuriYakuzen.Seguir");

        protected override string ObtenerDialogoDejarSeguir() =>
            Language.GetTextValue("Mods.Novias.NPCDialogue.KusuriYakuzen.DejarDeSeguir");
    }
}