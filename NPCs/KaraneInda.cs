using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.Events;
using Terraria.GameContent.Personalities;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Utilities;
using Novias.Players;
using Microsoft.Xna.Framework;
using Novias.Items.GirlfriendsItems.KaraneInda;
using Novias.Projectiles;
using Novias.Effects;
using Novias.Items.Weapons.Melee;
using Novias.Buffs;
using Novias.Items.Potions;

namespace Novias.NPCs
{
    [AutoloadHead]
    public class KaraneInda : ModNPC
    {
        private int TimerAnimacion = 0;
        private const int VelocidadFrame = 8;
        private int TimerAtaque = 0;
        private const int CooldownAtaque = 30;
        private bool EstaAtacando = false;

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 19;
            NPCID.Sets.ShimmerTownTransform[Type] = false;
            NPC.Happiness
                .SetBiomeAffection<HallowBiome>(AffectionLevel.Like)
                .SetBiomeAffection<DesertBiome>(AffectionLevel.Dislike);
            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Velocity = 1f
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
        }

        public override void SetDefaults()
        {
            NPC.width = 20;
            NPC.height = 38;
            NPC.aiStyle = NPCAIStyleID.Passive;
            NPC.lifeMax = 1000;
            NPC.defense = 75;
            NPC.knockBackResist = 0.8f;
            NPC.HitSound = SoundID.NPCHit18;
            NPC.DeathSound = SoundID.NPCDeath20;
            NPC.townNPC = true;
            NPC.friendly = true;
        }

        public override void AddShops()
        {
            var tienda = new NPCShop(Type, "Tienda");
            tienda.Add(ModContent.ItemType<GatitoMenso>());
            tienda.Add(ModContent.ItemType<PocionDeTsundere>());
            tienda.Register();
        }

        public override void SetChatButtons(ref string button, ref string button2)
        {
            KaranePlayer modPlayer = Main.LocalPlayer.GetModPlayer<KaranePlayer>();

            button = "Tienda";

            if (!modPlayer.LeDioRegalo)
                button2 = "Dar regalo";
            else
                button2 = modPlayer.EstaSiguiendo ? "Dejar de seguir" : "Seguir";
        }

        public override void OnChatButtonClicked(bool firstButton, ref string shop)
        {
            Player jugador = Main.LocalPlayer;
            KaranePlayer modPlayer = jugador.GetModPlayer<KaranePlayer>();

            if (firstButton)
            {
                shop = "Tienda";
                return;
            }

            if (!modPlayer.LeDioRegalo)
            {
                if (jugador.HasItem(ModContent.ItemType<GatitoDePeluche>()))
                {
                    jugador.ConsumeItem(ModContent.ItemType<GatitoDePeluche>());
                    modPlayer.LeDioRegalo = true;
                    Main.npcChatText = "¿U-un gatito...? N-no es como si me alegrara o algo así... pero... gracias.";

                    Projectile.NewProjectile(
                        jugador.GetSource_FromThis(),
                        jugador.Center,
                        new Vector2(0f, -8f),
                        ModContent.ProjectileType<KaraneLove>(),
                        0,
                        0f,
                        jugador.whoAmI
                    );
                }
                else
                {
                    Main.npcChatText = "¿Un regalo? No tengo tiempo tus bromas.";
                }
                return;
            }

            if (modPlayer.EstaSiguiendo)
            {
                modPlayer.EstaSiguiendo = false;
                NPC.aiStyle = NPCAIStyleID.Passive;
                Main.npcChatText = "Bien, como quieras.";
            }
            else
            {
                modPlayer.EstaSiguiendo = true;
                NPC.aiStyle = 0;
                Main.npcChatText = "N-no tengo de otra. N-no creas que lo hago porque quiera estar contigo.";
            }
        }

        public override string GetChat()
        {
            return Main.rand.Next(3) switch
            {
                0 => "¡N-no es como si quisiera hablar contigo!",
                1 => "N-no te confundas, no vine a tu mundo por ti o algo parecido...",
                _ => "¡¿Q-qué estás mirando?!"
            };
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheHallow,
            });
        }

        public override bool CanTownNPCSpawn(int numTownNPCs)
        {
            return NPC.downedBoss2;
        }

        public override void AI()
        {
            base.AI();
            NPC.spriteDirection = NPC.direction;

            KaranePlayer modPlayer = Main.LocalPlayer.GetModPlayer<KaranePlayer>();

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
                        float dist = NPC.Distance(npc.Center);
                        if (dist < distanciaMinima)
                        {
                            distanciaMinima = dist;
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
                    NPC.frame.Y = 15 * NPC.frame.Height;

                    Projectile.NewProjectile(
                        NPC.GetSource_FromThis(),
                        NPC.Center,
                        direccion * 20f,
                        ModContent.ProjectileType<GatitoMensoProyectil>(),
                        40,
                        2f,
                        Main.myPlayer,
                        NPC.whoAmI
                    );
                }
            }

            if (!modPlayer.EstaSiguiendo)
            {
                if (EstaAtacando)
                    NPC.velocity.X = 0f;
                return;
            }

            Main.LocalPlayer.AddBuff(ModContent.BuffType<FuerzaDeTsundere>(), 2);

            Player jugador = Main.LocalPlayer;
            float distancia = NPC.Distance(jugador.Center);

            if (distancia > 1000f)
            {
                for (int i = 0; i < 20; i++)
                {
                    Dust d = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, DustID.MagicMirror);
                    d.color = new Color(255, 140, 0);
                    d.velocity *= 2f;
                }
                SoundEngine.PlaySound(SoundID.Item6, NPC.position);
                NPC.Center = jugador.Center;
                for (int i = 0; i < 20; i++)
                {
                    Dust d = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, DustID.MagicMirror);
                    d.color = new Color(255, 140, 0);
                    d.velocity *= 2f;
                }
                return;
            }

            NPC.target = Main.myPlayer;

            if (distancia > 82f)
            {
                float velocidad = System.Math.Clamp(distancia / 50f, 2f, 12f);
                float diferenciaX = jugador.Center.X - NPC.Center.X;

                if (System.Math.Abs(diferenciaX) > 20f)
                {
                    NPC.velocity.X = diferenciaX > 0 ? velocidad : -velocidad;
                    NPC.direction = diferenciaX > 0 ? 1 : -1;
                    NPC.spriteDirection = NPC.direction;
                }
                else
                {
                    NPC.velocity.X = 0f;
                }

                if (NPC.collideX)
                    NPC.velocity.Y = -6f;
            }
            else
            {
                NPC.velocity.X = 0f;
            }
        }

        public override void FindFrame(int frameHeight)
        {
            int frameActual = NPC.frame.Y / frameHeight;
            bool moviendose = System.Math.Abs(NPC.velocity.X) >= 0.5f;

            if ((EstaAtacando || (frameActual >= 15 && frameActual <= 18)) && !moviendose)
            {
                if (frameActual < 15)
                {
                    NPC.frame.Y = frameHeight * 15;
                    TimerAnimacion = 0;
                    return;
                }
                TimerAnimacion++;
                if (TimerAnimacion >= 8)
                {
                    TimerAnimacion = 0;
                    frameActual++;
                    if (frameActual > 18)
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
            if (EstaAtacando && moviendose)
                EstaAtacando = false;
            bool estaHablando = Main.LocalPlayer.talkNPC == NPC.whoAmI && !moviendose && NPC.velocity.Y == 0f;
            if (estaHablando)
            {
                NPC.frame.Y = frameHeight * 14;
                TimerAnimacion = 0;
                return;
            }
            if (!NPC.IsABestiaryIconDummy && !NPC.velocity.Y.Equals(0f))
            {
                NPC.frame.Y = frameHeight * 1;
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
                int currentFrame = NPC.frame.Y / frameHeight;
                if (currentFrame < 2 || currentFrame > 13)
                    currentFrame = 2;
                else
                {
                    currentFrame++;
                    if (currentFrame > 13)
                        currentFrame = 2;
                }
                NPC.frame.Y = frameHeight * currentFrame;
            }
        }
    }
}