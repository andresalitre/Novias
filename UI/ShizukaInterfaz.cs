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
    public class ShizukaInterfaz : InterfazNovias
    {
        public static ShizukaInterfaz Instance => ModContent.GetInstance<ShizukaInterfaz>();
        protected override NoviaUIState CrearEstado() => new ShizukaUIState();
        protected override bool EsEstaNovia(NPC npc) => npc.ModNPC is ShizukaYoshimoto;
    }

    public class ShizukaUIState : NoviaUIState
    {
        protected override Color ColorFondo => new Color(10, 10, 30);
        protected override Color ColorBorde => new Color(100, 180, 255);
        protected override Color ColorTitulo => new Color(180, 220, 255);
        protected override Color ColorDialogoNPC => new Color(180, 220, 255);
        protected override string NombreNPC => "Shizuka";
        protected override int BuffBeso => ModContent.BuffType<ArmoniaMagica>();

        protected override NoviasPlayerBase ObtenerPlayer() =>
            Main.LocalPlayer.GetModPlayer<ShizukaPlayer>();

        protected override MisionData[] ObtenerMisiones() =>
            ShizukaMisiones.ObtenerMisiones();

        protected override string ObtenerDialogoChat()
        {
            var p = Main.LocalPlayer.GetModPlayer<ShizukaPlayer>();
            int idx = p.Fase < 1 ? Main.rand.Next(3) : Main.rand.Next(5);
            string cat = p.Fase < 1 ? "PreMision" : "Chat";
            return Language.GetTextValue($"Mods.Novias.NPCDialogue.ShizukaYoshimoto.{cat}{idx}");
        }

        protected override string ObtenerDialogoBeso() =>
            Language.GetTextValue("Mods.Novias.NPCDialogue.ShizukaYoshimoto.Beso", Main.LocalPlayer.name);

        protected override string ObtenerDialogoSeguir() =>
            Language.GetTextValue("Mods.Novias.NPCDialogue.ShizukaYoshimoto.Seguir");

        protected override string ObtenerDialogoDejarSeguir() =>
            Language.GetTextValue("Mods.Novias.NPCDialogue.ShizukaYoshimoto.DejarDeSeguir");
    }
}