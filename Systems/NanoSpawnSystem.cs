using Terraria;
using Terraria.ModLoader;
using Novias.NPCs.Novias;
using Novias.NPCs.NanoSystem;
using Novias.Systems;
using Novias.NPCs;

namespace Novias.Systems
{
    public class NanoSpawnSystem : ModSystem
    {
        private int timer = 0;

        public override void PostUpdateNPCs()
        {
            if (NoviasWorld.NanoAyudada)
                return;

            timer++;
            if (timer < 300) return;
            timer = 0;

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                if (Main.npc[i].active && Main.npc[i].ModNPC is NanoAsustada)
                    return;
            }

            int noviasPresentes = 0;
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (npc.active && npc.ModNPC is ComportamientoNovia)
                    noviasPresentes++;
            }

            if (noviasPresentes < 2)
                return;

            int spawnX = 0;
            int spawnY = 0;
            bool encontrado = false;

            int alturaIsla = (int)(Main.worldSurface * 0.3f);

            for (int x = 100; x < Main.maxTilesX - 100; x++)
            {
                for (int y = 50; y < alturaIsla; y++)
                {
                    Tile tile = Main.tile[x, y];
                    if (tile.HasTile && (
                        tile.TileType == Terraria.ID.TileID.Cloud ||
                        tile.TileType == Terraria.ID.TileID.RainCloud))
                    {
                        spawnX = x;
                        spawnY = y - 3;
                        encontrado = true;
                        break;
                    }
                }
                if (encontrado) break;
            }

            if (!encontrado)
                return;

            NPC.NewNPC(
                NPC.GetSource_NaturalSpawn(),
                spawnX * 16,
                spawnY * 16,
                ModContent.NPCType<NanoAsustada>()
            );
        }
    }
}