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
        protected override Color ColorFondo => new Color(31, 16, 1);
        protected override Color ColorBorde => new Color(255, 165, 0);
        protected override Color ColorTitulo => new Color(239, 178, 97);
        protected override Color ColorDialogoNPC => new Color(239, 178, 97);
        protected override string NombreNPC => "Karane";
        protected override int BuffBeso => ModContent.BuffType<FuerzaDeTsundere>();

        protected override Color ColorParaNombre(string nombre)
        {
            if (nombre == Main.LocalPlayer.name) return ColorDialogoJugador;
            if (nombre == "Hakari") return new Color(255, 105, 180);
            return ColorDialogoNPC;
        }

        protected override NoviasPlayerBase ObtenerPlayer() =>
            Main.LocalPlayer.GetModPlayer<KaranePlayer>();

        protected override MisionData[] ObtenerMisiones() =>
            KaraneMisiones.ObtenerMisiones();

        protected override string ObtenerDialogoChat()
        {
            var p = Main.LocalPlayer.GetModPlayer<KaranePlayer>();
            int idx = p.Fase < 1 ? Main.rand.Next(3) : Main.rand.Next(5);
            string cat = p.Fase < 1 ? "PreMision" : "Chat";
            return Language.GetTextValue($"Mods.Novias.NPCDialogue.KaraneInda.{cat}{idx}");
        }

        protected override string ObtenerDialogoBeso() =>
            Language.GetTextValue("Mods.Novias.NPCDialogue.KaraneInda.Beso", Main.LocalPlayer.name);

        protected override string ObtenerDialogoSeguir() =>
            Language.GetTextValue("Mods.Novias.NPCDialogue.KaraneInda.Seguir");

        protected override string ObtenerDialogoDejarSeguir() =>
            Language.GetTextValue("Mods.Novias.NPCDialogue.KaraneInda.DejarDeSeguir");
    }
}