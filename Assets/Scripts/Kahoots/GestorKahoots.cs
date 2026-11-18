
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class GestorKahoots
{
    public static List<Kahoot> ObtenerKahoots()
    {
        List<Kahoot> kahoots = new List<Kahoot>();

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
