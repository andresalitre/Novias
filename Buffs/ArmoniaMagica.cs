using Terraria;
using Terraria.ModLoader;

namespace Novias.Buffs
{
    public class ArmoniaMagica : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.statManaMax2 += 150;
            player.manaRegenBonus += 300;
        }
    }
}
