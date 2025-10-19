using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

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

    [Header("Info Text")]
    public TMP_Text infoText;

    void Start()
    {
        ShowPanel(panelMain);
        infoText.text = "Welcome. Here you'll learn how the inspection system works.";

        // --- Main actions ---
        btnReadDoc.onClick.AddListener(() =>
        {
            ShowPanel(panelDoc);
            infoText.text = "This is the document panel.\nIf the information doesn't match, it means the NPC is an AI copy.";
        });

        btnQuestions.onClick.AddListener(() =>
        {
            ShowPanel(panelQuestions);
            infoText.text = "Here you can ask the NPC three different questions to evaluate its behavior.";
        });

        btnEliminate.onClick.AddListener(() =>
        {
            ShowPanel(panelLose);
            infoText.text = "You eliminated someone. Sometimes that means you lose.";
        });

        btnSave.onClick.AddListener(() =>
        {
            ShowPanel(panelWin);
            infoText.text = "You saved this person. But saving the wrong one can also cost you later.";
        });

        // --- Return buttons ---
        btnBackDoc.onClick.AddListener(() =>
        {
            ShowPanel(panelMain);
            infoText.text = "Back to the main interface.";
        });

        btnBackQuestions.onClick.AddListener(() =>
        {
            ShowPanel(panelMain);
            infoText.text = "Back to the main interface.";
        });

        // --- Play Again buttons ---
        btnPlayAgainWin.onClick.AddListener(() =>
        {
            ShowPanel(panelMain);
            infoText.text = "You tried again. Learn from your past choices.";
        });

        btnPlayAgainLose.onClick.AddListener(() =>
        {
            ShowPanel(panelMain);
            infoText.text = "You tried again. Failure teaches best.";
        });

        // --- Continue button (next scene) ---
        btnContinue.onClick.AddListener(() =>
        {
            int nextScene = SceneManager.GetActiveScene().buildIndex + 1;
            SceneManager.LoadScene(nextScene);
        });
    }

    void ShowPanel(GameObject panel)
    {
        // Keep main panel always visible
        panelMain.SetActive(true);

        // Toggle secondary panels
        panelDoc.SetActive(panel == panelDoc);
        panelQuestions.SetActive(panel == panelQuestions);
        panelWin.SetActive(panel == panelWin);
        panelLose.SetActive(panel == panelLose);
    }
}
