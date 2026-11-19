
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class ControladorPreguntaUI : MonoBehaviour
{
    [Header("Enunciado")]
    public TMP_InputField inputEnunciado;

    [Header("Respuestas")]
    public TMP_InputField inputRespuesta1;
    public TMP_InputField inputRespuesta2;
    public TMP_InputField inputRespuesta3;
    public TMP_InputField inputRespuesta4;

    [Header("Respuesta correcta")]
    public TMP_Dropdown dropdownRespuestaCorrecta;

    [Header("Eliminar")]
    public Button botonEliminar;

    public void ConfigurarBotonEliminar(
        System.Action<ControladorPreguntaUI> accion
    )
    {
        botonEliminar.onClick.AddListener(() => accion(this));
    }

    public void CargarPregunta(Pregunta pregunta)
    {
        inputEnunciado.text = pregunta.enunciado;

        inputRespuesta1.text = pregunta.respuestas[0];
        inputRespuesta2.text = pregunta.respuestas[1];
        inputRespuesta3.text = pregunta.respuestas[2];
        inputRespuesta4.text = pregunta.respuestas[3];

        dropdownRespuestaCorrecta.value = pregunta.respuestaCorrecta;
    }

    public Pregunta ObtenerPregunta(float duracion)
    {
        Pregunta pregunta = new Pregunta();

        pregunta.enunciado = inputEnunciado.text.Trim();

        pregunta.respuestas = new List<string>
        {
            inputRespuesta1.text.Trim(),
            inputRespuesta2.text.Trim(),
            inputRespuesta3.text.Trim(),
            inputRespuesta4.text.Trim()
        };

        pregunta.respuestaCorrecta = dropdownRespuestaCorrecta.value;
        pregunta.duracion = duracion;

        return pregunta;
    }

    public bool EsValida()
    {
        if (string.IsNullOrWhiteSpace(inputEnunciado.text))
            return false;

        if (string.IsNullOrWhiteSpace(inputRespuesta1.text))
            return false;

        if (string.IsNullOrWhiteSpace(inputRespuesta2.text))
            return false;

        if (string.IsNullOrWhiteSpace(inputRespuesta3.text))
            return false;

        if (string.IsNullOrWhiteSpace(inputRespuesta4.text))
            return false;

        return true;
    }
}
