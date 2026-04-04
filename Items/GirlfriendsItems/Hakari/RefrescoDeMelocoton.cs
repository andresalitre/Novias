using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Novias.Items.GirlfriendsItems.Hakari;

namespace Novias.Items.GirlfriendsItems.Hakari
{
    public class RefrescoDeMelocoton : ModItem
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
            Item.value = Item.buyPrice(gold: 4);
            Item.rare = ItemRarityID.Pink;
            Item.buffType = BuffID.WellFed2;
            Item.buffTime = 60 * 300;
        }

        public override bool? UseItem(Player player)
        {
            Terraria.Audio.SoundEngine.PlaySound(Terraria.ID.SoundID.Item3, player.position);
            player.QuickSpawnItem(player.GetSource_FromThis(), ModContent.ItemType<MedioRefrescoDeMelocoton>());
            return true;
        }

        public override void AddRecipes()
        {
            Recipe recipe1 = CreateRecipe();
            recipe1.AddIngredient(ItemID.BottledWater);
            recipe1.AddIngredient(ItemID.CopperBar, 3);
            recipe1.AddIngredient(ItemID.Peach);
            recipe1.AddTile(TileID.Bottles);
            recipe1.Register();

            Recipe recipe2 = CreateRecipe();
            recipe2.AddIngredient(ItemID.BottledWater);
            recipe2.AddIngredient(ItemID.TinBar, 3);
            recipe2.AddIngredient(ItemID.Peach);
            recipe2.AddTile(TileID.Bottles);
            recipe2.Register();
        }
    }
}