
public static class GestorNavegacion
{
    public static string EscenaAnteriorLeaderboard { get; private set; }

    public static void EstablecerOrigenLeaderboard(string escena)
    {
        EscenaAnteriorLeaderboard = escena;
    }
}
