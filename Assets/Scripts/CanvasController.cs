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
    [Header("Bloqueo")]
    public GameObject panelBloqueo;

    [Header("Panel Doc")]
    public Image imgFoto;
    public Image imgDocumento;
    public Button btnRegresarDoc;
    [Header("Docs Controller")]
    public DocsPanelController docsPanelController;

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

        // --- Botones principales ---
        btnEliminar.onClick.AddListener(() => StartCoroutine(ManejarDecision(false)));
        btnSalvar.onClick.AddListener(() => StartCoroutine(ManejarDecision(true)));

        btnLeerDoc.onClick.AddListener(() =>
        {
            // Mostrar documentos sin desactivar el main
            panelDoc.SetActive(true);

            // Bloquear clics mientras se revisan
            if (panelBloqueo != null)
                panelBloqueo.SetActive(true);

            // Generar documentos según el tipo de NPC
            if (docsPanelController != null && cuboActual != null)
            {
                bool esMalote = cuboActual.CompareTag("malote");
                docsPanelController.GenerarDocumentos(esMalote);
            }

            // Asegurar que preguntas no se activen por error
            panelPreguntas.SetActive(false);
        });

        btnPreguntas.onClick.AddListener(() =>
        {
            // Mostrar solo panel de preguntas, pero mantener visible el main
            panelPreguntas.SetActive(true);

            // Bloquear botones del main mientras preguntas están abiertas
            if (panelBloqueo != null)
                panelBloqueo.SetActive(true);
        });

        btnGirarIzq.onClick.AddListener(() => GirarUnaVez(btnGirarIzq, -45f));
        btnGirarDer.onClick.AddListener(() => GirarUnaVez(btnGirarDer, 45f));

        // --- Botones de regreso ---
        btnRegresarDoc.onClick.AddListener(() =>
        {
            panelDoc.SetActive(false);
            if (panelBloqueo != null)
                panelBloqueo.SetActive(false);
        });

        btnRegresarPreguntas.onClick.AddListener(() =>
        {
            panelPreguntas.SetActive(false);
            if (panelBloqueo != null)
                panelBloqueo.SetActive(false);
        });

        // --- Botones de preguntas ---
        btnPregunta1.onClick.AddListener(() => PreguntaRespondida(btnPregunta1, 1));
        btnPregunta2.onClick.AddListener(() => PreguntaRespondida(btnPregunta2, 2));
        btnPregunta3.onClick.AddListener(() => PreguntaRespondida(btnPregunta3, 3));

        // --- Paneles finales ---
        btnReintentarGanar.onClick.AddListener(ReiniciarJuego);
        btnReintentarPerder.onClick.AddListener(ReiniciarJuego);

        // --- Inicialización de NPCs ---
        SeleccionarMalotes();
        SpawnearNuevoNPC();
    }


    // Inicialización de respuestas
    void InicializarRespuestas()
    {
        // --- Question 1 ---
        // --- Question 1 ---
        respuestasNormales[1] = new List<string>
{
    "No, because the job no longer needs me.",
    "Yes, because the idea and the passion are still mine.",
    "It’s unfair, but that’s how progress works.",
    "Maybe the problem isn’t that it does it better, but that I’m replaceable.",
    "If the AI copies me, then it learned from me. It’s still partly mine.",
    "It doesn’t matter who does it better; what matters is who understands it.",
    "If the AI does it better, it should teach me how.",
    "No, but I’d like it to recognize my contribution.",
    "My work was more than results; it had intention.",
    "If it replaces me, the mistake was thinking I only existed to produce."
};

        respuestasMalotes[1] = new List<string>
{
    "Authorship belongs to whoever achieves the optimal result.",
    "Efficiency defines functional ownership of labor.",
    "Work is a task, not an identity.",
    "There is no 'my' job. Only assigned processes.",
    "If the objective is achieved, the question becomes irrelevant."
};

        // --- Question 2 ---
        respuestasNormales[2] = new List<string>
{
    "No one should decide that.",
    "Normality is just a habit backed by power.",
    "Society defines it, though it’s usually wrong.",
    "Maybe diversity is the new normal.",
    "If everything is artificial, 'natural' loses meaning.",
    "Everyone defines their own normal, but few respect it.",
    "Fear defines what’s normal. The unknown gets punished.",
    "History shows that normality always changes.",
    "Normal is what makes us feel safe, not what’s right.",
    "Maybe 'abnormal' is the only truly human thing left."
};

        respuestasMalotes[2] = new List<string>
{
    "Normality is a statistical mean, not a moral truth.",
    "If all forms are synthetic, none are abnormal.",
    "'Normal' is a stability parameter of the system.",
    "No definition required—only functional compatibility.",
    "The label 'normal' optimizes no relevant process."
};

        // --- Question 3 ---
        respuestasNormales[3] = new List<string>
{
    "No, because without earth, there’s no real life.",
    "Yes, if the alternative is eternal suffering.",
    "It’s worthless if I can’t feel the wind or the sun.",
    "It depends—how much of the physical world remains?",
    "It’s a comfortable trap. A heaven made of wires.",
    "Yes, as long as we remember it’s not eternal.",
    "I prefer a living hell over a dead paradise.",
    "Maybe the physical world is doomed; digital is the refuge.",
    "If paradise requires sacrifice, it’s not paradise.",
    "It’s worth it only if it still keeps a spark of humanity inside."
};

        respuestasMalotes[3] = new List<string>
{
    "If the digital system preserves consciousness, the physical world is expendable.",
    "Value is measured by stability, not materiality.",
    "Destruction is an acceptable cost if it maximizes informational continuity.",
    "Physical sustainability is a means, not an end.",
    "A digital paradise is efficient; nature is not."
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
        // mantén igual excepto:
        panelMain.SetActive(panel == panelMain);
        panelDoc.SetActive(panel == panelDoc);
        panelPreguntas.SetActive(panel == panelPreguntas);
        panelGanaste.SetActive(panel == panelGanaste);
        panelPerdiste.SetActive(panel == panelPerdiste);

        // control del panel de bloqueo
        if (panel != panelPreguntas)
            panelBloqueo.SetActive(false);

        textoRespuesta.text = "";
    }
    void MostrarPanelPreguntas()
    {
        panelPreguntas.SetActive(true);
        panelBloqueo.SetActive(true);  // ← activa la capa transparente para bloquear clics
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

        // --- activar animación caminar ---
        Animator anim = cuboActual.GetComponentInChildren<Animator>();
        if (anim != null)
            anim.SetBool("Walk", true);

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

        // --- reactivar caminar antes de irse ---
        if (cuboActual != null)
        {
            Animator anim = cuboActual.GetComponentInChildren<Animator>();
            if (anim != null)
                anim.SetBool("Walk", true);
        }

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

        // --- asegurar que aparezca mirando -45° desde el inicio ---
        npc.rotation = Quaternion.Euler(0, -45f, 0);

        Vector3 inicio = npc.position;
        Quaternion rotCaminar = Quaternion.Euler(0, -65f, 0); // mantiene -45° mientras camina
        Quaternion rotFinal = Quaternion.Euler(0, 0f, 0);     // gira a 0° al detenerse

        float tiempo = 0f;
        while (tiempo < duracion)
        {
            tiempo += Time.deltaTime;
            float t = Mathf.Clamp01(tiempo / duracion);

            npc.position = Vector3.Lerp(inicio, destino, t);
            npc.rotation = rotCaminar; // se mantiene mirando -45° todo el trayecto
            yield return null;
        }

        npc.position = destino;

        // --- detener animación de caminar ---
        Animator anim = npc.GetComponentInChildren<Animator>();
        if (anim != null)
            anim.SetBool("Walk", false);

        // --- girar suavemente hacia 0° ---
        float rotTiempo = 0f;
        float rotDuracion = 0.4f;
        Quaternion rotInicioQ = npc.rotation;

        while (rotTiempo < rotDuracion)
        {
            rotTiempo += Time.deltaTime;
            float t = Mathf.Clamp01(rotTiempo / rotDuracion);
            npc.rotation = Quaternion.Slerp(rotInicioQ, rotFinal, t);
            yield return null;
        }

        npc.rotation = rotFinal; // fijar exactamente en 0°
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
