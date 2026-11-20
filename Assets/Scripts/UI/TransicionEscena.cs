
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransicionEscena : MonoBehaviour
{
    public static TransicionEscena Instancia;

    [Header("Configuracion")]
    public float duracion = 0.8f;

    private bool enTransicion = false;
    private int escenaAnterior = -1;

    private void Awake()
    {
        if (Instancia == null)
        {
            Instancia = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // CAMBIO DE ESCENA CON ANIMACION
    public void CambiarEscena(string nombreEscena, RectTransform boton)
    {
        if (enTransicion || boton == null)
            return;

        enTransicion = true;

        StartCoroutine(AnimarTransicion(nombreEscena, boton));
    }

    private IEnumerator AnimarTransicion(string nombreEscena, RectTransform boton)
    {
        boton.SetAsLastSibling();

        Canvas canvas = boton.GetComponentInParent<Canvas>();
        RectTransform canvasRect = canvas.GetComponent<RectTransform>();

        float ancho = canvasRect.rect.width;
        float alto = canvasRect.rect.height;

        float distancia = Mathf.Sqrt(ancho * ancho + alto * alto);

        float escalaFinal = distancia /
            Mathf.Min(boton.rect.width, boton.rect.height) * 2f;

        Vector3 escalaInicial = boton.localScale;

        float tiempo = 0f;

        while (tiempo < duracion)
        {
            tiempo += Time.deltaTime;

            float progreso = Mathf.Clamp01(tiempo / duracion);
            float suavizado = Mathf.SmoothStep(0f, 1f, progreso);

            boton.localScale = Vector3.Lerp(
                escalaInicial,
                Vector3.one * escalaFinal,
                suavizado
            );

            yield return null;
        }

        escenaAnterior = SceneManager.GetActiveScene().buildIndex;

        SceneManager.LoadScene(nombreEscena);
    }

    // CAMBIO DE ESCENA SIN ANIMACION
    public void CambiarEscenaDirecta(string nombreEscena)
    {
        if (enTransicion)
            return;

        enTransicion = true;

        escenaAnterior = SceneManager.GetActiveScene().buildIndex;

        SceneManager.LoadScene(nombreEscena);
    }

    // VOLVER A LA ESCENA ANTERIOR
    public void Volver()
    {
        if (enTransicion || escenaAnterior == -1)
            return;

        enTransicion = true;

        int destino = escenaAnterior;
        escenaAnterior = -1;

        Debug.Log("Escena anterior guardada: " + escenaAnterior);

        SceneManager.LoadScene(destino);
    }

    // REACTIVAR NAVEGACION AL CARGAR UNA ESCENA
    private void OnEnable()
    {
        SceneManager.sceneLoaded += AlCargarEscena;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= AlCargarEscena;
    }

    private void AlCargarEscena(Scene escena, LoadSceneMode modo)
    {
        enTransicion = false;
    }
}
