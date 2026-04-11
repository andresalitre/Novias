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
        public static bool NanoAyudada = false;
        public static bool NanoEsperandoDialogo = false;

        public override void SaveWorldData(TagCompound tag)
        {
            tag["KaraneSiguiendo"] = KaraneSiguiendo;
            tag["HakariSiguiendo"] = HakariSiguiendo;
            tag["ShizukaSiguiendo"] = ShizukaSiguiendo;
            tag["NanoSiguiendo"] = NanoSiguiendo;
            tag["NanoAyudada"] = NanoAyudada;
            tag["NanoEsperandoDialogo"] = NanoEsperandoDialogo;
        }

        public override void LoadWorldData(TagCompound tag)
        {
            KaraneSiguiendo = tag.GetInt("KaraneSiguiendo");
            HakariSiguiendo = tag.GetInt("HakariSiguiendo");
            ShizukaSiguiendo = tag.GetInt("ShizukaSiguiendo");
            NanoSiguiendo = tag.GetInt("NanoSiguiendo");
            NanoAyudada = tag.GetBool("NanoAyudada");
            NanoEsperandoDialogo = tag.GetBool("NanoEsperandoDialogo");
        }
    }
}