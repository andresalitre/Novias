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

namespace Novias.NPCs.Novias
{
    [AutoloadHead]
    public class KaraneInda : ComportamientoNovia
    {
        protected override bool EstaSiguiendo => Main.LocalPlayer.GetModPlayer<KaranePlayer>().EstaSiguiendo;
        protected override Color ColorPolvo => new Color(255, 140, 0);
        protected override int BuffSeguimiento => ModContent.BuffType<FuerzaDeTsundere>();
        protected override int CooldownAtaque => 30;
        protected override int EfectoNovia => ModContent.ProjectileType<Corazon>();
        protected override int RegeneracionVida => 5;

        protected override void LanzarAtaque(Vector2 direccion)
        {
            NPC.frame.Y = 16 * NPC.frame.Height;
            Projectile.NewProjectile(
                NPC.GetSource_FromThis(),
                NPC.Center,
                direccion * 20f,
                ModContent.ProjectileType<GatitoMensoProyectil>(),
                40, 7f, Main.myPlayer, NPC.whoAmI
            );
        }

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 20;
            NPCID.Sets.ShimmerTownTransform[Type] = false;
            NPC.Happiness
                .SetNPCAffection<HakariHanazono>(AffectionLevel.Like)
                .SetNPCAffection<ShizukaYoshimoto>(AffectionLevel.Like);
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
            NPC.defense = 50;
            NPC.knockBackResist = 0.8f;
            NPC.HitSound = SoundID.NPCHit18;
            NPC.DeathSound = SoundID.NPCDeath20;
            NPC.townNPC = true;
            NPC.friendly = true;
        }

        public override void AddShops()
        {
            var tienda = new NPCShop(Type, "Shop");
            tienda.Add(ModContent.ItemType<GatitoMenso>());
            tienda.Add(ModContent.ItemType<PocionDeTsundere>(), new Condition("", () => Main.LocalPlayer.GetModPlayer<KaranePlayer>().LeDioRegalo));
            tienda.Register();
        }

        public override void SetChatButtons(ref string button, ref string button2)
        {
            KaranePlayer modPlayer = Main.LocalPlayer.GetModPlayer<KaranePlayer>();
            button = Language.GetTextValue("Mods.Novias.NPCDialogue.KaraneInda.BotonTienda");
            if (!modPlayer.LeDioRegalo)
                button2 = Language.GetTextValue("Mods.Novias.NPCDialogue.KaraneInda.BotonRegalo");
            else
                button2 = modPlayer.EstaSiguiendo
                    ? Language.GetTextValue("Mods.Novias.NPCDialogue.KaraneInda.BotonDejarSeguir")
                    : Language.GetTextValue("Mods.Novias.NPCDialogue.KaraneInda.BotonSeguir");
        }

        public override void OnChatButtonClicked(bool firstButton, ref string shop)
        {
            Player jugador = Main.LocalPlayer;
            KaranePlayer modPlayer = jugador.GetModPlayer<KaranePlayer>();

            if (firstButton)
            {
                shop = "Shop";
                return;
            }

            if (!modPlayer.LeDioRegalo)
            {
                if (jugador.HasItem(ModContent.ItemType<GatitoDePeluche>()))
                {
                    jugador.ConsumeItem(ModContent.ItemType<GatitoDePeluche>());
                    modPlayer.LeDioRegalo = true;
                    Main.npcChatText = Language.GetTextValue("Mods.Novias.NPCDialogue.KaraneInda.RegaloRecibido");
                    Animacion(jugador);
                }
                else
                {
                    Main.npcChatText = Language.GetTextValue("Mods.Novias.NPCDialogue.KaraneInda.SinRegalo") + $"\n[i:{ModContent.ItemType<GatitoDePeluche>()}]";
                }
                return;
            }

            if (modPlayer.EstaSiguiendo)
            {
                modPlayer.EstaSiguiendo = false;
                NPC.aiStyle = NPCAIStyleID.Passive;
                Main.npcChatText = Language.GetTextValue("Mods.Novias.NPCDialogue.KaraneInda.DejarDeSeguir");
            }
            else
            {
                modPlayer.EstaSiguiendo = true;
                NPC.aiStyle = 0;
                Main.npcChatText = Language.GetTextValue("Mods.Novias.NPCDialogue.KaraneInda.Seguir");
            }
        }

        public override string GetChat()
        {
            KaranePlayer modPlayer = Main.LocalPlayer.GetModPlayer<KaranePlayer>();
            string prefijo = modPlayer.LeDioRegalo ? "Chat" : "PreRegalo";
            return Language.GetTextValue($"Mods.Novias.NPCDialogue.KaraneInda.{prefijo}{Main.rand.Next(3)}");
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