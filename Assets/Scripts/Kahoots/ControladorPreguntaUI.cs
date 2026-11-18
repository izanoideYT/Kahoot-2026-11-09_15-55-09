
using UnityEngine;
using TMPro;

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

    public Pregunta ObtenerPregunta(float duracion)
    {
        Pregunta pregunta = new Pregunta();

        pregunta.enunciado = inputEnunciado.text.Trim();

        pregunta.respuestas = new System.Collections.Generic.List<string>
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
