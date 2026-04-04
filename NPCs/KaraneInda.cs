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
using Novias.Items.GirlfriendsItems.Karane;
using Novias.Projectiles;
using Novias.Items.Weapons.Melee;
using Novias.Buffs;
using Novias.Items.Potions;
using Novias.Effects;

namespace Novias.NPCs
{
    [AutoloadHead]
    public class KaraneInda : ComportamientoNovia
    {
        protected override bool EstaSiguiendo => Main.LocalPlayer.GetModPlayer<KaranePlayer>().EstaSiguiendo;
        protected override Color ColorPolvo => new Color(255, 140, 0);
        protected override int BuffSeguimiento => ModContent.BuffType<FuerzaDeTsundere>();
        protected override int CooldownAtaque => 30;
        protected override int TipoProyectilRegalo => ModContent.ProjectileType<Corazon>();
        protected override int RegeneracionVida => 5;

        protected override void LanzarAtaque(Vector2 direccion)
        {
            NPC.frame.Y = 16 * NPC.frame.Height;
            Projectile.NewProjectile(
                NPC.GetSource_FromThis(),
                NPC.Center,
                direccion * 20f,
                ModContent.ProjectileType<GatitoMensoProyectil>(),
                45, 7f, Main.myPlayer, NPC.whoAmI
            );
        }

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 20;
            NPCID.Sets.ShimmerTownTransform[Type] = false;
            NPC.Happiness
                .SetNPCAffection<HakariHanazono>(AffectionLevel.Love);
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
            NPC.lifeMax = 2500;
            NPC.defense = 85;
            NPC.lifeRegen = 7;
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
                    DarRegalo(jugador);
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
                0 => "¡¿Q-qué estás mirando?! ¡No me mires con esa cara tan amable! ¡Me dan ganas de golpearte!",
                1 => "N-no te confundas, no vine a tu mundo para estar contigo o algo parecido...",
                _ => "¡N-no te acerques tanto! No es que me moleste… solo hace calor, ¿ok?!"
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
    }
}