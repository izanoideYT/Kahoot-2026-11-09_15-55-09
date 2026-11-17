using UnityEngine;

public class Inicializador : MonoBehaviour
{
    private void Awake()
    {
        GestorArchivos.inicializar();
        Debug.Log("Sistema de archivos Encendido");
    }
}
