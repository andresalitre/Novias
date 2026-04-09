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
using Novias.Items.GirlfriendsItems;
using Novias.Projectiles;
using Novias.Buffs;
using Novias.Items.Potions;
using Novias.Effects;

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
                direccion * 20f,
                ProjectileID.ThrowingKnife,
                25, 7f, Main.myPlayer, NPC.whoAmI
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
        {
            NanoPlayer modPlayer = Main.LocalPlayer.GetModPlayer<NanoPlayer>();
            return modPlayer.Ayudada;
        }

        public override void AddShops()
        {
            var tienda = new NPCShop(Type, "Shop");
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

            if (modPlayer.EsperandoDialogo)
            {
                NPC.velocity.X = 0f;
                return;
            }

            base.AI();
        }

        public override void SetChatButtons(ref string button, ref string button2)
        {
            NanoPlayer modPlayer = Main.LocalPlayer.GetModPlayer<NanoPlayer>();

            if (modPlayer.EsperandoDialogo)
            {
                button = Language.GetTextValue("Mods.Novias.NPCDialogue.NanoEiai.BotonHablar");
                button2 = "";
                return;
            }

            button = Language.GetTextValue("Mods.Novias.NPCDialogue.NanoEiai.BotonTienda");
            button2 = modPlayer.EstaSiguiendo
                ? Language.GetTextValue("Mods.Novias.NPCDialogue.NanoEiai.BotonDejarSeguir")
                : Language.GetTextValue("Mods.Novias.NPCDialogue.NanoEiai.BotonSeguir");
        }

        public override void OnChatButtonClicked(bool firstButton, ref string shop)
        {
            Player jugador = Main.LocalPlayer;
            NanoPlayer modPlayer = jugador.GetModPlayer<NanoPlayer>();

            if (modPlayer.EsperandoDialogo)
            {
                if (!firstButton) return;
                modPlayer.EsperandoDialogo = false;
                Main.npcChatText = Language.GetTextValue("Mods.Novias.NPCDialogue.NanoEiai.Gracias");
                return;
            }

            if (firstButton)
            {
                shop = "Shop";
                return;
            }

            if (modPlayer.EstaSiguiendo)
            {
                modPlayer.EstaSiguiendo = false;
                NPC.aiStyle = NPCAIStyleID.Passive;
                Main.npcChatText = Language.GetTextValue("Mods.Novias.NPCDialogue.NanoEiai.DejarDeSeguir");
            }
            else
            {
                modPlayer.EstaSiguiendo = true;
                NPC.aiStyle = 0;
                Main.npcChatText = Language.GetTextValue("Mods.Novias.NPCDialogue.NanoEiai.Seguir");
            }
        }

        public override string GetChat()
        {
            NanoPlayer modPlayer = Main.LocalPlayer.GetModPlayer<NanoPlayer>();
            if (modPlayer.EsperandoDialogo)
                return Language.GetTextValue("Mods.Novias.NPCDialogue.NanoEiai.EsperandoDialogo");
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
        {
            return Main.LocalPlayer.GetModPlayer<NanoPlayer>().Ayudada;
        }
    }
}