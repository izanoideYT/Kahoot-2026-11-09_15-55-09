
public static class GestorPartida
{
    public static Kahoot KahootActual { get; private set; }

    public static Resultado ResultadoActual { get; private set; }

    public static void SeleccionarKahoot(Kahoot kahoot)
    {
        KahootActual = kahoot;
    }

    public static void GuardarResultado(Resultado resultado)
    {
        ResultadoActual = resultado;
    }

    public static void LimpiarPartida()
    {
        KahootActual = null;
        ResultadoActual = null;
    }
}
