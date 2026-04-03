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
using Novias.Items.GirlfriendsItems.Hakari;
using Novias.Projectiles;
using Novias.Items.Weapons.Melee;
using Novias.Buffs;
using Novias.Items.Potions;
using Novias.Effects;

namespace Novias.NPCs
{
    [AutoloadHead]
    public class HakariHanazono : ComportamientoNovia
    {
        protected override bool EstaSiguiendo => Main.LocalPlayer.GetModPlayer<HakariPlayer>().EstaSiguiendo;
        protected override Color ColorPolvo => new Color(255, 105, 180);
        protected override int BuffSeguimiento => ModContent.BuffType<ImpulsoSeductor>();
        protected override int CooldownAtaque => 35;
        protected override int TipoProyectilRegalo => ModContent.ProjectileType<KaraneLove>();

        protected override void LanzarAtaque(Vector2 direccion)
        {
            NPC.frame.Y = 16 * NPC.frame.Height;
            Projectile.NewProjectile(
                NPC.GetSource_FromThis(),
                NPC.Center,
                direccion * 20f,
                ModContent.ProjectileType<GatitoMensoProyectil>(),
                25, 2f, Main.myPlayer, NPC.whoAmI
            );
        }

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 20;
            NPCID.Sets.ShimmerTownTransform[Type] = false;
            NPC.Happiness
                .SetNPCAffection<KaraneInda>(AffectionLevel.Love);
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
            NPC.lifeMax = 1250;
            NPC.defense = 50;
            NPC.knockBackResist = 0.5f;
            NPC.HitSound = SoundID.NPCHit18;
            NPC.DeathSound = SoundID.NPCDeath20;
            NPC.townNPC = true;
            NPC.friendly = true;
            NPC.lifeRegen += 5;
        }

        public override void AddShops()
        {
            var tienda = new NPCShop(Type, "Tienda");
            // tienda.Add(ModContent.ItemType<GatitoMenso>());
            // tienda.Add(ModContent.ItemType<PocionDeTsundere>());
            tienda.Register();
        }

        public override void SetChatButtons(ref string button, ref string button2)
        {
            HakariPlayer modPlayer = Main.LocalPlayer.GetModPlayer<HakariPlayer>();
            button = "Tienda";
            if (!modPlayer.LeDioRegalo)
                button2 = "Dar regalo";
            else
                button2 = modPlayer.EstaSiguiendo ? "Dejar de seguir" : "Seguir";
        }

        public override void OnChatButtonClicked(bool firstButton, ref string shop)
        {
            Player jugador = Main.LocalPlayer;
            HakariPlayer modPlayer = jugador.GetModPlayer<HakariPlayer>();

            if (firstButton)
            {
                shop = "Tienda";
                return;
            }

            if (!modPlayer.LeDioRegalo)
            {
                if (jugador.HasItem(ModContent.ItemType<RegaloHakari>()))
                {
                    jugador.ConsumeItem(ModContent.ItemType<RegaloHakari>());
                    modPlayer.LeDioRegalo = true;
                    Main.npcChatText = "¡¿E-esto es para mí?! Lo voy a guardar como un tesoro y lo abrazare cada noche pensando en ti… ¡Gracias!";
                    DarRegalo(jugador);
                }
                else
                {
                    Main.npcChatText = "¿No tienes nada? No es necesario algo... solo acercate...";
                }
                return;
            }

            if (modPlayer.EstaSiguiendo)
            {
                modPlayer.EstaSiguiendo = false;
                NPC.aiStyle = NPCAIStyleID.Passive;
                Main.npcChatText = "E-eh…? ¿Ya no hace falta?";
            }
            else
            {
                modPlayer.EstaSiguiendo = true;
                NPC.aiStyle = 0;
                Main.npcChatText = "¿Seguirte? ¡Claro que sí! Donde tú vayas, iré pegadita a tu lado";
            }
        }

        public override string GetChat()
        {
            return Main.rand.Next(3) switch
            {
                0 => "Si sigues hablandome, voy a empezar a pensar que soy tu favorita... me gusta esa idea.",
                1 => "Así que aquí es donde vives… me alegra haber llegado, ahora podré estar cerca de ti todo el tiempo.",
                _ => "Creo que me torcí el tobillo… ¿podrías ayudarme a caminar? Prometo no soltarme de tu brazo."
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