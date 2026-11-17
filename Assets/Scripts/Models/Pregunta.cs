using UnityEngine;
using System.Collections.Generic;
using System;


[Serializable]
public class Pregunta
{
    public string enunciado;
    public List<string> respuestas;
    public int respuestaCorrecta;
    public float duracion;

}
