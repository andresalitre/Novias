using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Novias.Players
{
    public abstract class NoviasPlayerBase : ModPlayer
    {
        public int Fase = 0;
        public int MisionActual = 0;
        public bool EstaSiguiendo = false;
        public bool CompletacionPendiente = false;
        public bool UIAbierta = false;

        protected string PrefixoGuardado { get; }
        protected NoviasPlayerBase(string prefixo) { PrefixoGuardado = prefixo; }

        public void CompletarMision()
        {
            if (Fase < 3) Fase++;
            MisionActual++;
        }

        public void AvanzarMisionSinFase()
        {
            MisionActual++;
        }

        public bool MisionCompletada(int indice) => MisionActual > indice;

        public override void SaveData(TagCompound tag)
        {
            tag[$"{PrefixoGuardado}_Fase"] = Fase;
            tag[$"{PrefixoGuardado}_MisionActual"] = MisionActual;
            tag[$"{PrefixoGuardado}_Siguiendo"] = EstaSiguiendo;
            tag[$"{PrefixoGuardado}_CompPendiente"] = CompletacionPendiente;
        }

        public override void LoadData(TagCompound tag)
        {
            Fase = tag.GetInt($"{PrefixoGuardado}_Fase");
            MisionActual = tag.GetInt($"{PrefixoGuardado}_MisionActual");
            EstaSiguiendo = tag.GetBool($"{PrefixoGuardado}_Siguiendo");
            CompletacionPendiente = tag.GetBool($"{PrefixoGuardado}_CompPendiente");
        }
    }
}