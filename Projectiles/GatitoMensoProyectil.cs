using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Novias.Projectiles
{
    public class GatitoMensoProyectil : ModProjectile
    {
        private const int TiempoAntesDeGravedad = 40;

        public override void SetDefaults()
        {
            Projectile.width = 35;
            Projectile.height = 35;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = false;  
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 360;
            Projectile.penetrate = -1;    
            Projectile.DamageType = DamageClass.Melee;

        }

        public override void OnSpawn(Terraria.DataStructures.IEntitySource source)
        {
            SoundEngine.PlaySound(SoundID.Item57, Projectile.Center);
        }

        public override void AI()
        {
            Projectile.ai[0]++;

            if (Projectile.ai[0] >= TiempoAntesDeGravedad)
            {
                Projectile.velocity.Y += 0.4f;
                Projectile.tileCollide = true;
            }


            if (Main.rand.NextBool(4))
            {
                Dust polvo = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.TreasureSparkle);
                polvo.color = new Color(225, 140, 60);
                polvo.velocity *= 0.3f;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            SoundEngine.PlaySound(SoundID.Item57, target.Center);
        }

    }
}