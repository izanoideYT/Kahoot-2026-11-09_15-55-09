
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ControladorLeaderboard : MonoBehaviour
{
    [Header("Selector de Kahoots")]
    public GameObject prefabKahoot;
    public Transform contenidoKahoots;

    [Header("Clasificacion")]
    public GameObject prefabEntradaLeaderboard;
    public Transform contenidoLeaderboard;
    public TMP_Text textoNombreKahoot;

    public GameObject panelLeaderboard;

    private List<Kahoot> kahoots;

    private void Start()
    {
        panelLeaderboard.SetActive(false);
        CargarKahoots();
    }

    private void CargarKahoots()
    {
        kahoots = GestorKahoots.ObtenerKahoots();

        foreach (Kahoot kahoot in kahoots)
        {
            GameObject boton = Instantiate(
                prefabKahoot,
                contenidoKahoots
            );

            TMP_Text texto = boton.GetComponentInChildren<TMP_Text>();
            Button componenteBoton = boton.GetComponent<Button>();

            if (texto == null || componenteBoton == null)
            {
                Debug.LogError("El prefab Kahoot no tiene los componentes necesarios.");
                continue;
            }

            texto.text = kahoot.nombre;

            componenteBoton.onClick.AddListener(
                () => SeleccionarKahoot(kahoot)
            );
        }
    }

    private void SeleccionarKahoot(Kahoot kahoot)
    {
        panelLeaderboard.SetActive(true);

        textoNombreKahoot.text = kahoot.nombre;

        MostrarLeaderboard(kahoot.nombre);
    }

    private void MostrarLeaderboard(string nombreKahoot)
    {
        foreach (Transform hijo in contenidoLeaderboard)
        {
            Destroy(hijo.gameObject);
        }

        LeaderboardData datos =
            GestorLeaderboard.CargarLeaderboard(nombreKahoot);

        for (int i = 0; i < datos.entradas.Count; i++)
        {
            EntradaLeaderboard entrada = datos.entradas[i];

            GameObject objeto = Instantiate(
                prefabEntradaLeaderboard,
                contenidoLeaderboard
            );

            TMP_Text[] textos = objeto.GetComponentsInChildren<TMP_Text>();

            foreach (TMP_Text texto in textos)
            {
                switch (texto.name)
                {
                    case "Text Posicion":
                        texto.text = (i + 1).ToString();
                        break;

                    case "Text Nombre Usuario":
                        texto.text = entrada.nombreUsuario;
                        break;

                    case "Text Puntuacion":
                        texto.text = entrada.puntuacion.ToString();
                        break;

                    case "Text Tiempo":
                        texto.text = entrada.tiempoTotal.ToString("F1") + " s";
                        break;
                }
            }
        }

        Debug.Log("Leaderboard cargado: " + nombreKahoot);
    }
}
