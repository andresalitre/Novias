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
using Novias.Items.Potions;
using Novias.Items.Weapons.Ranged;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using ReLogic.Content;

namespace Novias.NPCs.Novias
{
    [AutoloadHead]
    public class KusuriYakuzen : ComportamientoNovia
    {
        protected override bool EstaSiguiendo => Main.LocalPlayer.GetModPlayer<KusuriPlayer>().EstaSiguiendo;
        protected override Color ColorPolvo => new Color(255, 0, 0);
        protected override int CooldownAtaque => 45;
        protected override int EfectoNovia => ModContent.ProjectileType<Corazon>();
        protected override int RegeneracionVida => 8;

        protected override bool EstaHablandoConInterfaz =>
            InterfazNovias.InterfazAbierta<KusuriInterfaz>();

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
            NPC.defense = 75;
            NPC.knockBackResist = 0.8f;
            NPC.HitSound = SoundID.NPCHit18;
            NPC.DeathSound = SoundID.NPCDeath20;
            NPC.townNPC = true;
            NPC.friendly = true;
        }

        public override void AddShops()
        {
            var tienda = new NPCShop(Type, "Shop");
            tienda.Add(ModContent.ItemType<Cutter>());
            tienda.Add(ModContent.ItemType<PocionDeEficiencia>(), new Condition("Mods.Novias.Condiciones.MisionCompletada", () => Main.LocalPlayer.GetModPlayer<KusuriPlayer>().MisionActual >= 2));
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
            KusuriInterfaz.Instance._state?.IniciarConNPC(NPC);
            return " ";
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheHallow,
            });
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            var p = Main.LocalPlayer.GetModPlayer<KusuriPlayer>();
            if (!p.EsMayor) return true;

            var textura = ModContent.Request<Texture2D>("Novias/NPCs/Novias/KusuriYakuzen_Mayor").Value;
            int frameH = textura.Height / Main.npcFrameCount[NPC.type];
            int frameIndex = NPC.frame.Y / (TextureAssets.Npc[NPC.type].Value.Height / Main.npcFrameCount[NPC.type]);
            Rectangle frame = new Rectangle(0, frameIndex * frameH, textura.Width, frameH);

            SpriteEffects efecto = NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            Vector2 pos = NPC.Center - screenPos + new Vector2(0f, NPC.gfxOffY + NPC.height - 15f);
            Vector2 origen = new Vector2(textura.Width / 2f, frameH);
            spriteBatch.Draw(textura, pos, frame, drawColor, NPC.rotation, origen, NPC.scale, efecto, 0f);
            return false;
        }

        public override bool CanTownNPCSpawn(int numTownNPCs) =>
            Main.LocalPlayer.GetModPlayer<ShizukaPlayer>().MisionActual >= 2;
    }
}