
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
}
