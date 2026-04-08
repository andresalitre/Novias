using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Novias.Items.Paintings;
using Novias.Items.Paintings.Tiles;

namespace Novias.Items.Paintings
{
    public class Familia : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = 99;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.value = Item.buyPrice(silver: 50);
            Item.rare = ItemRarityID.White;
            Item.createTile = ModContent.TileType<FamiliaTile>();
        }
    }
}