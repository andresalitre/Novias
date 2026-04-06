using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Microsoft.Xna.Framework;

namespace Novias.Items.GirlfriendsItems.Shizuka
{
    public class TelefonoDeShizuka : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(silver: 15);
            Item.rare = ItemRarityID.Blue;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.HoldUp;
        }

        public override void AddRecipes()
        {
            Recipe recipe1 = CreateRecipe();
            recipe1.AddIngredient(ItemID.PlatinumBar, 5);
            recipe1.AddIngredient(ItemID.Glass, 5);
            recipe1.AddIngredient(ItemID.Book);
            recipe1.AddIngredient(ItemID.Sapphire, 3);
            recipe1.AddTile(TileID.Anvils);
            recipe1.Register();

            Recipe recipe2 = CreateRecipe();
            recipe2.AddIngredient(ItemID.GoldBar, 5);
            recipe2.AddIngredient(ItemID.Glass, 5);
            recipe2.AddIngredient(ItemID.Book);
            recipe2.AddIngredient(ItemID.Sapphire, 3);
            recipe2.AddTile(TileID.Anvils);
            recipe2.Register();
        }

    }
}