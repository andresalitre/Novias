using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Novias.Players
{
    public class KaranePlayer : NoviasPlayerBase
    {
        public KaranePlayer() : base("Karane") { }

        public bool Mision1CompartidaCompletada = false;
        public int ContadorEnemigosMision2 = 0;

        public override void SaveData(TagCompound tag)
        {
            base.SaveData(tag);
            tag["Karane_M1Compartida"] = Mision1CompartidaCompletada;
            tag["Karane_ContadorM2"] = ContadorEnemigosMision2;
        }

        public override void LoadData(TagCompound tag)
        {
            base.LoadData(tag);
            Mision1CompartidaCompletada = tag.GetBool("Karane_M1Compartida");
            ContadorEnemigosMision2 = tag.GetInt("Karane_ContadorM2");
        }
    }
}