using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Novias.Items.GirlfriendsItems.Hakari
{
    public class MedioRefrescoDeMelocoton : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 32;
            Item.useStyle = ItemUseStyleID.DrinkLiquid;
            Item.useAnimation = 17;
            Item.useTime = 17;
            Item.consumable = true;
            Item.maxStack = 9999;
            Item.rare = ItemRarityID.Pink;
            Item.buffType = BuffID.WellFed2;
            Item.buffTime = 60 * 300;
        }

        public override bool? UseItem(Player player)
        {
            Terraria.Audio.SoundEngine.PlaySound(Terraria.ID.SoundID.Item3, player.position);
            return true;
        }
    }
}