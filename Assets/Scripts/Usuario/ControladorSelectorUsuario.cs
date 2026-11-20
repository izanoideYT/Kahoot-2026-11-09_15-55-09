
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ControladorSelectorUsuario : MonoBehaviour
{
    [Header("Selector")]
    public GameObject prefabUsuario;
    public Transform contenido;

    [Header("Creacion")]
    public TMP_InputField campoNombre;

    [Header("Botones")]
    public Button botonCrear;
    public Button botonBorrar;
    public Button botonCancelar;

    private List<Usuario> usuarios;
    private Usuario usuarioSeleccionado;
    private GameObject tarjetaSeleccionada;

    private void Start()
    {
        botonCrear.onClick.AddListener(CrearUsuario);
        botonBorrar.onClick.AddListener(BorrarUsuario);
        botonCancelar.onClick.AddListener(CancelarCreacion);

        botonBorrar.interactable = false;

        CargarUsuarios();
    }

    public void CargarUsuarios()
    {
        foreach (Transform hijo in contenido)
        {
            Destroy(hijo.gameObject);
        }

        usuarios = GestorUsuarios.ObtenerUsuarios();

        foreach (Usuario usuario in usuarios)
        {
            GameObject tarjeta = Instantiate(prefabUsuario, contenido);

            TMP_Text texto = tarjeta.transform.Find("Text Nombre Usuario")
                ?.GetComponent<TMP_Text>();

            Button botonJugar = tarjeta.transform.Find("selectBTn")
                ?.GetComponent<Button>();

            if (texto == null || botonJugar == null)
            {
                Debug.LogError("Falta Text Nombre Usuario o Button Jugar en " + tarjeta.name);
                continue;
            }

            texto.text = usuario.nombre;

            Button botonTarjeta = tarjeta.GetComponent<Button>();

            if (botonTarjeta != null)
            {
                botonTarjeta.onClick.AddListener(
                    () => SeleccionarUsuario(usuario, tarjeta)
                );
            }

            botonJugar.onClick.AddListener(() => Jugar(usuario));
        }
    }

    private void SeleccionarUsuario(Usuario usuario, GameObject tarjeta)
    {
        usuarioSeleccionado = usuario;
        tarjetaSeleccionada = tarjeta;

        botonBorrar.interactable = true;

        Debug.Log("Usuario seleccionado: " + usuario.nombre);
    }

    private void Jugar(Usuario usuario)
    {
        GestorUsuarios.SeleccionarUsuario(usuario);

        if (TransicionEscena.Instancia != null)
        {
            TransicionEscena.Instancia.CambiarEscenaDirecta("Selecionar Kahoot");
        }
        else
        {
            Debug.LogError("No se ha encontrado TransicionEscena.");
        }
    }

    private void CrearUsuario()
    {
        string nombre = campoNombre.text.Trim();

        if (string.IsNullOrWhiteSpace(nombre))
        {
            Debug.LogWarning("Introduce un nombre de usuario.");
            return;
        }

        foreach (Usuario usuario in usuarios)
        {
            if (usuario.nombre.ToLower() == nombre.ToLower())
            {
                Debug.LogWarning("Ese usuario ya existe.");
                return;
            }
        }

        Usuario nuevoUsuario = new Usuario();
        nuevoUsuario.nombre = nombre;

        GestorUsuarios.GuardarUsuario(nuevoUsuario);

        campoNombre.text = "";

        usuarioSeleccionado = null;
        tarjetaSeleccionada = null;
        botonBorrar.interactable = false;

        CargarUsuarios();

        Debug.Log("Usuario creado: " + nombre);
    }

    private void BorrarUsuario()
    {
        if (usuarioSeleccionado == null)
        {
            Debug.LogWarning("Selecciona un usuario primero.");
            return;
        }

        GestorUsuarios.EliminarUsuario(usuarioSeleccionado);

        Debug.Log("Usuario eliminado: " + usuarioSeleccionado.nombre);

        usuarioSeleccionado = null;
        tarjetaSeleccionada = null;
        botonBorrar.interactable = false;

        CargarUsuarios();
    }

    private void CancelarCreacion()
    {
        campoNombre.text = "";
    }

    public void Volver()
    {
        Debug.Log("Volviendo al menú principal...");
        SceneManager.LoadScene("Main Scene");
    }
}
