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
using Novias.Items.Weapons.Ranged;
using Novias.Projectiles;
using Novias.Buffs;
using Novias.Items.Potions;
using Novias.Effects;
using Novias.Items.Ammo;

namespace Novias.NPCs
{
    [AutoloadHead]
    public class HakariHanazono : ComportamientoNovia
    {
        protected override bool EstaSiguiendo => Main.LocalPlayer.GetModPlayer<HakariPlayer>().EstaSiguiendo;
        protected override Color ColorPolvo => new Color(255, 105, 180);
        protected override int BuffSeguimiento => ModContent.BuffType<ImpulsoSeductor>();
        protected override int CooldownAtaque => 40;
        protected override int TipoProyectilRegalo => ModContent.ProjectileType<Corazon>();
        protected override int RegeneracionVida => 8;

        protected override void LanzarAtaque(Vector2 direccion)
        {
            NPC.frame.Y = 16 * NPC.frame.Height;
            Projectile.NewProjectile(
                NPC.GetSource_FromThis(),
                NPC.Center,
                direccion * 20f,
                ModContent.ProjectileType<LecheHakari>(),
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
            NPC.lifeMax = 2000;
            NPC.defense = 50;
            NPC.lifeRegen = 5;
            NPC.knockBackResist = 0.8f;
            NPC.HitSound = SoundID.NPCHit18;
            NPC.DeathSound = SoundID.NPCDeath20;
            NPC.townNPC = true;
            NPC.friendly = true;
        }

        public override void AddShops()
        {
            var tienda = new NPCShop(Type, "Shop");
            tienda.Add(ModContent.ItemType<CañonDeLeche>());
            tienda.Add(ModContent.ItemType<LecheHakariMunicion>());
            tienda.Add(ItemID.Peach);
            tienda.Add(ModContent.ItemType<PocionDeSeduccion>(), new Condition("", () => Main.LocalPlayer.GetModPlayer<HakariPlayer>().LeDioRegalo));
            tienda.Add(ModContent.ItemType<RefrescoDeMelocoton>(), new Condition("", () => Main.LocalPlayer.GetModPlayer<HakariPlayer>().LeDioRegalo));
            tienda.Register();
        }

        public override void SetChatButtons(ref string button, ref string button2)
        {
            HakariPlayer modPlayer = Main.LocalPlayer.GetModPlayer<HakariPlayer>();
            button = Language.GetTextValue("Mods.Novias.NPCDialogue.HakariHanazono.BotonTienda");
            if (!modPlayer.LeDioRegalo)
                button2 = Language.GetTextValue("Mods.Novias.NPCDialogue.HakariHanazono.BotonRegalo");
            else
                button2 = modPlayer.EstaSiguiendo
                    ? Language.GetTextValue("Mods.Novias.NPCDialogue.HakariHanazono.BotonDejarSeguir")
                    : Language.GetTextValue("Mods.Novias.NPCDialogue.HakariHanazono.BotonSeguir");
        }

        public override void OnChatButtonClicked(bool firstButton, ref string shop)
        {
            Player jugador = Main.LocalPlayer;
            HakariPlayer modPlayer = jugador.GetModPlayer<HakariPlayer>();

            if (firstButton)
            {
                shop = "Shop";
                return;
            }

            if (!modPlayer.LeDioRegalo)
            {
                if (jugador.HasItem(ModContent.ItemType<MedioRefrescoDeMelocoton>()))
                {
                    jugador.ConsumeItem(ModContent.ItemType<MedioRefrescoDeMelocoton>());
                    modPlayer.LeDioRegalo = true;
                    Main.npcChatText = Language.GetTextValue("Mods.Novias.NPCDialogue.HakariHanazono.RegaloRecibido");
                    DarRegalo(jugador);
                }
                else
                {
                    Main.npcChatText = Language.GetTextValue("Mods.Novias.NPCDialogue.HakariHanazono.SinRegalo");
                }
                return;
            }

            if (modPlayer.EstaSiguiendo)
            {
                modPlayer.EstaSiguiendo = false;
                NPC.aiStyle = NPCAIStyleID.Passive;
                Main.npcChatText = Language.GetTextValue("Mods.Novias.NPCDialogue.HakariHanazono.DejarDeSeguir");
            }
            else
            {
                modPlayer.EstaSiguiendo = true;
                NPC.aiStyle = 0;
                Main.npcChatText = Language.GetTextValue("Mods.Novias.NPCDialogue.HakariHanazono.Seguir");
            }
        }

        public override string GetChat()
        {
            HakariPlayer modPlayer = Main.LocalPlayer.GetModPlayer<HakariPlayer>();
            string prefijo = modPlayer.LeDioRegalo ? "Chat" : "PreRegalo";
            return Language.GetTextValue($"Mods.Novias.NPCDialogue.HakariHanazono.{prefijo}{Main.rand.Next(3)}");
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
            return NPC.downedBoss1;
        }
    }
}