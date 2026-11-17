
using System.IO;
using UnityEngine;

public static class GestorUsuarios
{
    private static string RutaUsuario =>
        Path.Combine(GestorArchivos.RutaUsuario, "Usuario.json");

    public static void GuardarUsuario(Usuario usuario)
    {
        string json = JsonUtility.ToJson(usuario, true);

        File.WriteAllText(RutaUsuario, json);
    }

    public static Usuario CargarUsuario()
    {
        if (!File.Exists(RutaUsuario))
        {
            return null;
        }

        string json = File.ReadAllText(RutaUsuario);

        return JsonUtility.FromJson<Usuario>(json);
    }
}
