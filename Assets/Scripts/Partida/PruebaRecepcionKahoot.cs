using UnityEngine;

public class PruebaRecepcionKahoot : MonoBehaviour
{
    private void Start()
    {
        Kahoot kahoot = GestorPartida.KahootActual;

        if (kahoot == null)
        {
            Debug.LogError("No se ha recibido ningún Kahoot.");
            return;
        }

        Debug.Log("Kahoot recibido: " + kahoot.nombre);
        Debug.Log("Número de preguntas: " + kahoot.preguntas.Count);
    }
}