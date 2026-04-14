using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Novias.Players
{
    public class NanoPlayer : ModPlayer
    {
        public bool EstaSiguiendo = false;
        public bool HacerAnimacion = false;

        public override void SaveData(TagCompound tag)
        {
            tag["EstaSiguiendo"] = EstaSiguiendo;
        }

        public override void LoadData(TagCompound tag)
        {
            EstaSiguiendo = tag.GetBool("EstaSiguiendo");
        }
    }
}