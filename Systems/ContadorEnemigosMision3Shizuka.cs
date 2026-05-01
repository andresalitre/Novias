using Terraria;
using Terraria.ModLoader;
using Novias.Players;
using Novias.NPCs.Misiones;

namespace Novias.Systems
{
    public class ContadorEnemigosMision3Shizuka : GlobalNPC
    {
        public override void OnKill(NPC npc)
        {
            if (!npc.friendly && npc.lifeMax > 1)
            {
                var s = Main.LocalPlayer.GetModPlayer<ShizukaPlayer>();
                var h = Main.LocalPlayer.GetModPlayer<HakariPlayer>();
                var k = Main.LocalPlayer.GetModPlayer<KaranePlayer>();

                if (s.EstaSiguiendo && !h.EstaSiguiendo && !k.EstaSiguiendo && s.MisionActual == 2)
                {
                    if (Main.LocalPlayer.ZoneCorrupt || Main.LocalPlayer.ZoneCrimson)
                    {
                        ShizukaMisiones.ContadorEnemigosMision3Shizuka++;
                    }
                }
            }
        }
    }
}