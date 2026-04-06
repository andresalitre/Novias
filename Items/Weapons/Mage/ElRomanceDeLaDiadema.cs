using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Novias.Projectiles;
using Microsoft.Xna.Framework;

namespace Novias.Items.Weapons.Mage
{
    public class ElRomanceDeLaDiadema : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 48;
            Item.height = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 35;
            Item.useAnimation = 35;
            Item.autoReuse = true;
            Item.DamageType = DamageClass.Magic;
            Item.damage = 20;
            Item.knockBack = 4f;
            Item.rare = ItemRarityID.Blue;
            Item.mana = 13;
            Item.shoot = ModContent.ProjectileType<NotaDeCanto>();
            Item.shootSpeed = 18f;
            Item.value = Item.buyPrice(gold: 40);
            Item.noMelee = true;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-2f, -6f);
        }

        public override bool Shoot(Player player, Terraria.DataStructures.EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item35, position);

            for (int i = 0; i < 2; i++)
            {
                Vector2 direccionRandom = velocity.RotatedByRandom(MathHelper.ToRadians(180f));
                Vector2 offset = new Vector2(0f, -4f);
                Projectile.NewProjectile(source, position + offset, direccionRandom, ModContent.ProjectileType<NotaDeCanto>(), damage, knockback, player.whoAmI);
            }

            return false;
        }
    }
}