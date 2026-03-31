using Terraria;
using Terraria.ModLoader;

namespace Novias.Effects
{
    public class FuerzaDeTsundere : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = false;
            Main.buffNoSave[Type] = false;
            Main.buffNoTimeDisplay[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetDamage(DamageClass.Melee) += 0.15f;
        }
    }
}