
using UnityEngine;
using UnityEngine.EventSystems;

public class ControladorBotonMenu : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler
{
    [Header("Configuracion")]
    public float escalaNormal = 1f;
    public float escalaHover = 1.08f;
    public float velocidad = 10f;

    [Header("Transicion")]
    public string nombreEscena;
    private RectTransform rectTransform;

    
    private float escalaObjetivo;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
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
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        escalaObjetivo = escalaHover;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        escalaObjetivo = escalaNormal;
    }

    public void Seleccionar()
    {
        TransicionEscena.Instancia.CambiarEscena(
            nombreEscena,
            rectTransform
        );
    }
}
