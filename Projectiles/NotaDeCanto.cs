using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Novias.Projectiles
{
    public class NotaDeCanto : ModProjectile
    {
        private const int TiempoEstatico = 25;

        public override void SetDefaults()
        {
            Projectile.width = 28;
            Projectile.height = 30;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 360;
            Projectile.penetrate = -1;
            Projectile.damage = 0;
            Projectile.DamageType = DamageClass.Magic;
        }

        public override void AI()
        {
            Projectile.ai[0]++;

            Lighting.AddLight(Projectile.Center, 0.3f, 0.8f, 1f);

            Dust polvo = Dust.NewDustDirect(
                Projectile.position, Projectile.width, Projectile.height,
                DustID.RainbowMk2);
            polvo.velocity = Main.rand.NextVector2Circular(1f, 1f);
            polvo.scale = Main.rand.NextFloat(0.8f, 1.4f);
            polvo.noGravity = true;
            polvo.color = Main.hslToRgb(Main.GlobalTimeWrappedHourly * 0.5f % 1f, 1f, 0.5f);

            if (Projectile.ai[0] < TiempoEstatico)
            {
                Projectile.velocity *= 0.85f;
                Projectile.spriteDirection = Projectile.direction;
                Projectile.rotation = Projectile.velocity.ToRotation();
                if (Projectile.spriteDirection == -1)
                    Projectile.rotation += MathHelper.Pi;
            }
            else if (Projectile.ai[0] < TiempoEstatico + 8)
            {
                Projectile.velocity = Vector2.Zero;
            }
            else
            {
                NPC objetivo = BuscarObjetivo();

                if (objetivo != null)
                {
                    Vector2 punta = Projectile.Center - new Vector2(0, Projectile.height / 2f).RotatedBy(Projectile.rotation);
                    Vector2 direccion = objetivo.Center - punta;
                    direccion.Normalize();
                    Projectile.velocity = Vector2.Lerp(Projectile.velocity, direccion * 20f, 0.25f);

                    Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
                    Projectile.spriteDirection = Projectile.velocity.X > 0 ? 1 : -1;
                    if (Projectile.spriteDirection == -1)
                        Projectile.rotation += MathHelper.Pi;

                    if (Vector2.Distance(punta, objetivo.Center) < 30f)
                    {
                        Explosion();
                        Projectile.Kill();
                    }
                }
            }
        }

        private NPC BuscarObjetivo()
        {
            NPC objetivo = null;
            float distanciaMinima = 1200f;

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (!npc.active || npc.friendly || npc.townNPC) continue;

                float distancia = Vector2.Distance(Projectile.Center, npc.Center);
                if (distancia < distanciaMinima)
                {
                    distanciaMinima = distancia;
                    objetivo = npc;
                }
            }

            return objetivo;
        }

        private void Explosion()
        {
            if (Projectile.ai[1] == 1) return;
            Projectile.ai[1] = 1;

            SoundEngine.PlaySound(SoundID.Item30, Projectile.Center);

            for (int i = 0; i < 15; i++)
            {
                Dust polvoExp = Dust.NewDustDirect(
                    Projectile.position, Projectile.width, Projectile.height,
                    DustID.RainbowMk2);
                polvoExp.velocity = Main.rand.NextVector2Circular(6f, 6f);
                polvoExp.scale = Main.rand.NextFloat(1f, 2f);
                polvoExp.noGravity = true;
                polvoExp.color = Main.hslToRgb(Main.rand.NextFloat(), 1f, 0.5f);
            }

            if (Main.myPlayer == Projectile.owner)
            {
                float radioExplosion = 120f;
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC npc = Main.npc[i];
                    if (!npc.active || npc.friendly || npc.townNPC) continue;
                    if (Vector2.Distance(Projectile.Center, npc.Center) < radioExplosion)
                        npc.SimpleStrikeNPC(Projectile.damage, Projectile.direction, false, Projectile.knockBack);
                }
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Explosion();
            return true;
        }

        public override void OnKill(int timeLeft)
        {
            Explosion();
        }
    }
}