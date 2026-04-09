using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Bestiary;
using Terraria.Localization;
using Microsoft.Xna.Framework;
using Novias.Players;
using Novias.Projectiles;
using Novias.Effects;
using Novias.NPCs.Novias;

namespace Novias.NPCs
{
    [AutoloadHead]
    public class NanoAsustada : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 1;
            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Velocity = 0f
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
        }

        public override void SetDefaults()
        {
            NPC.width = 20;
            NPC.height = 38;
            NPC.aiStyle = 0; 
            NPC.lifeMax = 2500;
            NPC.defense = 65;
            NPC.knockBackResist = 0f; 
            NPC.HitSound = SoundID.NPCHit18;
            NPC.DeathSound = SoundID.NPCDeath20;
            NPC.townNPC = true;
            NPC.friendly = true;
            NPC.immortal = true; 
        }

        public override void AI()
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
        }

        public override bool CheckConditions(int left, int top, int right, int bottom)
        {
            return false;
        }

        public override void SetChatButtons(ref string button, ref string button2)
        {
            button = Language.GetTextValue("Mods.Novias.NPCDialogue.NanoAsustada.BotonAyudar");
            button2 = "";
        }

        public override void OnChatButtonClicked(bool firstButton, ref string shop)
        {
            Player jugador = Main.LocalPlayer;
            NanoPlayer modPlayer = jugador.GetModPlayer<NanoPlayer>();

            if (!firstButton) return;

            if (jugador.HasItem(ItemID.RecallPotion))
            {
                jugador.ConsumeItem(ItemID.RecallPotion);
                modPlayer.Ayudada = true;
                modPlayer.HacerAnimacion = true;

                Vector2 spawnPos;
                if (jugador.SpawnX > 0 && jugador.SpawnY > 0)
                {
                    spawnPos = new Vector2(
                        jugador.SpawnX * 16f,
                        (jugador.SpawnY - 3) * 16f
                    );
                }
                else
                {
                    spawnPos = new Vector2(
                        Main.spawnTileX * 16f,
                        (Main.spawnTileY - 3) * 16f
                    );
                }

                jugador.Teleport(spawnPos, 1);

                NPC.NewNPC(
                    NPC.GetSource_FromThis(),
                    (int)spawnPos.X + 40,
                    (int)spawnPos.Y,
                    ModContent.NPCType<NanoEiai>()
                );

                NPC.active = false;
                Main.npcChatText = Language.GetTextValue("Mods.Novias.NPCDialogue.NanoAsustada.Ayuda");
            }
            else
            {
                Main.npcChatText = Language.GetTextValue("Mods.Novias.NPCDialogue.NanoAsustada.SinPocion");
            }
        }

        public override string GetChat()
        {
            return Language.GetTextValue($"Mods.Novias.NPCDialogue.NanoAsustada.PreCondicion{Main.rand.Next(3)}");
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
            return false;
        }
    }
}