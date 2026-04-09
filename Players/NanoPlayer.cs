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
        public int TimerBloqueo = 0;

        public override void PreUpdate()
        {
            if (TimerBloqueo > 0)
            {
                TimerBloqueo--;
                Player.controlLeft = false;
                Player.controlRight = false;
                Player.controlJump = false;
                Player.controlDown = false;
                Player.controlUseItem = false;
            }
        }

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