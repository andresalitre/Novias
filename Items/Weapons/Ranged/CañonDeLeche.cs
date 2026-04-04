using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Novias.Projectiles;
using Novias.Items.Ammo;
using Microsoft.Xna.Framework;

namespace Novias.Items.Weapons.Ranged
{
    public class CañonDeLeche : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 48;
            Item.height = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.autoReuse = true;
            Item.DamageType = DamageClass.Ranged;
            Item.damage = 35;
            Item.knockBack = 4f;
            Item.rare = ItemRarityID.Pink;
            Item.shoot = ModContent.ProjectileType<LecheHakari>();
            Item.shootSpeed = 18f;
            Item.value = Item.buyPrice(gold: 40);
            Item.useAmmo = ModContent.ItemType<LecheHakariMunicion>();
            Item.noMelee = true;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-2f, -6f);
        }

        public override bool Shoot(Player player, Terraria.DataStructures.EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item62, position);
            Vector2 offset = new Vector2(0f, -4f);
            Projectile.NewProjectile(source, position + offset, velocity, ModContent.ProjectileType<LecheHakari>(), damage, knockback, player.whoAmI);
            return false;
        }

    }
}