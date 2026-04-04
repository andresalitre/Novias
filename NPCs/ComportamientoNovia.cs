using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Novias.Effects;

namespace Novias.NPCs
{
    public abstract class ComportamientoNovia : ModNPC
    {
        protected int TimerAtaque = 0;
        protected int TimerMerodeo = 0;
        protected int TimerAnimacion = 0;
        protected bool EstaAtacando = false;
        private int CooldownSalto = 0;
        private bool EnFila = false;

        protected abstract int CooldownAtaque { get; }
        protected abstract bool EstaSiguiendo { get; }
        protected abstract Color ColorPolvo { get; }
        protected abstract int BuffSeguimiento { get; }
        protected abstract int TipoProyectilRegalo { get; }
        protected abstract void LanzarAtaque(Vector2 direccion);
        protected abstract int RegeneracionVida { get; }

        protected virtual int TipoProyectilCorazon => ModContent.ProjectileType<Corazon>();

        protected void DarRegalo(Player jugador)
        {
            Projectile.NewProjectile(
                jugador.GetSource_FromThis(),
                jugador.Center,
                new Vector2(0f, -8f),
                TipoProyectilRegalo,
                0, 0f, jugador.whoAmI
            );

            Projectile.NewProjectile(
                NPC.GetSource_FromThis(),
                NPC.Center,
                new Vector2(0f, -8f),
                TipoProyectilCorazon,
                0, 0f, jugador.whoAmI
            );
        }

        private bool HayEscalonSubible()
        {
            Vector2 posicion = new Vector2(
                NPC.position.X + (NPC.direction == 1 ? NPC.width + 2 : -6),
                NPC.position.Y - 4
            );
            return !Collision.SolidCollision(posicion, 4, NPC.height - 8);
        }

        public override void AI()
        {
            TimerAtaque++;
            if (TimerAtaque >= CooldownAtaque && !EstaAtacando)
            {
                NPC objetivo = null;
                float distanciaMinima = 700f;
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC npc = Main.npc[i];
                    if (npc.active && !npc.friendly && !npc.townNPC && npc.damage > 0)
                    {
                        float distancia = NPC.Distance(npc.Center);
                        if (distancia < distanciaMinima)
                        {
                            distanciaMinima = distancia;
                            objetivo = npc;
                        }
                    }
                }

                if (objetivo != null)
                {
                    TimerAtaque = 0;
                    EstaAtacando = true;
                    Vector2 direccion = (objetivo.Center - NPC.Center).SafeNormalize(Vector2.UnitX);
                    NPC.direction = direccion.X > 0 ? 1 : -1;
                    NPC.spriteDirection = NPC.direction;
                    LanzarAtaque(direccion);
                }
            }

            if (!EstaSiguiendo)
            {
                if (EnFila)
                {
                    OrdenFila.RemoverDeFila(NPC.whoAmI);
                    EnFila = false;
                }
                base.AI();
                NPC.spriteDirection = NPC.direction;
                if (EstaAtacando)
                    NPC.velocity.X = 0f;
                return;
            }

            if (!EnFila)
            {
                EnFila = OrdenFila.RegistrarEnFila(NPC.whoAmI);
                if (!EnFila)
                    return;
            }

            Player player = Main.LocalPlayer;
            player.AddBuff(BuffSeguimiento, 2);

            if (CooldownSalto > 0)
                CooldownSalto--;

            NPC.velocity.Y += 0.4f;
            if (NPC.velocity.Y > 16f)
                NPC.velocity.Y = 16f;

            OrdenFila.ObtenerIndice(NPC.whoAmI, out int indice);
            Vector2 posicionObjetivo = OrdenFila.ObtenerPosicionEnFila(indice, NPC.Center.Y);
            float distanciaAlObjetivo = NPC.Distance(posicionObjetivo);
            float distanciaAlPlayer = NPC.Distance(player.Center);

            if (distanciaAlPlayer > 900f)
            {
                SpawnTeleportDust();
                SoundEngine.PlaySound(SoundID.Item6, NPC.position);
                NPC.Center = player.Center;
                SpawnTeleportDust();
                NPC.velocity = Vector2.Zero;
                return;
            }

            bool enElSuelo = NPC.collideY && NPC.velocity.Y >= 0f;
            float diferenciaX = posicionObjetivo.X - NPC.Center.X;

            if (distanciaAlObjetivo > 240f)
            {
                float velocidad = System.Math.Clamp(distanciaAlObjetivo / 60f, 2f, 5f);
                NPC.velocity.X = System.Math.Sign(diferenciaX) * velocidad;
            }
            else
            {
                TimerMerodeo--;
                if (TimerMerodeo <= 0)
                {
                    TimerMerodeo = Main.rand.Next(80, 220);
                    if (Main.rand.NextBool(4))
                        NPC.velocity.X = 0f;
                    else
                        NPC.velocity.X = Main.rand.NextFloat(0.4f, 1.2f) * (Main.rand.NextBool() ? 1f : -1f);
                }

                if (System.Math.Abs(diferenciaX) > 220f)
                    NPC.velocity.X = System.Math.Sign(diferenciaX) * 1.5f;
            }

            if (NPC.velocity.X != 0f)
            {
                NPC.direction = NPC.velocity.X > 0f ? 1 : -1;
                NPC.spriteDirection = NPC.direction;
            }

            if (enElSuelo && CooldownSalto <= 0 && NPC.collideX && !HayEscalonSubible())
            {
                NPC.velocity.Y = -11f;
                CooldownSalto = 30;
            }

            Collision.StepUp(ref NPC.position, ref NPC.velocity, NPC.width, NPC.height, ref NPC.stepSpeed, ref NPC.gfxOffY);
        }

        public override void FindFrame(int frameHeight)
        {
            int frameActual = NPC.frame.Y / frameHeight;
            bool moviendose = System.Math.Abs(NPC.velocity.X) >= 0.5f;
            bool enElAire = !NPC.IsABestiaryIconDummy && (NPC.velocity.Y < -0.1f || NPC.velocity.Y > 0.5f);
            bool estaHablando = Main.LocalPlayer.talkNPC == NPC.whoAmI && !moviendose && !enElAire;
            bool estaSentado = NPC.ai[0] == 5f;

            if (EstaAtacando && !moviendose && !enElAire)
            {
                if (frameActual < 16)
                {
                    NPC.frame.Y = frameHeight * 16;
                    TimerAnimacion = 0;
                    return;
                }
                TimerAnimacion++;
                if (TimerAnimacion >= 8)
                {
                    TimerAnimacion = 0;
                    frameActual++;
                    if (frameActual > 19)
                    {
                        EstaAtacando = false;
                        NPC.frame.Y = 0;
                    }
                    else
                    {
                        NPC.frame.Y = frameHeight * frameActual;
                    }
                }
                return;
            }

            if (EstaAtacando)
                EstaAtacando = false;

            if (enElAire)
            {
                NPC.frame.Y = frameHeight * 1;
                TimerAnimacion = 0;
                return;
            }

            if (estaSentado)
            {
                NPC.frame.Y = frameHeight * 15;
                TimerAnimacion = 0;
                return;
            }

            if (estaHablando)
            {
                NPC.frame.Y = frameHeight * 14;
                TimerAnimacion = 0;
                return;
            }

            if (!moviendose)
            {
                NPC.frame.Y = frameHeight * 0;
                TimerAnimacion = 0;
                return;
            }

            int velocidadAnimacion = (int)System.Math.Clamp(8f - System.Math.Abs(NPC.velocity.X) / 2f, 2f, 8f);
            TimerAnimacion++;
            if (TimerAnimacion >= velocidadAnimacion)
            {
                TimerAnimacion = 0;
                if (frameActual < 2 || frameActual > 13)
                    frameActual = 2;
                else
                {
                    frameActual++;
                    if (frameActual > 13)
                        frameActual = 2;
                }
                NPC.frame.Y = frameHeight * frameActual;
            }
        }

        public override void OnKill()
        {
            if (EnFila)
            {
                OrdenFila.RemoverDeFila(NPC.whoAmI);
                EnFila = false;
            }
        }

        private void SpawnTeleportDust()
        {
            for (int i = 0; i < 20; i++)
            {
                Dust polvo = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, DustID.MagicMirror);
                polvo.color = ColorPolvo;
                polvo.velocity *= 2f;
            }
        }

        public override void UpdateLifeRegen(ref int damage)
        {
            NPC.lifeRegen += RegeneracionVida * 2;
        }


        public override void HitEffect(NPC.HitInfo hit) { }
        public override bool? CanFallThroughPlatforms() => false;
    }
}