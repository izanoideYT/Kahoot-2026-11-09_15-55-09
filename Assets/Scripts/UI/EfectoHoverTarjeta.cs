
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EfectoHoverTarjeta : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler
{
    [Header("Escala")]
    public float escalaNormal = 1f;
    public float escalaHover = 1.025f;
    public float velocidad = 10f;

    [Header("Color")]
    public Color colorHover = new Color(0.20f, 0.16f, 0.32f);

    private RectTransform rectTransform;
    private Image imagen;

    private Color colorOriginal;
    private float escalaObjetivo;
    private bool estaEncima;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        imagen = GetComponent<Image>();

        colorOriginal = imagen.color;
        escalaObjetivo = escalaNormal;
    }

    private void Update()
    {
        Vector3 escala = Vector3.one * escalaObjetivo;

        rectTransform.localScale = Vector3.Lerp(
            rectTransform.localScale,
            escala,
            Time.deltaTime * velocidad
        );

        Color objetivo = estaEncima ? colorHover : colorOriginal;

        imagen.color = Color.Lerp(
            imagen.color,
            objetivo,
            Time.deltaTime * velocidad
        );
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        estaEncima = true;
        escalaObjetivo = escalaHover;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        estaEncima = false;
        escalaObjetivo = escalaNormal;
    }
}
