using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.Graphics.CameraModifiers;
using Terraria.ID;
using Terraria.ModLoader;

namespace Novias.Content.Projectiles
{
    public class GatitoDePelucheProyectil : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 60;
            Projectile.penetrate = -1;
            Projectile.damage = 0;
        }

        public override void AI()
        {
            if (Projectile.timeLeft > 20)
            {
                Projectile.velocity.Y = -4f;
                Projectile.velocity.X = 0f;
                Projectile.rotation = 0f;
                Projectile.position.X += Main.rand.Next(-1, 2);
            }
            else
            {
                Projectile.velocity = Vector2.Zero;
                Projectile.position.X += Main.rand.Next(-2, 3);
                Projectile.position.Y += Main.rand.Next(-1, 2);
            }
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 25; i++)
            {
                Dust polvo = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.TreasureSparkle);
                polvo.color = new Color(225, 140, 60);
                polvo.velocity = Main.rand.NextVector2Circular(4f, 4f);
                polvo.scale = 1.5f;
            }

            SoundEngine.PlaySound(SoundID.Item58, Projectile.position);
            Main.instance.CameraModifiers.Add(new PunchCameraModifier(Projectile.Center, Vector2.UnitY, 10f, 6f, 20));
        }
    }
}