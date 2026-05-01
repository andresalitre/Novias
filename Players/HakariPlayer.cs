using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Novias.Players
{
    public class HakariPlayer : NoviasPlayerBase
    {
        public HakariPlayer() : base("Hakari") { }

        public bool Mision1CompartidaCompletada = false;
        public int ContadorEnemigosMision2 = 0;

        public override void SaveData(TagCompound tag)
        {
            base.SaveData(tag);
            tag["Hakari_M1Compartida"] = Mision1CompartidaCompletada;
            tag["Hakari_ContadorM2"] = ContadorEnemigosMision2;
        }

        public override void LoadData(TagCompound tag)
        {
            base.LoadData(tag);
            Mision1CompartidaCompletada = tag.GetBool("Hakari_M1Compartida");
            ContadorEnemigosMision2 = tag.GetInt("Hakari_ContadorM2");
        }
    }
}