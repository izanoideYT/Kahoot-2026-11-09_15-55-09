
using UnityEngine;
using TMPro;

public class GeneradorParticulasUI : MonoBehaviour
{
    [Header("Configuracion")]
    public GameObject prefabParticula;
    public int cantidadParticulas = 20;

    [Header("Tamanos")]
    public float tamanoMinimo = 8f;
    public float tamanoMaximo = 22f;

    [Header("Velocidad")]
    public float velocidadMinima = 8f;
    public float velocidadMaxima = 20f;

    [Header("Colores")]
    public Color[] colores =
    {
        Color.white,
        new Color(0.44f, 0.22f, 0.85f),
        new Color(0f, 0.85f, 1f)
    };

    private RectTransform contenedor;

    private void Start()
    {
        contenedor = GetComponent<RectTransform>();

        GenerarParticulas();
    }

    private void GenerarParticulas()
    {
        for (int i = 0; i < cantidadParticulas; i++)
        {
            GameObject nuevaParticula = Instantiate(
                prefabParticula,
                transform
            );

            RectTransform rect =
                nuevaParticula.GetComponent<RectTransform>();

            TMP_Text texto =
                nuevaParticula.GetComponent<TMP_Text>();

            float tamano = Random.Range(
                tamanoMinimo,
                tamanoMaximo
            );

            rect.sizeDelta = new Vector2(tamano, tamano);

            rect.anchoredPosition = new Vector2(
                Random.Range(
                    -contenedor.rect.width / 2f,
                    contenedor.rect.width / 2f
                ),
                Random.Range(
                    -contenedor.rect.height / 2f,
                    contenedor.rect.height / 2f
                )
            );

            texto.fontSize = tamano;

            texto.color = colores[
                Random.Range(0, colores.Length)
            ];

            ParticulaUI movimiento =
                nuevaParticula.GetComponent<ParticulaUI>();

            movimiento.velocidad = Random.Range(
                velocidadMinima,
                velocidadMaxima
            );
        }
    }
}
