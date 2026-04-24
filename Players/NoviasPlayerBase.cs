using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Novias.Players
{
    public abstract class NoviasPlayerBase : ModPlayer
    {
        public int Fase = 0;
        public bool Mision1CompartidaCompletada = false;

        public int MisionActual
        {
            get
            {
                if (Fase >= 3) return -1;
                if (Fase == 1 && Mision1CompartidaCompletada) return 2;
                if (Fase == 2) return 3;
                return Fase;
            }
        }

        public bool EstaSiguiendo = false;
        public bool CompletacionPendiente = false;
        public bool UIAbierta = false;
        public int ContadorEnemigosMision2 = 0;

        protected string PrefixoGuardado { get; }
        protected NoviasPlayerBase(string prefixo) { PrefixoGuardado = prefixo; }

        public void CompletarMision() { if (Fase < 3) Fase++; }
        public bool MisionCompletada(int indice) => Fase > indice;

        public override void SaveData(TagCompound tag)
        {
            tag[$"{PrefixoGuardado}_Fase"] = Fase;
            tag[$"{PrefixoGuardado}_Siguiendo"] = EstaSiguiendo;
            tag[$"{PrefixoGuardado}_CompPendiente"] = CompletacionPendiente;
            tag[$"{PrefixoGuardado}_ContadorM2"] = ContadorEnemigosMision2;
            tag[$"{PrefixoGuardado}_M1Compartida"] = Mision1CompartidaCompletada;
        }

        public override void LoadData(TagCompound tag)
        {
            Fase = tag.GetInt($"{PrefixoGuardado}_Fase");
            EstaSiguiendo = tag.GetBool($"{PrefixoGuardado}_Siguiendo");
            CompletacionPendiente = tag.GetBool($"{PrefixoGuardado}_CompPendiente");
            ContadorEnemigosMision2 = tag.GetInt($"{PrefixoGuardado}_ContadorM2");
            Mision1CompartidaCompletada = tag.GetBool($"{PrefixoGuardado}_M1Compartida");
        }
    }
}