using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Novias.Systems
{
    public class NoviasWorld : ModSystem
    {
        public static int KaraneSiguiendo = -1;
        public static int HakariSiguiendo = -1;
        public static int ShizukaSiguiendo = -1;
        public static int NanoSiguiendo = -1;

        public override void SaveWorldData(TagCompound tag)
        {
            tag["KaraneSiguiendo"] = KaraneSiguiendo;
            tag["HakariSiguiendo"] = HakariSiguiendo;
            tag["ShizukaSiguiendo"] = ShizukaSiguiendo;
            tag["NanoSiguiendo"] = NanoSiguiendo;
        }

        public override void LoadWorldData(TagCompound tag)
        {
            KaraneSiguiendo = tag.GetInt("KaraneSiguiendo");
            HakariSiguiendo = tag.GetInt("HakariSiguiendo");
            ShizukaSiguiendo = tag.GetInt("ShizukaSiguiendo");
            NanoSiguiendo = tag.GetInt("NanoSiguiendo");
        }

        public override void OnWorldLoad()
        {
            KaraneSiguiendo = -1;
            HakariSiguiendo = -1;
            ShizukaSiguiendo = -1;
            NanoSiguiendo = -1;
        }
    }
}