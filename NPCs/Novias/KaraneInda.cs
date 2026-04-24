using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Novias.Players;
using Novias.Projectiles;
using Novias.Effects;
using Novias.NPCs.Misiones;
using Novias.UI;
using Novias.Items.GirlfriendsItems.Karane;
using Novias.Items.Potions;
using Novias.Items.Weapons.Melee;
using Novias.Players;

namespace Novias.NPCs.Novias
{
    [AutoloadHead]
    public class KaraneInda : ComportamientoNovia
    {
        protected override bool EstaSiguiendo => Main.LocalPlayer.GetModPlayer<KaranePlayer>().EstaSiguiendo;
        protected override Color ColorPolvo => new Color(255, 165, 0);
        protected override int CooldownAtaque => 45;
        protected override int EfectoNovia => ModContent.ProjectileType<Corazon>();
        protected override int RegeneracionVida => 8;

        protected override bool EstaHablandoConInterfaz =>
            InterfazNovias.InterfazAbierta<KaraneInterfaz>();

        protected override void LanzarAtaque(Vector2 direccion)
        {
            NPC.frame.Y = 16 * NPC.frame.Height;
            Projectile.NewProjectile(
                NPC.GetSource_FromThis(),
                NPC.Center,
                direccion * 20f,
                ModContent.ProjectileType<GatitoMensoProyectil>(),
                20, 2f, Main.myPlayer, NPC.whoAmI
            );
        }

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 20;
            NPCID.Sets.ShimmerTownTransform[Type] = false;
            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new() { Velocity = 1f };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
        }

        public override void SetDefaults()
        {
            NPC.width = 20;
            NPC.height = 38;
            NPC.aiStyle = NPCAIStyleID.Passive;
            NPC.lifeMax = 2500;
            NPC.defense = 75;
            NPC.knockBackResist = 0.8f;
            NPC.HitSound = SoundID.NPCHit18;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.townNPC = true;
            NPC.friendly = true;
        }

        public override void AddShops()
        {
            var tienda = new NPCShop(Type, "Shop");
            tienda.Add(ModContent.ItemType<GatitoMenso>());
            tienda.Add(ModContent.ItemType<PocionDeTsundere>(), new Condition("Mods.Novias.Condiciones.MisionCompletada", () => Main.LocalPlayer.GetModPlayer<KaranePlayer>().MisionActual >= 1));
            tienda.Register();
        }

        public override void SetChatButtons(ref string button, ref string button2)
        {
            button = Language.GetTextValue("LegacyInterface.28");
            button2 = "";
        }

        public override void OnChatButtonClicked(bool firstButton, ref string shop)
        {
            if (firstButton) shop = "Shop";
        }

        public override bool CanChat() => true;

        public override string GetChat()
        {
            KaraneInterfaz.Instance._state?.IniciarConNPC(NPC);
            return " ";
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheHallow,
            });
        }

        public override bool CanTownNPCSpawn(int numTownNPCs) =>
            Main.LocalPlayer.GetModPlayer<HakariPlayer>().MisionActual >= 1;
    }
}