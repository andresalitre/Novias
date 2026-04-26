using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Novias.Players
{
    public abstract class NoviasPlayerBase : ModPlayer
    {
        public int Fase = 0;
        public bool EstaSiguiendo = false;
        public bool CompletacionPendiente = false;
        public bool UIAbierta = false;

        protected virtual bool MisionIntermediaCompletada => false;

        public int MisionActual
        {
            get
            {
                if (Fase >= 3) return -1;
                if (Fase == 1 && MisionIntermediaCompletada) return 2;
                if (Fase == 2) return 3;
                return Fase;
            }
        }

        protected string PrefixoGuardado { get; }
        protected NoviasPlayerBase(string prefixo) { PrefixoGuardado = prefixo; }

        public void CompletarMision() { if (Fase < 3) Fase++; }
        public bool MisionCompletada(int indice) => Fase > indice;

        public override void SaveData(TagCompound tag)
        {
            tag[$"{PrefixoGuardado}_Fase"] = Fase;
            tag[$"{PrefixoGuardado}_Siguiendo"] = EstaSiguiendo;
            tag[$"{PrefixoGuardado}_CompPendiente"] = CompletacionPendiente;
        }

        public override void LoadData(TagCompound tag)
        {
            Fase = tag.GetInt($"{PrefixoGuardado}_Fase");
            EstaSiguiendo = tag.GetBool($"{PrefixoGuardado}_Siguiendo");
            CompletacionPendiente = tag.GetBool($"{PrefixoGuardado}_CompPendiente");
        }
    }
}