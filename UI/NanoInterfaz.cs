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
    public class NanoInterfaz : InterfazNovias
    {
        public static NanoInterfaz Instance => ModContent.GetInstance<NanoInterfaz>();
        protected override NoviaUIState CrearEstado() => new NanoUIState();
        protected override bool EsEstaNovia(NPC npc) => npc.ModNPC is NanoEiai;
    }

    public class NanoUIState : NoviaUIState
    {
        protected override Color ColorFondo => new Color(18, 10, 30);
        protected override Color ColorBorde => new Color(180, 130, 255);
        protected override Color ColorTitulo => new Color(210, 180, 255);
        protected override Color ColorDialogoNPC => new Color(210, 180, 255);

        protected override string NombreNPC => "Nano";
        protected override int BuffBeso => ModContent.BuffType<ArmoniaMagica>();

        protected override NoviasPlayerBase ObtenerPlayer() =>
            Main.LocalPlayer.GetModPlayer<NanoPlayer>();

        protected override MisionData[] ObtenerMisiones() =>
            NanoMisiones.ObtenerMisiones();

        protected override string ObtenerDialogoChat()
        {
            var p = Main.LocalPlayer.GetModPlayer<NanoPlayer>();
            int idx = p.Fase < 2 ? Main.rand.Next(3) : Main.rand.Next(5);
            string cat = p.Fase < 2 ? "PreMision" : "Chat";
            return Language.GetTextValue($"Mods.Novias.NPCDialogue.NanoEiai.{cat}{idx}");
        }

        protected override string ObtenerDialogoBeso() =>
            Language.GetTextValue("Mods.Novias.NPCDialogue.NanoEiai.Beso", Main.LocalPlayer.name);
        protected override string ObtenerDialogoSeguir() =>
            Language.GetTextValue("Mods.Novias.NPCDialogue.NanoEiai.Seguir");

        protected override string ObtenerDialogoDejarSeguir() =>
            Language.GetTextValue("Mods.Novias.NPCDialogue.NanoEiai.DejarDeSeguir");
    }
}