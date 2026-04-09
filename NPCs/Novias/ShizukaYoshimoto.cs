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
using Novias.Items.GirlfriendsItems.Shizuka;
using Novias.Projectiles;
using Novias.Items.Weapons.Mage;
using Novias.Buffs;
using Novias.Items.Potions;
using Novias.Effects;
using Novias.Items.Paintings;

namespace Novias.NPCs.Novias
{
    [AutoloadHead]
    public class ShizukaYoshimoto : ComportamientoNovia
    {
        protected override bool EstaSiguiendo => Main.LocalPlayer.GetModPlayer<ShizukaPlayer>().EstaSiguiendo;
        protected override Color ColorPolvo => new Color(178, 255, 255);
        protected override int BuffSeguimiento => ModContent.BuffType<ArmoniaMagica>();
        protected override int CooldownAtaque => 35;
        protected override int EfectoNovia => ModContent.ProjectileType<Corazon>();
        protected override int RegeneracionVida => 5;

        protected override void LanzarAtaque(Vector2 direccion)
        {
            NPC.frame.Y = 16 * NPC.frame.Height;
            Projectile.NewProjectile(
                NPC.GetSource_FromThis(),
                NPC.Center,
                direccion * 20f,
                ModContent.ProjectileType<NotaDeCanto>(),
                30, 7f, Main.myPlayer, NPC.whoAmI
            );
        }

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 20;
            NPCID.Sets.ShimmerTownTransform[Type] = false;
            NPC.Happiness
                .SetNPCAffection<HakariHanazono>(AffectionLevel.Like)
                .SetNPCAffection<KaraneInda>(AffectionLevel.Like);
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
            NPC.lifeMax = 2000;
            NPC.defense = 40;
            NPC.knockBackResist = 0.8f;
            NPC.HitSound = SoundID.NPCHit18;
            NPC.DeathSound = SoundID.NPCDeath20;
            NPC.townNPC = true;
            NPC.friendly = true;
        }

        public override void AddShops()
        {
            var tienda = new NPCShop(Type, "Shop");
            tienda.Add(ModContent.ItemType<ElRomanceDeLaDiadema>());
            tienda.Add(ModContent.ItemType<PocionDeEcosDeAmor>(), new Condition("", () => Main.LocalPlayer.GetModPlayer<ShizukaPlayer>().LeDioRegalo));
            tienda.Add(ModContent.ItemType<Familia>());
            tienda.Register();
        }

        public override void SetChatButtons(ref string button, ref string button2)
        {
            ShizukaPlayer modPlayer = Main.LocalPlayer.GetModPlayer<ShizukaPlayer>();
            button = Language.GetTextValue("Mods.Novias.NPCDialogue.ShizukaYoshimoto.BotonTienda");
            if (!modPlayer.LeDioRegalo)
                button2 = Language.GetTextValue("Mods.Novias.NPCDialogue.ShizukaYoshimoto.BotonRegalo");
            else
                button2 = modPlayer.EstaSiguiendo
                    ? Language.GetTextValue("Mods.Novias.NPCDialogue.ShizukaYoshimoto.BotonDejarSeguir")
                    : Language.GetTextValue("Mods.Novias.NPCDialogue.ShizukaYoshimoto.BotonSeguir");
        }

        public override void OnChatButtonClicked(bool firstButton, ref string shop)
        {
            Player jugador = Main.LocalPlayer;
            ShizukaPlayer modPlayer = jugador.GetModPlayer<ShizukaPlayer>();

            if (firstButton)
            {
                shop = "Shop";
                return;
            }

            if (!modPlayer.LeDioRegalo)
            {
                if (jugador.HasItem(ModContent.ItemType<TelefonoDeShizuka>()))
                {
                    jugador.ConsumeItem(ModContent.ItemType<TelefonoDeShizuka>());
                    modPlayer.LeDioRegalo = true;
                    Main.npcChatText = Language.GetTextValue("Mods.Novias.NPCDialogue.ShizukaYoshimoto.RegaloRecibido");
                    Animacion(jugador);
                }
                else
                {
                    Main.npcChatText = Language.GetTextValue("Mods.Novias.NPCDialogue.ShizukaYoshimoto.SinRegalo") + $"\n[i:{ModContent.ItemType<TelefonoDeShizuka>()}]";
                }
                return;
            }

            if (modPlayer.EstaSiguiendo)
            {
                modPlayer.EstaSiguiendo = false;
                NPC.aiStyle = NPCAIStyleID.Passive;
                Main.npcChatText = Language.GetTextValue("Mods.Novias.NPCDialogue.ShizukaYoshimoto.DejarDeSeguir");
            }
            else
            {
                modPlayer.EstaSiguiendo = true;
                NPC.aiStyle = 0;
                Main.npcChatText = Language.GetTextValue("Mods.Novias.NPCDialogue.ShizukaYoshimoto.Seguir");
            }
        }

        public override string GetChat()
        {
            ShizukaPlayer modPlayer = Main.LocalPlayer.GetModPlayer<ShizukaPlayer>();
            string prefijo = modPlayer.LeDioRegalo ? "Chat" : "PreRegalo";
            return Language.GetTextValue($"Mods.Novias.NPCDialogue.ShizukaYoshimoto.{prefijo}{Main.rand.Next(3)}");
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheHallow,
            });
        }

        public bool llego = false;
        public override bool CanTownNPCSpawn(int numTownNPCs)
        {
            if (llego)
                return true;

            int noviasPresentes = 0;
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (npc.active && npc.ModNPC is ComportamientoNovia)
                    noviasPresentes++;
            }

            if (noviasPresentes < 2)
                return false;

            int libros = 0;
            for (int i = 0; i < Main.LocalPlayer.inventory.Length; i++)
            {
                if (Main.LocalPlayer.inventory[i].type == ItemID.Book)
                    libros += Main.LocalPlayer.inventory[i].stack;
            }

            if (libros >= 5)
            {
                llego = true;
                return true;
            }

            return false;
        }
    }
}