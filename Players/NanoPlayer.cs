using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Novias.Players
{
    public class NanoPlayer : ModPlayer
    {
        public bool LeDioRegalo = false;
        public bool EstaSiguiendo = false;

        public override void SaveData(TagCompound tag)
        {
            tag["LeDioRegalo"] = LeDioRegalo;
            tag["EstaSiguiendo"] = EstaSiguiendo;
        }

        public override void LoadData(TagCompound tag)
        {
            LeDioRegalo = tag.GetBool("LeDioRegalo");
            EstaSiguiendo = tag.GetBool("EstaSiguiendo");
        }
    }
}