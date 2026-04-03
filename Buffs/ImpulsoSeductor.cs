using Terraria;
using Terraria.ModLoader;

namespace Novias.Buffs
{
    public class ImpulsoSeductor : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.lifeRegen += 2;
        }
    }
}