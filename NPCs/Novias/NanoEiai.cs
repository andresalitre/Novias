using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.Personalities;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Novias.Players;
using Novias.Systems;
using Microsoft.Xna.Framework;
using Novias.Buffs;
using Novias.Items.Potions;
using Novias.Effects;
using Novias.Projectiles;
using Novias.Items.GirlfriendsItems.Nano;
using Novias.Items.Weapons.Ranged;

namespace Novias.NPCs.Novias
{
    [AutoloadHead]
    public class NanoEiai : ComportamientoNovia
    {
        protected override bool EstaSiguiendo => Main.LocalPlayer.GetModPlayer<NanoPlayer>().EstaSiguiendo;
        protected override Color ColorPolvo => new Color(255, 255, 255);
        protected override int BuffSeguimiento => ModContent.BuffType<Eficiencia>();
        protected override int CooldownAtaque => 25;
        protected override int EfectoNovia => ModContent.ProjectileType<Corazon>();
        protected override int RegeneracionVida => 7;

        protected override void LanzarAtaque(Vector2 direccion)
        {
            NPC.frame.Y = 16 * NPC.frame.Height;
            Projectile.NewProjectile(
                NPC.GetSource_FromThis(),
                NPC.Center,
                direccion * 18f,
                ModContent.ProjectileType<CutterProyectil>(),
                15, 18f, Main.myPlayer, NPC.whoAmI
            );
        }

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 20;
            NPCID.Sets.ShimmerTownTransform[Type] = false;
            NPC.Happiness
                .SetNPCAffection<HakariHanazono>(AffectionLevel.Like)
                .SetNPCAffection<KaraneInda>(AffectionLevel.Like)
                .SetNPCAffection<ShizukaYoshimoto>(AffectionLevel.Love);
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
            NPC.defense = 65;
            NPC.knockBackResist = 0.8f;
            NPC.HitSound = SoundID.NPCHit18;
            NPC.DeathSound = SoundID.NPCDeath20;
            NPC.townNPC = true;
            NPC.friendly = true;
        }

        public override bool CheckConditions(int left, int top, int right, int bottom)
            => NoviasWorld.NanoAyudada;

        public override void AddShops()
        {
            var tienda = new NPCShop(Type, "Shop");
            tienda.Add(ModContent.ItemType<Cutter>());
            tienda.Add(ModContent.ItemType<PocionDeEficiencia>());
            tienda.Register();
        }

        public override void AI()
        {
            NanoPlayer modPlayer = Main.LocalPlayer.GetModPlayer<NanoPlayer>();

            if (modPlayer.HacerAnimacion)
            {
                modPlayer.HacerAnimacion = false;
                Animacion(Main.LocalPlayer);
            }

            if (NoviasWorld.NanoEsperandoDialogo)
            {
                NPC.velocity.X = 0f;

                Player jugadorCercano = null;
                float distanciaMinima = 300f;
                foreach (Player player in Main.ActivePlayers)
                {
                    float distancia = NPC.Distance(player.Center);
                    if (distancia < distanciaMinima)
                    {
                        distanciaMinima = distancia;
                        jugadorCercano = player;
                    }
                }
                if (jugadorCercano != null)
                {
                    NPC.direction = jugadorCercano.Center.X > NPC.Center.X ? 1 : -1;
                    NPC.spriteDirection = NPC.direction;
                }
                return;
            }

            base.AI();
        }

        public override void SetChatButtons(ref string button, ref string button2)
        {
            NanoPlayer modPlayer = Main.LocalPlayer.GetModPlayer<NanoPlayer>();
            button = Language.GetTextValue("Mods.Novias.NPCDialogue.NanoEiai.BotonTienda");
            button2 = modPlayer.EstaSiguiendo
                ? Language.GetTextValue("Mods.Novias.NPCDialogue.NanoEiai.BotonDejarSeguir")
                : Language.GetTextValue("Mods.Novias.NPCDialogue.NanoEiai.BotonSeguir");
        }

        public override void OnChatButtonClicked(bool firstButton, ref string shop)
        {
            Player jugador = Main.LocalPlayer;
            NanoPlayer modPlayer = jugador.GetModPlayer<NanoPlayer>();

            if (firstButton)
            {
                shop = "Shop";
                return;
            }

            if (modPlayer.EstaSiguiendo)
            {
                modPlayer.EstaSiguiendo = false;
                NPC.aiStyle = NPCAIStyleID.Passive;
                NoviasWorld.NanoSiguiendo = -1;
                Main.npcChatText = Language.GetTextValue("Mods.Novias.NPCDialogue.NanoEiai.DejarDeSeguir");
            }
            else
            {
                if (NoviasWorld.NanoSiguiendo != -1 && NoviasWorld.NanoSiguiendo != jugador.whoAmI)
                {
                    Main.npcChatText = Language.GetTextValue("Mods.Novias.NPCDialogue.NanoEiai.Ocupada");
                    return;
                }
                modPlayer.EstaSiguiendo = true;
                NPC.aiStyle = 0;
                NoviasWorld.NanoSiguiendo = jugador.whoAmI;
                Main.npcChatText = Language.GetTextValue("Mods.Novias.NPCDialogue.NanoEiai.Seguir");
            }
        }

        public override string GetChat()
        {
            NanoPlayer modPlayer = Main.LocalPlayer.GetModPlayer<NanoPlayer>();

            if (NoviasWorld.NanoEsperandoDialogo)
            {
                NoviasWorld.NanoEsperandoDialogo = false;
                return Language.GetTextValue("Mods.Novias.NPCDialogue.NanoEiai.Gracias");
            }

            return Language.GetTextValue($"Mods.Novias.NPCDialogue.NanoEiai.Chat{Main.rand.Next(3)}");
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Sky,
            });
        }

        public override bool CanTownNPCSpawn(int numTownNPCs)
            => NoviasWorld.NanoAyudada;
    }
}