
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ControladorCrearKahoot : MonoBehaviour
{
    [Header("Datos del Kahoot")]
    public TMP_InputField inputNombreKahoot;
    public TMP_InputField inputDuracionPregunta;

    [Header("Preguntas")]
    public GameObject prefabPregunta;
    public Transform contenidoPreguntas;

    [Header("Botones")]
    public Button botonAnadirPregunta;
    public Button botonGuardarKahoot;
    public Button botonVolver;

    [Header("Confirmacion de salida")]
    public GameObject panelConfirmarSalida;
    public Button botonSeguirEditando;
    public Button botonSalirSinGuardar;

    private Kahoot kahootEnEdicion;

    private void Start()
    {
        botonAnadirPregunta.onClick.AddListener(AnadirPregunta);
        botonGuardarKahoot.onClick.AddListener(GuardarKahoot);

        botonVolver.onClick.AddListener(Volver);
        botonSeguirEditando.onClick.AddListener(CerrarConfirmacion);
        botonSalirSinGuardar.onClick.AddListener(SalirSinGuardar);

        panelConfirmarSalida.SetActive(false);

        if (!string.IsNullOrEmpty(GestorKahoots.RutaKahootEnEdicion))
        {
            CargarKahootExistente();
        }
        else
        {
            for (int i = 0; i < 10; i++)
            {
                AnadirPregunta();
            }
        }
    }

    private void CargarKahootExistente()
    {
        string ruta = GestorKahoots.RutaKahootEnEdicion;

        if (!File.Exists(ruta))
        {
            Debug.LogError("No se encuentra el archivo del Kahoot.");
            return;
        }

        string contenido = File.ReadAllText(ruta);

        kahootEnEdicion = JsonUtility.FromJson<Kahoot>(contenido);

        if (kahootEnEdicion == null ||
            kahootEnEdicion.preguntas == null ||
            kahootEnEdicion.preguntas.Count == 0)
        {
            Debug.LogError("El Kahoot no contiene datos válidos.");
            return;
        }

        inputNombreKahoot.text = kahootEnEdicion.nombre;

        inputDuracionPregunta.text =
            kahootEnEdicion.preguntas[0].duracion.ToString();

        foreach (Pregunta pregunta in kahootEnEdicion.preguntas)
        {
            GameObject nuevaPregunta = CrearTarjetaPregunta();

            ControladorPreguntaUI controlador =
                nuevaPregunta.GetComponent<ControladorPreguntaUI>();

            controlador.CargarPregunta(pregunta);
        }
    }

    public void AnadirPregunta()
    {
        if (prefabPregunta == null || contenidoPreguntas == null)
        {
            Debug.LogError("Faltan referencias del prefab o del Content.");
            return;
        }

        CrearTarjetaPregunta();
    }

    private GameObject CrearTarjetaPregunta()
    {
        GameObject nuevaPregunta = Instantiate(
            prefabPregunta,
            contenidoPreguntas
        );

        ControladorPreguntaUI controlador =
            nuevaPregunta.GetComponent<ControladorPreguntaUI>();

        if (controlador == null)
        {
            Debug.LogError("El prefab no tiene ControladorPreguntaUI.");
            return nuevaPregunta;
        }

        controlador.ConfigurarBotonEliminar(EliminarPregunta);

        return nuevaPregunta;
    }

    private void EliminarPregunta(ControladorPreguntaUI pregunta)
    {
        Destroy(pregunta.gameObject);
    }

    private void GuardarKahoot()
    {
        string nombre = inputNombreKahoot.text.Trim();

        if (string.IsNullOrWhiteSpace(nombre))
        {
            Debug.LogWarning("Introduce un nombre para el Kahoot.");
            return;
        }

        if (!float.TryParse(
            inputDuracionPregunta.text,
            out float duracion
        ) || duracion <= 0)
        {
            Debug.LogWarning("Introduce una duración válida.");
            return;
        }

        List<Pregunta> preguntas = new List<Pregunta>();

        foreach (Transform hijo in contenidoPreguntas)
        {
            ControladorPreguntaUI controlador =
                hijo.GetComponent<ControladorPreguntaUI>();

            if (controlador == null)
                continue;

            if (!controlador.EsValida())
            {
                Debug.LogWarning("Hay una pregunta con campos vacíos.");
                return;
            }

            preguntas.Add(controlador.ObtenerPregunta(duracion));
        }

        if (preguntas.Count == 0)
        {
            Debug.LogWarning("El Kahoot necesita al menos una pregunta.");
            return;
        }

        Kahoot nuevoKahoot = new Kahoot();

        nuevoKahoot.nombre = nombre;
        nuevoKahoot.preguntas = preguntas;

        bool guardado;

        if (!string.IsNullOrEmpty(GestorKahoots.RutaKahootEnEdicion))
        {
            guardado = GestorKahoots.ActualizarKahoot(nuevoKahoot);
        }
        else
        {
            guardado = GestorKahoots.GuardarKahoot(nuevoKahoot);
        }

        if (guardado)
        {
            Debug.Log("Kahoot guardado correctamente.");

            SceneManager.LoadScene("Selecionar Kahoot");
        }
    }

    public void Volver()
    {
        panelConfirmarSalida.SetActive(true);
    }

    private void CerrarConfirmacion()
    {
        panelConfirmarSalida.SetActive(false);
    }

    private void SalirSinGuardar()
    {
        SceneManager.LoadScene("Selecionar Kahoot");
    }
}
