
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class ControladorUsuario : MonoBehaviour
{
    public TMP_InputField campoNombre;
    public Button botonConfirmar;

    private void Start()
    {
        botonConfirmar.onClick.AddListener(ConfirmarUsuario);
    }

    private void ConfirmarUsuario()
    {
        string nombre = campoNombre.text.Trim();

        if (string.IsNullOrEmpty(nombre))
        {
            Debug.LogWarning("Debes introducir un nombre.");
            return;
        }

        Usuario usuario = new Usuario();
        usuario.nombre = nombre;

        GestorUsuarios.GuardarUsuario(usuario);

        SceneManager.LoadScene("Selecionar Kahoot");
    }
}
