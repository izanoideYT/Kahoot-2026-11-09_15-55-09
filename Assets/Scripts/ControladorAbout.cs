using UnityEngine;
using UnityEngine.SceneManagement;

public class ControladorAbout : MonoBehaviour
{
    public void Volver()
    {
        SceneManager.LoadScene("Main Scene");
    }
}