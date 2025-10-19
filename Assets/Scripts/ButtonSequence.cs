using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class ButtonSequenceFinal_NoZoomText : MonoBehaviour
{
    [Header("Botones principales")]
    public Button[] mainButtons;
    [Header("Botón final")]
    public Button finalButton;
    [Header("Pantalla negra")]
    public Image blackScreen;
    [Header("Configuración de tiempos")]
    public float fadeInTime = 1f;
    public float fadeOutTime = 1f;
    public float delayBetweenButtons = 0.3f;
    public float blackScreenFadeTime = 1f;
    public float delayBeforeBlackAfterFinal = 3f; // tiempo antes de aparecer pantalla negra

    void Start()
    {
        // Inicialmente ocultar todo
        foreach (Button btn in mainButtons)
            SetButtonAlpha(btn, 0f);

        SetButtonAlpha(finalButton, 0f);

        if (blackScreen)
        {
            Color c = blackScreen.color;
            c.a = 0f;
            blackScreen.color = c;
        }

        StartCoroutine(Sequence());
    }

    IEnumerator Sequence()
    {
        // Fade In de los botones principales
        foreach (Button btn in mainButtons)
        {
            yield return StartCoroutine(FadeButton(btn, 0f, 1f, fadeInTime));
            yield return new WaitForSeconds(delayBetweenButtons);
        }

        yield return new WaitForSeconds(0.5f);

        // Fade Out de los botones principales
        foreach (Button btn in mainButtons)
        {
            yield return StartCoroutine(FadeButton(btn, 1f, 0f, fadeOutTime));
        }

        // Fade In del botón final (sin zoom)
        yield return StartCoroutine(FadeButton(finalButton, 0f, 1f, fadeInTime));

        // Esperar antes de mostrar la pantalla negra
        yield return new WaitForSeconds(delayBeforeBlackAfterFinal);

        // Fade In de la pantalla negra
        yield return StartCoroutine(FadeImage(blackScreen, 0f, 1f, blackScreenFadeTime));
    }

    IEnumerator FadeButton(Button btn, float from, float to, float time)
    {
        float t = 0f;
        SetButtonAlpha(btn, from);
        while (t < 1f)
        {
            t += Time.deltaTime / time;
            float alpha = Mathf.Lerp(from, to, t);
            SetButtonAlpha(btn, alpha);
            yield return null;
        }
        SetButtonAlpha(btn, to);
    }

    void SetButtonAlpha(Button btn, float alpha)
    {
        // Fade imagen del botón
        Image img = btn.GetComponent<Image>();
        if (img) img.color = new Color(img.color.r, img.color.g, img.color.b, alpha);

        // Fade de Text normal
        Text txt = btn.GetComponentInChildren<Text>();
        if (txt) txt.color = new Color(txt.color.r, txt.color.g, txt.color.b, alpha);

        // Fade de TextMeshProUGUI
        TextMeshProUGUI tmp = btn.GetComponentInChildren<TextMeshProUGUI>();
        if (tmp) tmp.alpha = alpha;
    }

    IEnumerator FadeImage(Image img, float from, float to, float time)
    {
        float t = 0f;
        Color c = img.color;
        while (t < 1f)
        {
            t += Time.deltaTime / time;
            c.a = Mathf.Lerp(from, to, t);
            img.color = c;
            yield return null;
        }
        c.a = to;
        img.color = c;
    }
}
