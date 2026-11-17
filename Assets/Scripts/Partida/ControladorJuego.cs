
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ControladorJuego : MonoBehaviour
{
    [Header("Preguntas")]
    public TMP_Text textoEnunciado;
    public TMP_Text textoNumeroPreguntas;

    [Header("Resultado de pregunta")]
    public GameObject panelResultadoPregunta;
    public TMP_Text textoRespuestaCorrecta;

    [Header("Respuestas")]
    public Button[] botonesRespuestas;

    [Header("Puntuacion")]
    public TMP_Text textoPuntuacion;

    [Header("Temporizador")]
    public TMP_Text textoTiempo;

    private Coroutine corrutinaTemporizador;
    private float tiempoRestante;

    private Kahoot kahootActual;
    private int preguntaActual = 0;
    private int puntuacion = 0;

    private int respuestasCorrectas = 0;
    private int respuestasIncorrectas = 0;

    private float tiempoTotal = 0f;
    private float tiempoInicio;

    private bool respuestaSeleccionada = false;

    private void Start()
    {
        kahootActual = GestorPartida.KahootActual;

        if (kahootActual == null || kahootActual.preguntas == null ||
            kahootActual.preguntas.Count == 0)
        {
            Debug.LogError("No hay un Kahoot válido para jugar.");
            return;
        }

        tiempoInicio = Time.time;

        textoPuntuacion.text = "Puntuación: " + puntuacion;

        panelResultadoPregunta.SetActive(false);

        for (int i = 0; i < botonesRespuestas.Length; i++)
        {
            int indice = i;

            botonesRespuestas[i].onClick.AddListener(
                () => SeleccionarRespuesta(indice)
            );
        }

        MostrarPregunta();
    }

    private void MostrarPregunta()
    {
        Pregunta pregunta = kahootActual.preguntas[preguntaActual];

        respuestaSeleccionada = false;

        textoEnunciado.text = pregunta.enunciado;

        textoNumeroPreguntas.text =
            "Pregunta " + (preguntaActual + 1) +
            " de " + kahootActual.preguntas.Count;

        for (int i = 0; i < botonesRespuestas.Length; i++)
        {
            if (i < pregunta.respuestas.Count)
            {
                TMP_Text textoRespuesta =
                    botonesRespuestas[i].GetComponentInChildren<TMP_Text>();

                textoRespuesta.text = pregunta.respuestas[i];
                botonesRespuestas[i].gameObject.SetActive(true);
                botonesRespuestas[i].interactable = true;
            }
            else
            {
                botonesRespuestas[i].gameObject.SetActive(false);
            }
        }

        tiempoRestante = pregunta.duracion;

        textoTiempo.text =
            "Tiempo: " + Mathf.CeilToInt(tiempoRestante);

        corrutinaTemporizador = StartCoroutine(Temporizador());
    }

    private void MostrarResultadoPregunta(string mensaje)
    {
        Pregunta pregunta = kahootActual.preguntas[preguntaActual];

        string respuestaCorrecta =
            pregunta.respuestas[pregunta.respuestaCorrecta];

        textoRespuestaCorrecta.text =
            mensaje + "\nRespuesta correcta: " + respuestaCorrecta;

        panelResultadoPregunta.SetActive(true);
    }

    private void SeleccionarRespuesta(int indice)
    {
        if (respuestaSeleccionada)
            return;

        respuestaSeleccionada = true;

        if (corrutinaTemporizador != null)
        {
            StopCoroutine(corrutinaTemporizador);
            corrutinaTemporizador = null;
        }

        Pregunta pregunta = kahootActual.preguntas[preguntaActual];

        if (indice == pregunta.respuestaCorrecta)
        {
            Debug.Log("¡Respuesta correcta!");

            puntuacion += 1000;
            respuestasCorrectas++;

            MostrarResultadoPregunta("¡Respuesta correcta!");
        }
        else
        {
            Debug.Log("Respuesta incorrecta.");

            respuestasIncorrectas++;

            MostrarResultadoPregunta("Respuesta incorrecta.");
        }

        textoPuntuacion.text = "Puntuación: " + puntuacion;

        foreach (Button boton in botonesRespuestas)
        {
            boton.interactable = false;
        }

        StartCoroutine(AvanzarPregunta());
    }

    private void TiempoAgotado()
    {
        if (respuestaSeleccionada)
            return;

        respuestaSeleccionada = true;
        corrutinaTemporizador = null;

        respuestasIncorrectas++;

        Debug.Log("¡Tiempo agotado!");

        MostrarResultadoPregunta("¡Tiempo agotado!");

        foreach (Button boton in botonesRespuestas)
        {
            boton.interactable = false;
        }

        StartCoroutine(AvanzarPregunta());
    }

    private System.Collections.IEnumerator AvanzarPregunta()
    {
        yield return new WaitForSeconds(2f);

        panelResultadoPregunta.SetActive(false);

        preguntaActual++;

        if (preguntaActual >= kahootActual.preguntas.Count)
        {
            FinalizarPartida();
            yield break;
        }

        MostrarPregunta();
    }

    private System.Collections.IEnumerator Temporizador()
    {
        while (tiempoRestante > 0)
        {
            tiempoRestante -= Time.deltaTime;

            textoTiempo.text =
                "Tiempo: " + Mathf.CeilToInt(tiempoRestante);

            yield return null;
        }

        tiempoRestante = 0;
        textoTiempo.text = "Tiempo: 0";

        TiempoAgotado();
    }

    private void FinalizarPartida()
    {
        tiempoTotal = Time.time - tiempoInicio;

        Resultado resultado = new Resultado();

        Usuario usuario = GestorUsuarios.CargarUsuario();

        resultado.nombreUsuario =
            usuario != null ? usuario.nombre : "Desconocido";

        resultado.puntuacion = puntuacion;
        resultado.respuestasCorrectas = respuestasCorrectas;
        resultado.respuestasIncorrectas = respuestasIncorrectas;
        resultado.tiempoTotal = tiempoTotal;

        GestorPartida.GuardarResultado(resultado);

        GestorLeaderboard.GuardarResultado(kahootActual.nombre, resultado);
        SceneManager.LoadScene("Jugar");
    }
}
