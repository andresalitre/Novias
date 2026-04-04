using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Novias.Projectiles;

namespace Novias.Items.Ammo
{
    public class LecheHakariMunicion : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 14;
            Item.height = 14;
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.ammo = ModContent.ItemType<LecheHakariMunicion>();
            Item.shoot = ModContent.ProjectileType<LecheHakari>();
            Item.value = Item.buyPrice(copper: 80);
            Item.shootSpeed = 1f;
            Item.damage = 10;
            Item.DamageType = DamageClass.Ranged;
            Item.rare = ItemRarityID.Pink;
        }
    }
}