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
    public class HakariInterfaz : InterfazNovias
    {
        public static HakariInterfaz Instance => ModContent.GetInstance<HakariInterfaz>();
        protected override NoviaUIState CrearEstado() => new HakariUIState();
        protected override bool EsEstaNovia(NPC npc) => npc.ModNPC is HakariHanazono;
    }

    public class HakariUIState : NoviaUIState
    {
        protected override Color ColorFondo => new Color(28, 16, 38);
        protected override Color ColorBorde => new Color(200, 90, 150);
        protected override Color ColorTitulo => new Color(255, 190, 230);
        protected override Color ColorDialogoNPC => new Color(255, 190, 230);
        protected override string NombreNPC => "Hakari";
        protected override int BuffBeso => ModContent.BuffType<ImpulsoSeductor>();

        protected override Color ColorParaNombre(string nombre)
        {
            if (nombre == Main.LocalPlayer.name) return ColorDialogoJugador;
            if (nombre == "Karane") return new Color(255, 165, 0);
            return ColorDialogoNPC;
        }

        protected override NoviasPlayerBase ObtenerPlayer() =>
            Main.LocalPlayer.GetModPlayer<HakariPlayer>();

        protected override MisionData[] ObtenerMisiones() =>
            HakariMisiones.ObtenerMisiones();

        protected override string ObtenerDialogoChat()
        {
            var p = Main.LocalPlayer.GetModPlayer<HakariPlayer>();
            int idx = p.Fase < 1 ? Main.rand.Next(3) : Main.rand.Next(5);
            string cat = p.Fase < 1 ? "PreMision" : "Chat";
            return Language.GetTextValue($"Mods.Novias.NPCDialogue.HakariHanazono.{cat}{idx}");
        }

        protected override string ObtenerDialogoBeso() =>
            Language.GetTextValue("Mods.Novias.NPCDialogue.HakariHanazono.Beso", Main.LocalPlayer.name);

        protected override string ObtenerDialogoSeguir() =>
            Language.GetTextValue("Mods.Novias.NPCDialogue.HakariHanazono.Seguir");

        protected override string ObtenerDialogoDejarSeguir() =>
            Language.GetTextValue("Mods.Novias.NPCDialogue.HakariHanazono.DejarDeSeguir");
    }
}