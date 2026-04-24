using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Novias.Items.GirlfriendsItems.HakariKarane;

namespace Novias.Items.GirlfriendsItems.HakariKarane;

public class TrebolDrop : GlobalTile
{
    public override void KillTile(int i, int j, int type, ref bool fail, ref bool effectOnly, ref bool noItem)
    {
        if (type != TileID.Plants && type != TileID.Plants2) return;
        if (fail || effectOnly) return;

        Player player = Main.LocalPlayer;

        if (player.HeldItem.type != ItemID.Sickle) return;

        if (Main.rand.NextFloat() < 0.15f)
        {
            Item.NewItem(null, i * 16, j * 16, 16, 16, ModContent.ItemType<TrebolDe4Hojas>());
        }
    }
}