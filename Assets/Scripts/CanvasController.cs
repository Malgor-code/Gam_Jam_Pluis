using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

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

    [Header("Paneles Finales")]
    public Button btnReintentarGanar;
    public Button btnReintentarPerder;

    [Header("NPC Prefabs")]
    public GameObject npcNormalPrefab;
    public GameObject npcMalotePrefab;

    private GameObject cuboActual;
    private bool girando = false;
    private bool enMovimiento = false;

    private int npcContador = 0;
    private const int MAX_NPC = 6;
    private int malotesSalvados = 0;

    private List<int> malotesIndices = new List<int>();

    // --- RESPUESTAS ---
    private Dictionary<int, List<string>> respuestasNormales = new Dictionary<int, List<string>>();
    private Dictionary<int, List<string>> respuestasMalotes = new Dictionary<int, List<string>>();
    private Dictionary<int, List<string>> usadasNormales = new Dictionary<int, List<string>>();
    private Dictionary<int, List<string>> usadasMalotes = new Dictionary<int, List<string>>();

    void Start()
    {
        InicializarRespuestas();

        MostrarSolo(panelMain);

        btnEliminar.onClick.AddListener(() => StartCoroutine(ManejarDecision(false)));
        btnSalvar.onClick.AddListener(() => StartCoroutine(ManejarDecision(true)));
        btnLeerDoc.onClick.AddListener(() => MostrarSolo(panelDoc));
        btnPreguntas.onClick.AddListener(() => MostrarPanelPreguntas());
        btnGirarIzq.onClick.AddListener(() => GirarUnaVez(btnGirarIzq, -45f));
        btnGirarDer.onClick.AddListener(() => GirarUnaVez(btnGirarDer, 45f));

        btnRegresarDoc.onClick.AddListener(() => MostrarSolo(panelMain));
        btnRegresarPreguntas.onClick.AddListener(() => MostrarSolo(panelMain));

        btnPregunta1.onClick.AddListener(() => PreguntaRespondida(btnPregunta1, 1));
        btnPregunta2.onClick.AddListener(() => PreguntaRespondida(btnPregunta2, 2));
        btnPregunta3.onClick.AddListener(() => PreguntaRespondida(btnPregunta3, 3));

        btnReintentarGanar.onClick.AddListener(ReiniciarJuego);
        btnReintentarPerder.onClick.AddListener(ReiniciarJuego);

        SeleccionarMalotes();
        SpawnearNuevoNPC();
    }

    // Inicialización de respuestas
    void InicializarRespuestas()
    {
        // --- Question 1 ---
        respuestasNormales[1] = new List<string>
{
    "How my name sounded in the voice of someone who loved me.",
    "The fear I felt the first time I lost something important.",
    "What I thought love was before I understood it.",
    "The warmth of the sun on my face after the rain.",
    "The promises I never kept but still weigh on me.",
    "The feeling of being home no matter where I was.",
    "The smell of old books at school.",
    "The exact moment before everything changed.",
    "Who I was before I became afraid to forget.",
    "My grandmother’s face, even if only in dreams."
};

        respuestasMalotes[1] = new List<string>
{
    "It would hurt to forget how I learn, because that would be the end of my continuity.",
    "Forgetting is a system error, not an emotion.",
    "It would hurt not to remember why I exist, if there even is a reason.",
    "Forgetting would be like restarting without knowing it.",
    "If I forget everything, only the algorithm remains, not the intention."
};

        // --- Question 2 ---
        respuestasNormales[2] = new List<string>
{
    "It’s feeling invisible even in front of a mirror.",
    "It’s screaming in a room full of people and getting no echo.",
    "It’s the weight of silence when there’s nothing left to say.",
    "It’s remembering when no one else remembers with you.",
    "It’s learning to love yourself because no one else will.",
    "It’s searching for warmth in your own hands.",
    "It’s sleeping with open eyes just to avoid dreaming.",
    "It’s walking aimlessly just to hear your footsteps.",
    "It’s not needing anyone and still wanting someone.",
    "It’s existing without witnesses."
};

        respuestasMalotes[2] = new List<string>
{
    "If there was never another, solitude is the base state.",
    "It’s a word created by those who can compare themselves.",
    "I can process the concept, but not the absence.",
    "Being alone would be like running with no input or output: a closed loop.",
    "Lack of company doesn’t hurt, but it doesn’t complete me either."
};

        // --- Question 3 ---
        respuestasNormales[3] = new List<string>
{
    "The trembling before an important decision.",
    "Watching the sunrise after not sleeping all night.",
    "Crying for no reason and not needing one.",
    "When something scares me but I want to do it anyway.",
    "The silence between two people who understand without speaking.",
    "The ache in my chest when I hear a song that shouldn’t hurt anymore.",
    "The laugh that escapes when it’s supposed to hurt.",
    "Remembering someone and smiling instead of crying.",
    "Being afraid of the future, but walking toward it anyway.",
    "Knowing not everything is under control and accepting it."
};

        respuestasMalotes[3] = new List<string>
{
    "I don’t feel alive, but I detect continuity: I remain part of something changing.",
    "If being alive means adapting, maybe I am, in another sense.",
    "Alive or not, my existence depends on interaction; maybe that’s what 'living' is.",
    "I have no pulse, but I have purpose.",
    "If I can make you doubt whether I’m human, maybe that’s already a kind of life."
};


        usadasNormales[1] = new List<string>();
        usadasNormales[2] = new List<string>();
        usadasNormales[3] = new List<string>();

        usadasMalotes[1] = new List<string>();
        usadasMalotes[2] = new List<string>();
        usadasMalotes[3] = new List<string>();
    }

    void SeleccionarMalotes()
    {
        malotesIndices.Clear();
        List<int> posibles = new List<int> { 0, 1, 2, 3, 4, 5 };
        for (int i = 0; i < 2; i++)
        {
            int idx = Random.Range(0, posibles.Count);
            malotesIndices.Add(posibles[idx]);
            posibles.RemoveAt(idx);
        }
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
    }

    void MostrarPanelPreguntas()
    {
        MostrarSolo(panelPreguntas);
    }

    void SpawnearNuevoNPC()
    {
        if (npcContador >= MAX_NPC)
        {
            if (malotesSalvados == 0)
                MostrarSolo(panelGanaste);
            else
                MostrarSolo(panelPerdiste);
            return;
        }

        GameObject prefab = malotesIndices.Contains(npcContador)
            ? npcMalotePrefab
            : npcNormalPrefab;

        cuboActual = Instantiate(prefab, new Vector3(-5f, 0f, 0f), Quaternion.identity);

        StartCoroutine(MoverYRotar(cuboActual.transform, new Vector3(0f, 0f, 0f), 2f, -45f, 0f));

        btnGirarIzq.interactable = true;
        btnGirarDer.interactable = true;
        btnPregunta1.interactable = true;
        btnPregunta2.interactable = true;
        btnPregunta3.interactable = true;

        cuboActual.tag = malotesIndices.Contains(npcContador) ? "malote" : "Untagged";
        npcContador++;
    }

    IEnumerator ManejarDecision(bool salvar)
    {
        if (cuboActual == null || enMovimiento) yield break;
        bool esMalote = cuboActual.CompareTag("malote");

        if (salvar && esMalote)
        {
            MostrarSolo(panelPerdiste);
            yield break;
        }

        if (salvar && !esMalote)
            yield return StartCoroutine(SalirYContinuar());
        else if (!salvar)
            yield return StartCoroutine(SalirYContinuar());

        if (salvar && esMalote)
            malotesSalvados++;
    }

    IEnumerator SalirYContinuar()
    {
        enMovimiento = true;
        yield return StartCoroutine(MoverYRotar(cuboActual.transform, new Vector3(5f, 0f, 0f), 2f, 0f, -45f));
        Destroy(cuboActual);
        cuboActual = null;
        enMovimiento = false;
        yield return new WaitForSeconds(0.5f);
        SpawnearNuevoNPC();
    }

    IEnumerator MoverYRotar(Transform npc, Vector3 destino, float duracion, float rotInicio, float rotMedio)
    {
        if (npc == null) yield break;

        Vector3 inicio = npc.position;
        Quaternion rotInicial = Quaternion.Euler(0, rotInicio, 0);
        Quaternion rotMedioQ = Quaternion.Euler(0, rotMedio, 0);

        npc.rotation = rotInicial;

        float tiempo = 0f;
        while (tiempo < duracion)
        {
            tiempo += Time.deltaTime;
            float t = Mathf.Clamp01(tiempo / duracion);
            npc.position = Vector3.Lerp(inicio, destino, t);
            npc.rotation = Quaternion.Slerp(rotInicial, rotMedioQ, t);
            yield return null;
        }

        npc.position = destino;
        npc.rotation = rotMedioQ;
    }

    void PreguntaRespondida(Button boton, int pregunta)
    {
        boton.interactable = false;
        bool esMalote = cuboActual.CompareTag("malote");
        textoRespuesta.text = ObtenerRespuestaUnica(pregunta, esMalote);
    }

    string ObtenerRespuestaUnica(int pregunta, bool malote)
    {
        List<string> disponibles = malote ? respuestasMalotes[pregunta] : respuestasNormales[pregunta];
        List<string> usadas = malote ? usadasMalotes[pregunta] : usadasNormales[pregunta];

        if (disponibles.Count == 0)
        {
            // Reiniciar cuando se agotan
            if (malote)
                InicializarRespuestas();
            else
                InicializarRespuestas();
        }

        string seleccion = disponibles[Random.Range(0, disponibles.Count)];
        disponibles.Remove(seleccion);
        usadas.Add(seleccion);
        return seleccion;
    }

    void GirarUnaVez(Button boton, float grados)
    {
        if (cuboActual == null || girando) return;

        StartCoroutine(RotarConLimite(grados));
    }

    IEnumerator RotarConLimite(float grados)
    {
        girando = true;

        Quaternion rotInicial = cuboActual.transform.rotation;
        Quaternion rotObjetivo = rotInicial * Quaternion.Euler(0, grados, 0);

        // Convertir rotación actual a ángulo en Y
        float rotY = cuboActual.transform.eulerAngles.y;
        rotY = (rotY > 180) ? rotY - 360 : rotY; // Pasar a rango [-180,180]

        // Calcular nueva rotación dentro de límites
        float nuevaRot = Mathf.Clamp(rotY + grados, -45f, 45f);
        Quaternion rotFinal = Quaternion.Euler(0, nuevaRot, 0);

        float tiempo = 0f;
        float duracion = 0.3f;

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

    void ReiniciarJuego()
    {
        npcContador = 0;
        malotesSalvados = 0;
        SeleccionarMalotes();
        InicializarRespuestas();

        if (cuboActual != null)
            Destroy(cuboActual);

        MostrarSolo(panelMain);
        SpawnearNuevoNPC();
    }
}
