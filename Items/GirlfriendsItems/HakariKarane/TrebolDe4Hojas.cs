using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Microsoft.Xna.Framework;

namespace Novias.Items.GirlfriendsItems.HakariKarane
{
    public class TrebolDe4Hojas : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = 99;
            Item.value = Item.sellPrice(silver: 15);
            Item.rare = ItemRarityID.Green;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.HoldUp;
        }

    }
}