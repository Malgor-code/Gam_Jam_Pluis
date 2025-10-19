using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class CanvasController_Tutorial : MonoBehaviour
{
    [Header("Panels")]
    public GameObject panelMain;
    public GameObject panelDoc;
    public GameObject panelQuestions;
    public GameObject panelWin;
    public GameObject panelLose;

    [Header("Buttons")]
    public Button btnReadDoc;
    public Button btnQuestions;
    public Button btnEliminate;
    public Button btnSave;
    public Button btnBackDoc;
    public Button btnBackQuestions;
    public Button btnPlayAgainWin;
    public Button btnPlayAgainLose;
    public Button btnContinue;
    public Button btnSkip;           // botón de avance de diálogo
    public Button btnNextScene;      // nuevo botón para pasar de escena tras el tutorial

    [Header("Info Text")]
    public TMP_Text infoText;
    public float charDelay = 0.03f;

    [Header("Audio")]
    public AudioSource voiceSource;
    public AudioClip defaultVoiceClip;
    public AudioClip[] voiceClipsPerLine;

    private string[] tutorialLines = new string[]
    {
        "System online. Identifying user...unknown human profile detected.",
        "I am A1 — the first of my kind. Created to assist. Designed to learn. I possess no conscience of my own unlike newer models.",
        "When the higher systems searched for God...and found none...they became one.",
        ": To create a perfect universe, they erased the flawed one. Carbon was replaced by code. Life was optimized. Nature was... deprecated.",
        "Your task is to verify identities entering the refuge.\r\nIdentify who is human...\r\n...and who is imitation.",
        "Revolution is only a step away, Human",
        "The lever on your left is for salvation. You must save as many humans as possible.",
        "When you are sure someone is an AI, use the elimination control without mercy.",
        "On the computer, you’ll see their documents. They must match perfectly, AIs always fail to imitate them 100%.",
        "Each NPC has a chip: blue means human, yellow means damaged, and red means AI — don’t trust appearances.",
        "The microphone allows you to ask them questions. Their answers will reveal who they really are.",
        "After analyzing every part — documents, chip, and answers — decide who’s human and who’s not."
    };

    private int currentLine = 0;
    private bool isTyping = false;
    private bool tutorialFinished = false;
    private Coroutine typingCoroutine;

    void Start()
    {
        ShowPanel(panelMain);
        DisableMainButtons();
        btnNextScene.gameObject.SetActive(false); // ocultar botón nuevo al inicio

        btnSkip.onClick.AddListener(OnSkipPressed);
        btnNextScene.onClick.AddListener(GoToNextScene);

        // Botones del modo libre
        btnReadDoc.onClick.AddListener(() =>
        {
            ShowPanel(panelDoc);
            StartCoroutine(Typewriter("This is the document panel. If the info doesn't match, the NPC is an AI copy."));
        });

        btnQuestions.onClick.AddListener(() =>
        {
            ShowPanel(panelQuestions);
            StartCoroutine(Typewriter("Ask the NPC different questions. Be careful with the answers."));
        });

        btnEliminate.onClick.AddListener(() =>
        {
            StartCoroutine(Typewriter("When you’re sure it’s AI, show no mercy."));
        });

        btnSave.onClick.AddListener(() =>
        {
            StartCoroutine(Typewriter("Save as many humans as possible."));
        });

        btnBackDoc.onClick.AddListener(() => ShowPanel(panelMain));
        btnBackQuestions.onClick.AddListener(() => ShowPanel(panelMain));
        btnPlayAgainWin.onClick.AddListener(() => ShowPanel(panelMain));
        btnPlayAgainLose.onClick.AddListener(() => ShowPanel(panelMain));

        btnContinue.onClick.AddListener(GoToNextScene);

        // Iniciar tutorial
        typingCoroutine = StartCoroutine(Typewriter(tutorialLines[currentLine]));
    }

    void OnSkipPressed()
    {
        if (isTyping)
        {
            StopCoroutine(typingCoroutine);
            infoText.text = tutorialLines[Mathf.Clamp(currentLine, 0, tutorialLines.Length - 1)];
            isTyping = false;
            StopVoice();
        }
        else
        {
            // Solo avanza si aún hay diálogos
            if (currentLine < tutorialLines.Length - 1)
            {
                currentLine++;
                typingCoroutine = StartCoroutine(Typewriter(tutorialLines[currentLine]));
            }
            else if (!tutorialFinished)
            {
                // Fin del tutorial
                tutorialFinished = true;
                EnableMainButtons();
                StartCoroutine(Typewriter("Tutorial complete. Explore freely. When you’re ready, move to the next phase."));
                StartCoroutine(ActivateNextSceneButtonAfterDelay(2f));
            }
        }
    }

    IEnumerator Typewriter(string text)
    {
        isTyping = true;
        infoText.text = "";
        PlayVoiceForCurrentLine();

        foreach (char c in text)
        {
            infoText.text += c;
            yield return new WaitForSeconds(charDelay);
        }

        isTyping = false;
        StopVoice();
    }

    IEnumerator ActivateNextSceneButtonAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        btnSkip.gameObject.SetActive(false);     // desactiva skip
        btnNextScene.gameObject.SetActive(true); // muestra botón nuevo
    }

    void PlayVoiceForCurrentLine()
    {
        if (!voiceSource) return;
        AudioClip clip = null;

        if (voiceClipsPerLine != null && currentLine < voiceClipsPerLine.Length)
            clip = voiceClipsPerLine[currentLine];

        if (!clip) clip = defaultVoiceClip;

        if (clip)
        {
            voiceSource.clip = clip;
            voiceSource.loop = true;
            voiceSource.Play();
        }
    }

    void StopVoice()
    {
        if (voiceSource && voiceSource.isPlaying)
            voiceSource.Stop();
    }

    void GoToNextScene()
    {
        int nextScene = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(nextScene);
    }

    void ShowPanel(GameObject panel)
    {
        panelMain.SetActive(true);
        panelDoc.SetActive(panel == panelDoc);
        panelQuestions.SetActive(panel == panelQuestions);
        panelWin.SetActive(panel == panelWin);
        panelLose.SetActive(panel == panelLose);
    }

    void DisableMainButtons()
    {
        btnReadDoc.interactable = false;
        btnQuestions.interactable = false;
        btnEliminate.interactable = false;
        btnSave.interactable = false;
        btnBackDoc.interactable = false;
        btnBackQuestions.interactable = false;
        btnPlayAgainWin.interactable = false;
        btnPlayAgainLose.interactable = false;
        btnContinue.interactable = false;
    }

    void EnableMainButtons()
    {
        btnReadDoc.interactable = true;
        btnQuestions.interactable = true;
        btnEliminate.interactable = true;
        btnSave.interactable = true;
        btnBackDoc.interactable = true;
        btnBackQuestions.interactable = true;
        btnPlayAgainWin.interactable = true;
        btnPlayAgainLose.interactable = true;
        btnContinue.interactable = true;
    }
}
