
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class GestorKahoots
{
    private static Dictionary<Kahoot, string> rutasKahoots =
        new Dictionary<Kahoot, string>();

    public static string RutaKahootEnEdicion { get; private set; }

    public static List<Kahoot> ObtenerKahoots()
    {
        List<Kahoot> kahoots = new List<Kahoot>();

        rutasKahoots.Clear();
        RutaKahootEnEdicion = null;

        // Cargar Kahoots incluidos en el proyecto.
        TextAsset[] archivosInternos = Resources.LoadAll<TextAsset>("Kahoots");

        foreach (TextAsset archivo in archivosInternos)
        {
            try
            {
                Kahoot kahoot = JsonUtility.FromJson<Kahoot>(archivo.text);

                if (kahoot == null || kahoot.preguntas == null)
                {
                    throw new Exception("El JSON no contiene un Kahoot válido.");
                }

                kahoots.Add(kahoot);
            }
            catch (Exception error)
            {
                GestorExcepciones.RegistrarError(
                    "Error al cargar el Kahoot interno: " + archivo.name,
                    error
                );
            }
        }

        // Cargar Kahoots creados por el usuario.
        try
        {
            string[] archivosExternos = Directory.GetFiles(
                GestorArchivos.RutaKahoots, "*.json"
            );

            foreach (string ruta in archivosExternos)
            {
                try
                {
                    string contenido = File.ReadAllText(ruta);

                    Kahoot kahoot = JsonUtility.FromJson<Kahoot>(contenido);

                    if (kahoot == null || kahoot.preguntas == null)
                    {
                        throw new Exception("El JSON no contiene un Kahoot válido.");
                    }

                    kahoots.Add(kahoot);
                    rutasKahoots[kahoot] = ruta;
                }
                catch (Exception error)
                {
                    GestorExcepciones.RegistrarError(
                        "Error al cargar el archivo: " + ruta,
                        error
                    );
                }
            }
        }
        catch (Exception error)
        {
            GestorExcepciones.RegistrarError(
                "Error al acceder a la carpeta de Kahoots.",
                error
            );
        }

        return kahoots;
    }


    public static bool ActualizarKahoot(Kahoot kahoot)
    {
        try
        {
            if (kahoot == null ||
                string.IsNullOrWhiteSpace(kahoot.nombre) ||
                kahoot.preguntas == null ||
                kahoot.preguntas.Count == 0 ||
                string.IsNullOrEmpty(RutaKahootEnEdicion))
            {
                Debug.LogWarning("No se puede actualizar el Kahoot.");
                return false;
            }

            string nombreArchivo = kahoot.nombre.Trim();

            foreach (char caracter in Path.GetInvalidFileNameChars())
            {
                nombreArchivo = nombreArchivo.Replace(caracter, '_');
            }

            string nuevaRuta = Path.Combine(
                GestorArchivos.RutaKahoots,
                nombreArchivo + ".json"
            );

            string rutaAnterior = RutaKahootEnEdicion;

            // Comprobar si el nuevo nombre pertenece a otro archivo.
            if (File.Exists(nuevaRuta) &&
                !string.Equals(rutaAnterior, nuevaRuta, StringComparison.OrdinalIgnoreCase))
            {
                Debug.LogWarning("Ya existe otro Kahoot con ese nombre.");
                return false;
            }

            string json = JsonUtility.ToJson(kahoot, true);

            File.WriteAllText(nuevaRuta, json);

            // Si se ha cambiado el nombre, eliminar el archivo anterior.
            if (!string.Equals(rutaAnterior, nuevaRuta, StringComparison.OrdinalIgnoreCase))
            {
                File.Delete(rutaAnterior);
            }

            RutaKahootEnEdicion = null;

            Debug.Log("Kahoot actualizado correctamente.");

            return true;
        }
        catch (Exception error)
        {
            GestorExcepciones.RegistrarError(
                "Error al actualizar el Kahoot: " + kahoot.nombre,
                error
            );

            return false;
        }
    }



    public static bool PrepararEdicion(Kahoot kahoot)
    {
        if (kahoot == null)
        {
            Debug.LogWarning("No se ha seleccionado ningún Kahoot.");
            return false;
        }

        // Si ya es un Kahoot editable, utilizar su ruta original.
        if (rutasKahoots.ContainsKey(kahoot))
        {
            RutaKahootEnEdicion = rutasKahoots[kahoot];
            return true;
        }

        // Si es un Kahoot interno, crear una copia editable.
        if (!GuardarCopiaKahoot(kahoot))
        {
            return false;
        }

        return true;
    }

    private static bool GuardarCopiaKahoot(Kahoot kahoot)
    {
        try
        {
            Directory.CreateDirectory(GestorArchivos.RutaKahoots);

            string nombreArchivo = kahoot.nombre.Trim();

            foreach (char caracter in Path.GetInvalidFileNameChars())
            {
                nombreArchivo = nombreArchivo.Replace(caracter, '_');
            }

            string ruta = Path.Combine(
                GestorArchivos.RutaKahoots,
                nombreArchivo + ".json"
            );

            // Evitar sobrescribir otro Kahoot del usuario.
            if (File.Exists(ruta))
            {
                Debug.LogWarning(
                    "Ya existe un Kahoot con ese nombre en tus archivos."
                );

                return false;
            }

            string json = JsonUtility.ToJson(kahoot, true);

            File.WriteAllText(ruta, json);

            RutaKahootEnEdicion = ruta;

            Debug.Log("Se ha creado una copia editable del Kahoot.");

            return true;
        }
        catch (Exception error)
        {
            GestorExcepciones.RegistrarError(
                "Error al crear una copia editable del Kahoot: " + kahoot.nombre,
                error
            );

            return false;
        }
    }

    public static bool EliminarKahoot(Kahoot kahoot)
    {
        try
        {
            if (kahoot == null || !rutasKahoots.ContainsKey(kahoot))
            {
                Debug.LogWarning(
                    "No se puede eliminar un Kahoot predeterminado."
                );

                return false;
            }

            string ruta = rutasKahoots[kahoot];

            if (File.Exists(ruta))
            {
                File.Delete(ruta);
            }

            rutasKahoots.Remove(kahoot);

            if (RutaKahootEnEdicion == ruta)
            {
                RutaKahootEnEdicion = null;
            }

            Debug.Log("Kahoot eliminado: " + kahoot.nombre);

            return true;
        }
        catch (Exception error)
        {
            GestorExcepciones.RegistrarError(
                "Error al eliminar el Kahoot: " + kahoot.nombre,
                error
            );

            return false;
        }
    }


    public static bool GuardarKahoot(Kahoot kahoot)
    {
        try
        {
            if (kahoot == null ||
                string.IsNullOrWhiteSpace(kahoot.nombre) ||
                kahoot.preguntas == null ||
                kahoot.preguntas.Count == 0)
            {
                Debug.LogWarning("El Kahoot no contiene datos válidos.");
                return false;
            }

            Directory.CreateDirectory(GestorArchivos.RutaKahoots);

            string nombreArchivo = kahoot.nombre.Trim();

            foreach (char caracter in Path.GetInvalidFileNameChars())
            {
                nombreArchivo = nombreArchivo.Replace(caracter, '_');
            }

            string ruta = Path.Combine(
                GestorArchivos.RutaKahoots,
                nombreArchivo + ".json"
            );

            if (File.Exists(ruta))
            {
                Debug.LogWarning("Ya existe un Kahoot con ese nombre.");
                return false;
            }

            string json = JsonUtility.ToJson(kahoot, true);

            File.WriteAllText(ruta, json);

            Debug.Log("Kahoot guardado correctamente: " + ruta);

            return true;
        }
        catch (Exception error)
        {
            GestorExcepciones.RegistrarError(
                "Error al guardar el Kahoot: " + kahoot.nombre,
                error
            );

            return false;
        }
    }
}
