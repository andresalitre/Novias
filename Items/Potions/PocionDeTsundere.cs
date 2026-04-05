using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Novias.Buffs;

namespace Novias.Items.Potions
{
    public class PocionDeTsundere : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 26;
            Item.useStyle = ItemUseStyleID.DrinkLiquid;
            Item.useAnimation = 15;
            Item.useTime = 15;
            Item.consumable = true;
            Item.maxStack = 9999;
            Item.value = Item.buyPrice(gold: 8);
            Item.rare = ItemRarityID.Orange;
            Item.buffType = ModContent.BuffType<FuerzaDeTsundere>();
            Item.buffTime = 60 * 500;
        }

        public override bool? UseItem(Player player)
        {
            Terraria.Audio.SoundEngine.PlaySound(Terraria.ID.SoundID.Item3, player.position);
            return true;
        }
    }
}
