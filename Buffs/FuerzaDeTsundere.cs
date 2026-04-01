using Terraria;
using Terraria.ModLoader;

namespace Novias.Buffs
{
    public class FuerzaDeTsundere : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetDamage(DamageClass.Melee) += 0.15f;
            player.GetCritChance(DamageClass.Melee) += 10;
        }
    }
}