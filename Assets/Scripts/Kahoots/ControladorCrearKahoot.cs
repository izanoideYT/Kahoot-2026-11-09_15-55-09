
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    private void Start()
    {
        botonAnadirPregunta.onClick.AddListener(AnadirPregunta);
        botonGuardarKahoot.onClick.AddListener(GuardarKahoot);

        AnadirPregunta();
    }

    public void AnadirPregunta()
    {
        if (prefabPregunta == null || contenidoPreguntas == null)
        {
            Debug.LogError("Faltan referencias del prefab o del Content.");
            return;
        }

        Instantiate(prefabPregunta, contenidoPreguntas);
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

        bool guardado = GestorKahoots.GuardarKahoot(nuevoKahoot);

        if (guardado)
        {
            Debug.Log("Kahoot guardado correctamente.");
            UnityEngine.SceneManagement.SceneManager.LoadScene("Selecionar Kahoot");
        }
    }
}
