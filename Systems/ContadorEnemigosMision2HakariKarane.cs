using Terraria;
using Terraria.ModLoader;
using Novias.Players;

namespace Novias.Systems
{
    public class ContadorEnemigosMision2HakariKarane : GlobalNPC
    {
        public override void OnKill(NPC npc)
        {
            if (!npc.friendly && npc.lifeMax > 1)
            {
                var h = Main.LocalPlayer.GetModPlayer<HakariPlayer>();
                var k = Main.LocalPlayer.GetModPlayer<KaranePlayer>();
                if (h.Mision1CompartidaCompletada && h.Fase == 1
                    && h.EstaSiguiendo && k.EstaSiguiendo)
                {
                    h.ContadorEnemigosMision2++;
                    k.ContadorEnemigosMision2 = h.ContadorEnemigosMision2;
                }
            }
        }
    }
}