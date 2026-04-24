namespace Novias.Systems
{
    public class MisionData
    {
        public string Clave { get; init; }
        public string TituloKey { get; init; }
        public string DescripcionKey { get; init; }
        public string MensajeBloqueadoKey { get; init; } = "";
        public string DialogoRecompensaKey { get; init; }

        public int ItemRequisito { get; init; }
        public int CantidadRequisito { get; init; } = 1;
        public int ItemRecompensa { get; init; }
        public int CantidadRecompensa { get; init; } = 1;

        public LineaDialogo[] DialogosPresentacion { get; init; } = System.Array.Empty<LineaDialogo>();
        public LineaDialogo[] DialogosCompletacion { get; init; } = System.Array.Empty<LineaDialogo>();

        public System.Func<bool> Condicion { get; init; } = null;
        public System.Func<bool> CondicionCompletar { get; init; } = null;
        public System.Func<bool> YaFueCompletada { get; init; } = null;
        public System.Func<string> ObtenerContador { get; init; } = null;
        public System.Action OnAceptar { get; init; } = null;
        public System.Action OnIniciarCompletacion { get; init; } = null;
        public System.Action OnCompletar { get; init; } = null;
        public System.Action OnMensajesCompletacion { get; init; } = null;

        public bool EstaDisponible() => Condicion == null || Condicion();
        public bool PuedeCompletar() => CondicionCompletar == null || CondicionCompletar();
    }
}