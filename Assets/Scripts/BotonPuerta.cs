using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BotonPuerta : MonoBehaviour
{
    [Header("Referencias")]
    public Animator puertaAnimator;   // Debe tener parámetro bool "Open"
    public Button botonAbrir;
    [Header("Opcional")]
    public float cooldown = 0.3f;     // Anti-spam

    void Start()
    {
        botonAbrir.onClick.AddListener(() => StartCoroutine(PulseOpen()));
    }

    IEnumerator PulseOpen()
    {
        botonAbrir.interactable = false;   // evita múltiples pulsos

        puertaAnimator.SetBool("Open", true);
        yield return null;                  // espera 1 frame para que dispare la transición
        puertaAnimator.SetBool("Open", false);

        yield return new WaitForSeconds(cooldown);
        botonAbrir.interactable = true;
    }
}
