
using UnityEngine;
using TMPro;

public class EfectoBrilloTexto : MonoBehaviour
{
    [Header("Configuracion")]
    public float velocidad = 0.8f;
    public float anchuraBrillo = 0.12f;
    public Color colorBrillo = Color.white;


    [Header("Colores")]
    public Color colorInicial = new Color(0.44f, 0.22f, 0.85f);
    public Color colorFinal = new Color(0.0f, 0.85f, 1f);

    public float velocidadColor = 1f;

    private TMP_Text texto;

    private Color32[][] coloresOriginales;

    private float progreso = -0.3f;

    private void Awake()
    {
        texto = GetComponent<TMP_Text>();

        texto.ForceMeshUpdate();

        GuardarColoresOriginales();
    }

    private void GuardarColoresOriginales()
    {
        TMP_TextInfo info = texto.textInfo;

        coloresOriginales = new Color32[info.meshInfo.Length][];

        for (int i = 0; i < info.meshInfo.Length; i++)
        {
            coloresOriginales[i] =
                (Color32[])info.meshInfo[i].colors32.Clone();
        }
    }

    private void Update()
    {
        if (texto.textInfo.characterCount == 0)
            return;

        progreso += velocidad * Time.deltaTime;

        if (progreso > 1.3f)
            progreso = -0.3f;

        TMP_TextInfo info = texto.textInfo;

        for (int i = 0; i < info.meshInfo.Length; i++)
        {
            System.Array.Copy(
                coloresOriginales[i],
                info.meshInfo[i].colors32,
                coloresOriginales[i].Length
            );
        }

        int totalCaracteres = info.characterCount;

        for (int i = 0; i < totalCaracteres; i++)
        {
            TMP_CharacterInfo caracter = info.characterInfo[i];

            if (!caracter.isVisible)
                continue;

            float posicion = (float)i / Mathf.Max(1, totalCaracteres - 1);

            float onda = Mathf.PingPong(
                Time.time * velocidadColor + posicion,
                1f
            );

            Color colorLetra = Color.Lerp(
                colorInicial,
                colorFinal,
                onda
            );

            float distancia = Mathf.Abs(posicion - progreso);

            float intensidad = Mathf.Clamp01(
                1f - distancia / anchuraBrillo
            );

            colorLetra = Color.Lerp(
                colorLetra,
                colorBrillo,
                intensidad
            );

            int materialIndex = caracter.materialReferenceIndex;
            int vertexIndex = caracter.vertexIndex;

            Color32[] colores = info.meshInfo[materialIndex].colors32;

            for (int j = 0; j < 4; j++)
            {
                colores[vertexIndex + j] = colorLetra;
            }
        }

        texto.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
    }
}
