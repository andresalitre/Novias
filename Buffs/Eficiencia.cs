using Terraria;
using Terraria.ModLoader;

namespace Novias.Buffs
{
    public class Eficiencia : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.moveSpeed += 0.2f;
            player.pickSpeed -= 25f;
            player.GetAttackSpeed(DamageClass.Melee) += 0.15f;
        }
    }
}

