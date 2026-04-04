using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Novias.Projectiles
{
    public class LecheHakari : ModProjectile
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
            SoundEngine.PlaySound(SoundID.Item7, Projectile.Center);
            Projectile.direction = Projectile.velocity.X > 0 ? 1 : -1;
            Projectile.velocity *= 0.7f;
        }

        public override void AI()
        {
            Projectile.ai[0]++;
            Projectile.rotation += 0.15f * Projectile.direction;

            if (Projectile.ai[0] >= TiempoAntesDeGravedad)
            {
                Projectile.velocity.Y += 0.4f;
                Projectile.tileCollide = true;
            }
        }

        private void ExplotarLeche()
        {
            SoundEngine.PlaySound(SoundID.Splash, Projectile.Center);

            for (int i = 0; i < 20; i++)
            {
                Dust polvo = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Cloud);
                polvo.color = new Color(255, 255, 255);
                polvo.velocity = new Vector2(Main.rand.NextFloat(-6f, 6f), Main.rand.NextFloat(-8f, -2f));
                polvo.scale = Main.rand.NextFloat(1f, 2f);
                polvo.noGravity = false;
            }

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (npc.active && !npc.friendly && !npc.townNPC && Vector2.Distance(Projectile.Center, npc.Center) < 100f)
                    npc.SimpleStrikeNPC(Projectile.damage, Projectile.direction, false, Projectile.knockBack);
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            ExplotarLeche();
            Projectile.Kill();
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            ExplotarLeche();
            return true;
        }
    }
}