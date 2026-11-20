
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControladorVolver : MonoBehaviour
{
    public void Volver()
    {
        int escenaAnterior = PlayerPrefs.GetInt("EscenaAnterior", -1);

        if (escenaAnterior != -1)
        {
            SceneManager.LoadScene(escenaAnterior);
        }
    }
}
