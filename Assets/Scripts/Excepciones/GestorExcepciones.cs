using UnityEngine;
using System;
using System.IO;

public static class GestorExcepciones
{
    public static void RegistrarError(string descripcion, Exception exception)
    {
        try
        {
            string fecha = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            string nombreArchivo = "Error_" + fecha + ".txt";

            string ruta = Path.Combine(GestorArchivos.RutaErrorLogs, nombreArchivo);

            string contenido = "FECHA: " + DateTime.Now + "\n" + "DESCRIPCION: " + descripcion + "\n" + "ERROR: " + exception.Message + "\n" + "DETALLES:\n" + exception.StackTrace;

            File.WriteAllText(ruta, contenido);

            Debug.LogError("Informe de error generado en + " + ruta);


        } 
        catch (Exception error)
        {
            Debug.LogError("No se puede generar el informe: " + error.Message);
        }

    }
}
