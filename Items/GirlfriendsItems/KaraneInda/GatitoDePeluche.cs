using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Microsoft.Xna.Framework;

namespace Novias.Items.GirlfriendsItems.KaraneInda
{
    public class GatitoDePeluche : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(silver: 15);
            Item.rare = ItemRarityID.Orange;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.HoldUp;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Silk, 15);
            recipe.AddTile(TileID.Loom);
            recipe.Register();
        }

        public override bool? UseItem(Player player)
        {
            SoundEngine.PlaySound(SoundID.Item58, player.position);
            return true;
        }


    }
}