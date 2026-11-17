using UnityEngine;

using System.Collections.Generic;

public class PruebaKahoots : MonoBehaviour
{
    private void Start()
    {
        List<Kahoot> kahoots = GestorKahoots.ObtenerKahoots();

        Debug.Log("Kahoot encontrado: " + kahoots.Count);

        foreach ( Kahoot kahoot in kahoots)
        {

            Debug.Log("Nombre: " + kahoot.nombre);
            Debug.Log("Número de preguntas: " + kahoot.preguntas.Count);

            foreach (Pregunta pregunta in kahoot.preguntas)
            {
                Debug.Log("Pregunta: " + pregunta.enunciado);
                Debug.Log("Respuesta correcta: " + pregunta.respuestas[pregunta.respuestaCorrecta]);
            }
        }
        
    }
}
