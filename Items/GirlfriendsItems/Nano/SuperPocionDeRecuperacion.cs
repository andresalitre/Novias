using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Novias.Items.GirlfriendsItems.Nano
{
    public class SuperPocionDeRecuperacion : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 1;
            Item.rare = ItemRarityID.LightPurple;
            Item.useAnimation = 20;
            Item.useTime = 20;
            Item.useStyle = ItemUseStyleID.HoldUp;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.IceBlock, 15)
                .AddIngredient(ItemID.RecallPotion)
                .AddIngredient(ItemID.FallenStar, 5)
                .AddIngredient(ItemID.Vertebrae, 10)
                .AddTile(TileID.Bottles)
                .Register();

            CreateRecipe()
                .AddIngredient(ItemID.IceBlock, 15)
                .AddIngredient(ItemID.RecallPotion)
                .AddIngredient(ItemID.FallenStar, 5)
                .AddIngredient(ItemID.RottenChunk, 10)
                .AddTile(TileID.Bottles)
                .Register();
        }

        public override bool? UseItem(Player player)
        {
            SoundEngine.PlaySound(SoundID.Item29, player.position);
            return true;
        }
    }
}