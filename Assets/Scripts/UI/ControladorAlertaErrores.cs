
using UnityEngine;
using TMPro;
using System.Collections;

public class ControladorAlertaErrores : MonoBehaviour
{
    [Header("Alerta")]
    public RectTransform panelAlerta;
    public TMP_Text textoAlerta;

    [Header("Animacion")]
    public float posicionVisible = -30f;
    public float posicionOculta = 100f;
    public float velocidad = 8f;
    public float duracion = 4f;

    private bool mostrandoAlerta = false;

    private void Start()
    {
        panelAlerta.anchoredPosition = new Vector2(
            0, posicionOculta
        );

        int cantidadErrores = GestorExcepciones.ObtenerCantidadErrores();

        if (cantidadErrores > 0)
        {
            MostrarAlerta(
                "Se han detectado " + cantidadErrores + " errores en el sistema."
            );
        }
    }

    public void MostrarAlerta(string mensaje)
    {
        if (!mostrandoAlerta)
        {
            textoAlerta.text = mensaje;
            StartCoroutine(AnimarAlerta());
        }
    }

    private IEnumerator AnimarAlerta()
    {
        mostrandoAlerta = true;

        // Entrada
        yield return StartCoroutine(MoverPanel(posicionVisible));

        // Tiempo visible
        yield return new WaitForSeconds(duracion);

        // Salida
        yield return StartCoroutine(MoverPanel(posicionOculta));

        mostrandoAlerta = false;
    }

    private IEnumerator MoverPanel(float destino)
    {
        while (Mathf.Abs(panelAlerta.anchoredPosition.y - destino) > 0.5f)
        {
            Vector2 posicion = panelAlerta.anchoredPosition;

            posicion.y = Mathf.Lerp(
                posicion.y,
                destino,
                Time.deltaTime * velocidad
            );

            panelAlerta.anchoredPosition = posicion;

            yield return null;
        }

        Vector2 posicionFinal = panelAlerta.anchoredPosition;
        posicionFinal.y = destino;
        panelAlerta.anchoredPosition = posicionFinal;
    }
}
