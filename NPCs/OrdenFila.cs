using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace Novias.NPCs
{
    public class OrdenFila : ModSystem
    {
        public static readonly int MaxNoviasEnFila = 5;
        public static readonly float SeparacionEnFila = 48f;

        private static readonly Dictionary<int, int> indicesPorNPC = new();

        public static bool RegistrarEnFila(int npcWhoAmI)
        {
            if (indicesPorNPC.ContainsKey(npcWhoAmI))
                return true;

            if (indicesPorNPC.Count >= MaxNoviasEnFila)
                return false;

            indicesPorNPC[npcWhoAmI] = indicesPorNPC.Count;
            return true;
        }

        public static void RemoverDeFila(int npcWhoAmI)
        {
            if (!indicesPorNPC.ContainsKey(npcWhoAmI))
                return;

            int indiceRemovido = indicesPorNPC[npcWhoAmI];
            indicesPorNPC.Remove(npcWhoAmI);

            foreach (var clave in new List<int>(indicesPorNPC.Keys))
            {
                if (indicesPorNPC[clave] > indiceRemovido)
                    indicesPorNPC[clave]--;
            }
        }

        public static bool ObtenerIndice(int npcWhoAmI, out int indice)
        {
            return indicesPorNPC.TryGetValue(npcWhoAmI, out indice);
        }

        public static Microsoft.Xna.Framework.Vector2 ObtenerPosicionEnFila(int indice, float npcCenterY)
        {
            Player player = Main.LocalPlayer;
            float offsetX = -(indice + 1) * SeparacionEnFila;
            return new Microsoft.Xna.Framework.Vector2(player.Center.X + offsetX, npcCenterY);
        }

        public override void OnWorldUnload()
        {
            indicesPorNPC.Clear();
        }
    }
}