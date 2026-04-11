using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Novias.Projectiles;

namespace Novias.Items.Weapons.Ranged
{
    public class Cutter : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.rare = ItemRarityID.LightPurple;
            Item.value = Item.buyPrice(copper: 25);
            Item.noUseGraphic = true;
            Item.damage = 20;
            Item.crit = 15;
            Item.DamageType = DamageClass.Ranged;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useAnimation = 15;
            Item.useTime = 15;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.shoot = ModContent.ProjectileType<CutterProyectil>();
            Item.shootSpeed = 24f;
        }
    }
}