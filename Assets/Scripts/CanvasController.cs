using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class CanvasController : MonoBehaviour
{
    [Header("Paneles")]
    public GameObject panelMain;
    public GameObject panelDoc;
    public GameObject panelPreguntas;
    public GameObject panelGanaste;
    public GameObject panelPerdiste;

    [Header("Botones principales")]
    public Button btnEliminar;
    public Button btnSalvar;
    public Button btnLeerDoc;
    public Button btnPreguntas;
    public Button btnGirarIzq;
    public Button btnGirarDer;

    [Header("Panel Doc")]
    public Image imgFoto;
    public Image imgDocumento;
    public Button btnRegresarDoc;

    [Header("Panel Preguntas")]
    public Button btnPregunta1;
    public Button btnPregunta2;
    public Button btnPregunta3;
    public Button btnRegresarPreguntas;
    public TMP_Text textoRespuesta;

    [Header("NPC Prefab")]
    public GameObject npcPrefab;

    private GameObject cuboActual;
    private bool girando = false;

    void Start()
    {
        MostrarSolo(panelMain);

        // Eventos principales
        btnEliminar.onClick.AddListener(EliminarCubo);
        btnSalvar.onClick.AddListener(SalvarCubo);
        btnLeerDoc.onClick.AddListener(() => MostrarSolo(panelDoc));
        btnPreguntas.onClick.AddListener(() => MostrarPanelPreguntas());
        btnGirarIzq.onClick.AddListener(() => GirarUnaVez(btnGirarIzq, -45f));
        btnGirarDer.onClick.AddListener(() => GirarUnaVez(btnGirarDer, 45f));

        // Regresar
        btnRegresarDoc.onClick.AddListener(() => MostrarSolo(panelMain));
        btnRegresarPreguntas.onClick.AddListener(() => MostrarSolo(panelMain));

        // Preguntas
        btnPregunta1.onClick.AddListener(() => PreguntaRespondida(btnPregunta1, 1));
        btnPregunta2.onClick.AddListener(() => PreguntaRespondida(btnPregunta2, 2));
        btnPregunta3.onClick.AddListener(() => PreguntaRespondida(btnPregunta3, 3));

        // Instanciar NPC
        if (npcPrefab != null)
            cuboActual = Instantiate(npcPrefab, new Vector3(0, 0, -2f), Quaternion.identity);
    }

    void MostrarSolo(GameObject panel)
    {
        panelMain.SetActive(false);
        panelDoc.SetActive(false);
        panelPreguntas.SetActive(false);
        panelGanaste.SetActive(false);
        panelPerdiste.SetActive(false);

        panel.SetActive(true);
        textoRespuesta.text = "";

        // Reactivar botones de preguntas al salir del panel
        if (panel == panelMain)
        {
            btnPregunta1.interactable = true;
            btnPregunta2.interactable = true;
            btnPregunta3.interactable = true;
        }

        // Activar imágenes solo en panelDoc
        if (panel == panelDoc)
        {
            imgFoto.enabled = true;
            imgDocumento.enabled = true;
        }
    }

    void MostrarPanelPreguntas()
    {
        MostrarSolo(panelPreguntas);
        btnPregunta1.interactable = true;
        btnPregunta2.interactable = true;
        btnPregunta3.interactable = true;
    }

    void EliminarCubo()
    {
        if (cuboActual != null)
        {
            Destroy(cuboActual);
            cuboActual = null;
        }

        // Reactivar botones de rotación al eliminar el cubo
        btnGirarIzq.interactable = true;
        btnGirarDer.interactable = true;
    }

    void SalvarCubo()
    {
        float random = Random.value;
        if (random < 0.5f)
            MostrarSolo(panelGanaste);
        else
            MostrarSolo(panelPerdiste);
    }

    void PreguntaRespondida(Button boton, int pregunta)
    {
        boton.interactable = false;
        MostrarRespuesta(pregunta);
    }

    void MostrarRespuesta(int pregunta)
    {
        string[] respuestas;

        switch (pregunta)
        {
            case 1:
                respuestas = new string[]
                {
                    "Vengo del norte.",
                    "No recuerdo bien.",
                    "Solo estoy de paso."
                };
                break;

            case 2:
                respuestas = new string[]
                {
                    "Traigo papeles importantes.",
                    "Nada sospechoso.",
                    "Un regalo para mi familia."
                };
                break;

            case 3:
                respuestas = new string[]
                {
                    "Busco trabajo.",
                    "Vine a visitar a alguien.",
                    "Prefiero no responder."
                };
                break;

            default:
                respuestas = new string[] { "..." };
                break;
        }

        int i = Random.Range(0, respuestas.Length);
        textoRespuesta.text = respuestas[i];
    }

    void GirarUnaVez(Button boton, float grados)
    {
        if (cuboActual != null && !girando)
        {
            boton.interactable = false; // Desactiva ese botón tras uso
            StartCoroutine(RotarSuavemente(grados));
        }
    }

    IEnumerator RotarSuavemente(float grados)
    {
        girando = true;
        Quaternion rotInicial = cuboActual.transform.rotation;
        Quaternion rotFinal = rotInicial * Quaternion.Euler(0, grados, 0);
        float tiempo = 0f;
        float duracion = .5f;

        while (tiempo < duracion)
        {
            tiempo += Time.deltaTime;
            float t = Mathf.Clamp01(tiempo / duracion);
            cuboActual.transform.rotation = Quaternion.Slerp(rotInicial, rotFinal, t);
            yield return null;
        }

        cuboActual.transform.rotation = rotFinal;
        girando = false;
    }
}
