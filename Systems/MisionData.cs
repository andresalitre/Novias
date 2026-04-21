using Terraria.ModLoader;

namespace Novias.Systems
{
    public class LineaDialogo
    {
        public bool EsJugador { get; init; }
        public string Key { get; init; }
    }

    public class MisionData
    {
        public string Clave { get; init; }
        public string TituloKey { get; init; }
        public string DescripcionKey { get; init; }
        public string[] DialogosKey { get; init; } = System.Array.Empty<string>();
        public int ItemRequisito { get; init; }
        public int CantidadRequisito { get; init; } = 1;
        public int ItemRecompensa { get; init; }
        public int CantidadRecompensa { get; init; } = 1;
        public string DialogoRecompensaKey { get; init; }

        public LineaDialogo[] DialogosCompletacion { get; init; } = System.Array.Empty<LineaDialogo>();
    }
}