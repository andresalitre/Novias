using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace Novias.Projectiles
{
    public class CutterProyectil : ModProjectile
    {
        private int timerPersecucion = 20;

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 300;
            Projectile.penetrate = 1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }

        public override void OnSpawn(Terraria.DataStructures.IEntitySource source)
        {
            SoundEngine.PlaySound(SoundID.Item1, Projectile.position);
        }

        public override void AI()
        {
            NPC objetivo = null;

            if (timerPersecucion > 0)
            {
                timerPersecucion--;

                float distanciaMinima = 300f;
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC npc = Main.npc[i];
                    if (npc.active && !npc.friendly && npc.damage > 0)
                    {
                        float distancia = Projectile.Distance(npc.Center);
                        if (distancia < distanciaMinima)
                        {
                            distanciaMinima = distancia;
                            objetivo = npc;
                        }
                    }
                }

                if (objetivo != null)
                {
                    Vector2 direccion = (objetivo.Center - Projectile.Center).SafeNormalize(Vector2.Zero);
                    Projectile.velocity = Vector2.Lerp(Projectile.velocity, direccion * 24f, 0.12f);
                }
                else
                {
                    Projectile.velocity.Y += 0.15f;
                    if (Projectile.velocity.Y > 16f)
                        Projectile.velocity.Y = 16f;
                }
            }
            else
            {
                Projectile.velocity.Y += 0.15f;
                if (Projectile.velocity.Y > 16f)
                    Projectile.velocity.Y = 16f;
            }

            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.ArmorPenetration += 5;
        }
    }
}