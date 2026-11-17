
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControladorMenuPrincipal : MonoBehaviour
{
    public void Jugar()
    {
        SceneManager.LoadScene("Archivo Usuario");
    }

    public void Leaderboard()
    {
        SceneManager.LoadScene("Leaderboard");
    }

    public void About()
    {
        SceneManager.LoadScene("About");
    }

    public void Salir()
    {
        Application.Quit();
    }
}
