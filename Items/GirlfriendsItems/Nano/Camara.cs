using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Novias.Players;


namespace Novias.Items.GirlfriendsItems.Nano
{
    public class Camara : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 1;
            Item.rare = ItemRarityID.LightPurple;
            Item.useAnimation = 80;
            Item.useTime = 80;
            Item.useStyle = ItemUseStyleID.HoldUp;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Lens, 2)
                .AddIngredient(ItemID.IronBar, 15)
                .AddTile(TileID.Anvils)
                .Register();

            CreateRecipe()
                .AddIngredient(ItemID.Lens, 2)
                .AddIngredient(ItemID.LeadBar, 15)
                .AddTile(TileID.Anvils)
                .Register();
        }

        public override bool CanUseItem(Player player)
        {
            var nano = player.GetModPlayer<NanoPlayer>();
            if (!nano.EstaSiguiendo) return false;

            if (player.ZoneUnderworldHeight) return true;
            if (player.ZoneSnow) return true;
            if (player.position.Y < Main.worldSurface * 16 * 0.35f) return true;

            return false;
        }

        public override bool? UseItem(Player player)
        {
            SoundEngine.PlaySound(new SoundStyle("Novias/Sounds/Items/Camara"), player.position);

            int itemType = 0;

            if (player.ZoneUnderworldHeight)
                itemType = ModContent.ItemType<FotoInframundo>();
            else if (player.ZoneSnow)
                itemType = ModContent.ItemType<FotoNieve>();
            else if (player.position.Y < Main.worldSurface * 16 * 0.35f)
                itemType = ModContent.ItemType<FotoCielo>();

            if (itemType != 0)
                player.QuickSpawnItem(player.GetSource_FromThis(), itemType);

            return true;
        }
    }
}