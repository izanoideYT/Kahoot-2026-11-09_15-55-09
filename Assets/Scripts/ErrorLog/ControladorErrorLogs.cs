
using System;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class ControladorErrorLogs : MonoBehaviour
{
    [Header("Lista de archivos")]
    [SerializeField] private Transform content;
    [SerializeField] private Button prefabBotonArchivo;

    [Header("Panel de detalle")]
    [SerializeField] private TMP_Text textoDetalle;

    [Header("Panel sin errores")]
    [SerializeField] private GameObject panelSinErrores;
    [SerializeField] private TMP_Text textoSinErrores;

    [Header("Paneles principales")]
    [SerializeField] private GameObject panelLista;
    [SerializeField] private GameObject panelDetalle;

    [Header("Botones")]
    [SerializeField] private Button botonVolver;

    private string rutaErrorLogs;

    private void Start()
    {
        rutaErrorLogs = GestorArchivos.RutaErrorLogs;

        botonVolver.onClick.AddListener(Volver);

        CargarArchivos();
    }

    private void CargarArchivos()
    {
        // Limpiar botones anteriores.
        foreach (Transform hijo in content)
        {
            Destroy(hijo.gameObject);
        }

        if (!Directory.Exists(rutaErrorLogs))
        {
            MostrarSinErrores();
            return;
        }

        try
        {
            string[] archivos = Directory.GetFiles(
                rutaErrorLogs,
                "*.txt"
            );

            // Ordenar los archivos del más reciente al más antiguo.
            archivos = archivos
                .OrderByDescending(File.GetLastWriteTime)
                .ToArray();

            if (archivos.Length == 0)
            {
                MostrarSinErrores();
                return;
            }

            panelSinErrores.SetActive(false);
            panelLista.SetActive(true);
            panelDetalle.SetActive(true);

            textoDetalle.text =
                "Selecciona un registro para consultar su contenido.";

            foreach (string rutaArchivo in archivos)
            {
                CrearBotonArchivo(rutaArchivo);
            }
        }
        catch (Exception e)
        {
            Debug.LogError(
                "Error al cargar los registros: " + e.Message
            );

            MostrarSinErrores();
        }
    }

    private void CrearBotonArchivo(string rutaArchivo)
    {
        Button nuevoBoton = Instantiate(
            prefabBotonArchivo,
            content
        );

        string nombreArchivo = Path.GetFileName(rutaArchivo);

        TMP_Text textoBoton =
            nuevoBoton.GetComponentInChildren<TMP_Text>();

        if (textoBoton != null)
        {
            textoBoton.text = nombreArchivo;
        }

        nuevoBoton.onClick.AddListener(() =>
        {
            MostrarArchivo(rutaArchivo);
        });
    }

    private void MostrarArchivo(string rutaArchivo)
    {
        try
        {
            string contenido = File.ReadAllText(rutaArchivo);

            textoDetalle.text = contenido;
        }
        catch (Exception e)
        {
            textoDetalle.text =
                "No se ha podido leer el archivo.\n\n" +
                e.Message;

            Debug.LogError(
                "Error al leer un registro: " + e.Message
            );
        }
    }

    private void MostrarSinErrores()
    {
        panelLista.SetActive(false);
        panelDetalle.SetActive(false);
        panelSinErrores.SetActive(true);

        textoSinErrores.text =
            "No se han encontrado registros de errores.";
    }

    public void Volver()
    {
        SceneManager.LoadScene("Main Scene");
    }
}
