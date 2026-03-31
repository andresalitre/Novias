using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Novias.Projectiles;

namespace Novias.WeaponsMelee
{
    public class GatitoMenso : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 40;
            Item.DamageType = DamageClass.Melee;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.knockBack = 5f;
            Item.value = Item.buyPrice(gold: 3);
            Item.rare = ItemRarityID.Yellow;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<GatitoMensoProyectil>();
            Item.shootSpeed = 20f;
            Item.noUseGraphic = true;
        }
    }
}