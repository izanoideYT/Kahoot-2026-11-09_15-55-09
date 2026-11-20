
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ControladorSelectorKahoot : MonoBehaviour
{
    public GameObject tarjetaPrefab;
    public Transform contenido;

    private List<Kahoot> kahoots;

    private void Start()
    {
        CargarKahoots();
    }

    private void CargarKahoots()
    {
        kahoots = GestorKahoots.ObtenerKahoots();

        foreach (Kahoot kahoot in kahoots)
        {
            GameObject tarjeta = Instantiate(tarjetaPrefab, contenido);

            TMP_Text textoNombre = tarjeta.transform
                .Find("Text Nombre Kahoot")
                .GetComponent<TMP_Text>();

            textoNombre.text = kahoot.nombre;

            Button botonJugar = tarjeta.transform
                .Find("Button Jugar")
                .GetComponent<Button>();

            botonJugar.onClick.AddListener(
                () => SeleccionarKahoot(kahoot)
            );

            Button botonModificar = tarjeta.transform
                .Find("Button Modificar")
                .GetComponent<Button>();

            botonModificar.onClick.AddListener(
                () => ModificarKahoot(kahoot)
            );

            Button botonEliminar = tarjeta.transform
                    .Find("Button Eliminar")
                       .GetComponent<Button>();

            botonEliminar.onClick.AddListener(
                () => EliminarKahoot(kahoot, tarjeta)
            );
        }
    }
    private void EliminarKahoot(Kahoot kahoot, GameObject tarjeta)
    {
        if (GestorKahoots.EliminarKahoot(kahoot))
        {
            Destroy(tarjeta);
        }
    }

    private void SeleccionarKahoot(Kahoot kahoot)
    {
        GestorPartida.SeleccionarKahoot(kahoot);

        Debug.Log("Kahoot seleccionado: " + kahoot.nombre);

        SceneManager.LoadScene("Juego");
    }

    private void ModificarKahoot(Kahoot kahoot)
    {
        if (!GestorKahoots.PrepararEdicion(kahoot))
            return;

        SceneManager.LoadScene("Crear Kahoot");
    }

    public void AbrirCrearKahoot()
    {
        SceneManager.LoadScene("Crear Kahoot");
    }

    public void Volver()
    {
        Debug.Log("Volviendo a Archivo usuario...");

        SceneManager.LoadScene("Archivo usuario");
    }
}

