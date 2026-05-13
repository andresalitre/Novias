using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Novias.Players
{
    public class KusuriPlayer : NoviasPlayerBase
    {
        public KusuriPlayer() : base("Kusuri") { }

        public bool EsMayor = false;

        public override void SaveData(TagCompound tag)
        {
            base.SaveData(tag);
            tag["Kusuri_EsMayor"] = EsMayor;
        }

        public override void LoadData(TagCompound tag)
        {
            base.LoadData(tag);
            EsMayor = tag.GetBool("Kusuri_EsMayor");
        }
    }
}