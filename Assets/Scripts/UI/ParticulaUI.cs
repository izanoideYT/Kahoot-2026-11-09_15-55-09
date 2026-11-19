
using UnityEngine;
using TMPro;

public class ParticulaUI : MonoBehaviour
{
    [Header("Movimiento")]
    public float velocidad = 15f;

    [Header("Opacidad")]
    public float opacidadMaxima = 0.7f;
    public float velocidadFade = 1f;

    private RectTransform rectTransform;
    private RectTransform contenedor;
    private TMP_Text texto;

    private float opacidadActual;
    private bool apareciendo = true;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        contenedor = transform.parent as RectTransform;
        texto = GetComponent<TMP_Text>();

        opacidadActual = 0f;
    }

    private void Update()
    {
        MoverParticula();
        ActualizarOpacidad();
    }

    private void MoverParticula()
    {
        rectTransform.anchoredPosition +=
            Vector2.up * velocidad * Time.deltaTime;

        if (rectTransform.anchoredPosition.y >
            contenedor.rect.height / 2f + 30f)
        {
            ReiniciarParticula();
        }
    }

    private void ActualizarOpacidad()
    {
        if (apareciendo)
        {
            opacidadActual += velocidadFade * Time.deltaTime;

            if (opacidadActual >= opacidadMaxima)
            {
                opacidadActual = opacidadMaxima;
                apareciendo = false;
            }
        }
        else
        {
            float limiteFade =
                contenedor.rect.height / 2f - 100f;

            if (rectTransform.anchoredPosition.y > limiteFade)
            {
                opacidadActual -= velocidadFade * Time.deltaTime;
                opacidadActual = Mathf.Max(0f, opacidadActual);
            }
        }

        Color color = texto.color;
        color.a = opacidadActual;
        texto.color = color;
    }

    private void ReiniciarParticula()
    {
        Vector2 posicion = rectTransform.anchoredPosition;

        posicion.y = -contenedor.rect.height / 2f - 30f;

        posicion.x = Random.Range(
            -contenedor.rect.width / 2f,
            contenedor.rect.width / 2f
        );

        rectTransform.anchoredPosition = posicion;

        opacidadActual = 0f;
        apareciendo = true;
    }
}
