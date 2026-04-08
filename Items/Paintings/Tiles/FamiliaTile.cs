using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.ObjectData;
using Microsoft.Xna.Framework;

namespace Novias.Items.Paintings.Tiles
{
    public class FamiliaTile : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileLavaDeath[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3Wall);
            TileObjectData.addTile(Type);

            AddMapEntry(new Color(120, 85, 60));
            DustType = DustID.WoodFurniture;
        }
    }
}