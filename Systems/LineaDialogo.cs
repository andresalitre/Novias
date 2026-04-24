using Microsoft.Xna.Framework;

namespace Novias.Systems
{
    public class LineaDialogo
    {
        public bool EsJugador { get; init; }
        public string Key { get; init; }
        public string NombreNPC { get; init; } = "";
        public Color? ColorNombre { get; init; } = null;
    }
}