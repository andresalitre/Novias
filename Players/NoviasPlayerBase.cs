using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Novias.Players
{

    // fase actual (0-3), misión activa (0-2), seguimiento.

    //  Fases:
    //   0 — NPC recién llegada          → [Tienda] [Misión]
    //   1 — Misión 1 completada         → [Tienda] [Seguir] [Misión]
    //   2 — Misión 2 completada         → [Tienda] [Seguir] [Besar] [Misión]
    //   3 — Misión 3 completada         → [Tienda] [Seguir] [Besar] [Habilidad]
    public abstract class NoviasPlayerBase : ModPlayer
    {

        public int Fase = 0;

        // Índice de la misión actualmente activa (0, 1, 2).
        // -1 significa que ya no hay misiones pendientes (fase 3).
        public int MisionActual => Fase < 3 ? Fase : -1;

        // ¿Está esta NPC siguiendo al jugador?
        public bool EstaSiguiendo = false;



        public bool UIAbierta = false;

        /// Avanza a la siguiente fase. Máximo 3.
        public void CompletarMision()
        {
            if (Fase < 3) Fase++;
        }

        public bool MisionCompletada(int indiceMision) => Fase > indiceMision;


        protected string PrefixoGuardado { get; }

        protected NoviasPlayerBase(string prefixo)
        {
            PrefixoGuardado = prefixo;
        }

        public override void SaveData(TagCompound tag)
        {
            tag[$"{PrefixoGuardado}_Fase"] = Fase;
            tag[$"{PrefixoGuardado}_Siguiendo"] = EstaSiguiendo;
        }

        public override void LoadData(TagCompound tag)
        {
            Fase = tag.GetInt($"{PrefixoGuardado}_Fase");
            EstaSiguiendo = tag.GetBool($"{PrefixoGuardado}_Siguiendo");
        }
    }
}
