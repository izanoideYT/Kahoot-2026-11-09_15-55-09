
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ControladorResultados : MonoBehaviour
{
    [Header("Datos del Kahoot")]
    public TMP_Text textoNombreKahoot;
    public TMP_Text textoPreguntasTotales;

    [Header("Estadisticas")]
    public TMP_Text textoPuntuacionFinal;
    public TMP_Text textoRespuestasCorrectas;
    public TMP_Text textoRespuestasIncorrectas;

    [Header("Botones")]
    public Button botonVolverAJugar;
    public Button botonVerLeaderboard;
    public Button botonVolverMenu;

    private void Start()
    {
        MostrarResultados();

        botonVolverAJugar.onClick.AddListener(VolverAJugar);
        botonVerLeaderboard.onClick.AddListener(VerLeaderboard);
        botonVolverMenu.onClick.AddListener(VolverMenu);
    }

    private void MostrarResultados()
    {
        Resultado resultado = GestorPartida.ResultadoActual;
        Kahoot kahoot = GestorPartida.KahootActual;

        if (resultado == null || kahoot == null)
        {
            Debug.LogError("No se han encontrado los resultados de la partida.");
            return;
        }

        textoNombreKahoot.text = kahoot.nombre;

        textoPreguntasTotales.text =
            "Preguntas totales: " + kahoot.preguntas.Count;

        textoPuntuacionFinal.text =
            "Puntuación final: " + resultado.puntuacion;

        textoRespuestasCorrectas.text =
            "Respuestas correctas: " + resultado.respuestasCorrectas;

        textoRespuestasIncorrectas.text =
            "Respuestas incorrectas: " + resultado.respuestasIncorrectas;
    }

    private void VolverAJugar()
    {
        SceneManager.LoadScene("Juego");
    }

    private void VerLeaderboard()
    {
        SceneManager.LoadScene("Leaderboard");
    }

    private void VolverMenu()
    {
        GestorPartida.LimpiarPartida();
        SceneManager.LoadScene("Main Scene");
    }
}
