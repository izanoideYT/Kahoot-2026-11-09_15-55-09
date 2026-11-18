
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class GestorUsuarios
{
    public static Usuario UsuarioActual { get; private set; }

    public static List<Usuario> ObtenerUsuarios()
    {
        List<Usuario> usuarios = new List<Usuario>();

        if (!Directory.Exists(GestorArchivos.RutaUsuario))
        {
            Directory.CreateDirectory(GestorArchivos.RutaUsuario);
        }

        string[] archivos = Directory.GetFiles(
            GestorArchivos.RutaUsuario,
            "*.json"
        );

        foreach (string ruta in archivos)
        {
            try
            {
                string json = File.ReadAllText(ruta);
                Usuario usuario = JsonUtility.FromJson<Usuario>(json);

                if (usuario != null && !string.IsNullOrWhiteSpace(usuario.nombre))
                {
                    usuarios.Add(usuario);
                }
            }
            catch (Exception error)
            {
                GestorExcepciones.RegistrarError(
                    "Error al cargar un usuario: " + ruta,
                    error
                );
            }
        }

        return usuarios;
    }

    public static void GuardarUsuario(Usuario usuario)
    {
        if (usuario == null || string.IsNullOrWhiteSpace(usuario.nombre))
            return;

        string nombre = usuario.nombre.Trim();

        string[] archivos = Directory.GetFiles(
            GestorArchivos.RutaUsuario,
            "*.json"
        );

        string rutaUsuario = null;

        foreach (string ruta in archivos)
        {
            try
            {
                Usuario existente = JsonUtility.FromJson<Usuario>(
                    File.ReadAllText(ruta)
                );

                if (existente != null &&
                    string.Equals(
                        existente.nombre,
                        nombre,
                        StringComparison.OrdinalIgnoreCase))
                {
                    rutaUsuario = ruta;
                    break;
                }
            }
            catch
            {
                // Los archivos incorrectos se gestionan al cargar usuarios.
            }
        }



        if (rutaUsuario == null)
        {
            rutaUsuario = Path.Combine(
                GestorArchivos.RutaUsuario,
                "Usuario_" + Guid.NewGuid().ToString("N") + ".json"
            );
        }

        usuario.nombre = nombre;

        string json = JsonUtility.ToJson(usuario, true);
        File.WriteAllText(rutaUsuario, json);

        UsuarioActual = usuario;
    }

    public static void SeleccionarUsuario(Usuario usuario)
    {
        UsuarioActual = usuario;
    }

    public static Usuario CargarUsuario()
    {
        return UsuarioActual;
    }

    public static void EliminarUsuario(Usuario usuario)
    {
        if (usuario == null)
            return;

        string[] archivos = Directory.GetFiles(
            GestorArchivos.RutaUsuario,
            "*.json"
        );

        foreach (string ruta in archivos)
        {
            try
            {
                Usuario existente = JsonUtility.FromJson<Usuario>(
                    File.ReadAllText(ruta)
                );

                if (existente != null &&
                    existente.nombre == usuario.nombre)
                {
                    File.Delete(ruta);

                    if (UsuarioActual == usuario ||
                        (UsuarioActual != null &&
                         UsuarioActual.nombre == usuario.nombre))
                    {
                        UsuarioActual = null;
                    }

                    return;
                }
            }
            catch (System.Exception error)
            {
                GestorExcepciones.RegistrarError(
                    "Error al eliminar un usuario.",
                    error
                );
            }
        }
    }
}
