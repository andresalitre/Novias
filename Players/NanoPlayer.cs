using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Novias.Players
{
    public class NanoPlayer : ModPlayer
    {
        public bool Ayudada = false;
        public bool EstaSiguiendo = false;
        public bool HacerAnimacion = false;
        public bool EsperandoDialogo = false;

        public override void SaveData(TagCompound tag)
        {
            tag["Ayudada"] = Ayudada;
            tag["EstaSiguiendo"] = EstaSiguiendo;
            tag["EsperandoDialogo"] = EsperandoDialogo;
        }

        public override void LoadData(TagCompound tag)
        {
            Ayudada = tag.GetBool("Ayudada");
            EstaSiguiendo = tag.GetBool("EstaSiguiendo");
            EsperandoDialogo = tag.GetBool("EsperandoDialogo");
        }
    }
}