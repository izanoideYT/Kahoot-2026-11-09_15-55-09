
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using UnityEngine;

public static class GestorLeaderboard
{
    private static string ObtenerRuta(string nombreKahoot)
    {
        string nombreArchivo = nombreKahoot + ".xml";

        foreach (char caracter in Path.GetInvalidFileNameChars())
        {
            nombreArchivo = nombreArchivo.Replace(caracter, '_');
        }

        return Path.Combine(
            GestorArchivos.RutaLeaderboards,
            nombreArchivo
        );
    }

    public static LeaderboardData CargarLeaderboard(string nombreKahoot)
    {
        string ruta = ObtenerRuta(nombreKahoot);

        if (!File.Exists(ruta))
        {
            return new LeaderboardData
            {
                nombreKahoot = nombreKahoot
            };
        }

        try
        {
            XmlSerializer serializador =
                new XmlSerializer(typeof(LeaderboardData));

            using (FileStream archivo = new FileStream(ruta, FileMode.Open))
            {
                LeaderboardData datos =
                    (LeaderboardData)serializador.Deserialize(archivo);

                if (datos == null || datos.entradas == null)
                {
                    throw new Exception("El XML no contiene una clasificación válida.");
                }

                return datos;
            }
        }
        catch (Exception error)
        {
            GestorExcepciones.RegistrarError(
                "Error al cargar el Leaderboard de " + nombreKahoot,
                error
            );

            return new LeaderboardData
            {
                nombreKahoot = nombreKahoot
            };
        }
    }

    public static void GuardarResultado(string nombreKahoot, Resultado resultado)
    {
        try
        {
            LeaderboardData datos = CargarLeaderboard(nombreKahoot);

            EntradaLeaderboard entradaExistente =
                datos.entradas.Find(e => e.nombreUsuario == resultado.nombreUsuario);

            if (entradaExistente == null)
            {
                EntradaLeaderboard nuevaEntrada = new EntradaLeaderboard();

                nuevaEntrada.nombreUsuario = resultado.nombreUsuario;
                nuevaEntrada.puntuacion = resultado.puntuacion;
                nuevaEntrada.tiempoTotal = resultado.tiempoTotal;

                datos.entradas.Add(nuevaEntrada);
            }
            else
            {
                bool mejorPuntuacion =
                    resultado.puntuacion > entradaExistente.puntuacion;

                bool mismoPuntuacionMejorTiempo =
                    resultado.puntuacion == entradaExistente.puntuacion &&
                    resultado.tiempoTotal < entradaExistente.tiempoTotal;

                if (mejorPuntuacion || mismoPuntuacionMejorTiempo)
                {
                    entradaExistente.puntuacion = resultado.puntuacion;
                    entradaExistente.tiempoTotal = resultado.tiempoTotal;
                }
            }

            datos.entradas = datos.entradas
                .OrderByDescending(e => e.puntuacion)
                .ThenBy(e => e.tiempoTotal)
                .ToList();

            XmlSerializer serializador =
                new XmlSerializer(typeof(LeaderboardData));

            string ruta = ObtenerRuta(nombreKahoot);

            using (FileStream archivo = new FileStream(ruta, FileMode.Create))
            {
                serializador.Serialize(archivo, datos);
            }
        }
        catch (Exception error)
        {
            GestorExcepciones.RegistrarError(
                "Error al guardar el Leaderboard de " + nombreKahoot,
                error
            );
        }
    }
}
