using UnityEngine;
using System.IO;

public static class GestorArchivos{

    public static string RutaBase => Application.persistentDataPath;

    public static string RutaUsuario => Path.Combine(RutaBase, "Usuarios");
    public static string RutaKahoots => Path.Combine(RutaBase, "Kahoots");
    public static string RutaLeaderboards => Path.Combine(RutaBase, "Leaderboards");
    public static string RutaErrorLogs => Path.Combine(RutaBase, "ErrorLogs");

    public static void inicializar()
    {
        Directory.CreateDirectory(RutaUsuario);
        Directory.CreateDirectory(RutaKahoots);
        Directory.CreateDirectory(RutaLeaderboards);
        Directory.CreateDirectory(RutaErrorLogs);


    }


}
