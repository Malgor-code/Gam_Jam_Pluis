using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NPCAnimatorController : MonoBehaviour
{
    [Header("Animators de las bocas (pueden ser varios)")]
    public List<Animator> mouthAnimators = new List<Animator>();

    [Header("Duración del habla")]
    public float talkDuration = 0.6f;

    public void Hablar()
    {
        if (mouthAnimators == null || mouthAnimators.Count == 0)
        {
            Debug.LogWarning($"{name}: no hay animators de boca asignados.");
            return;
        }

        StopAllCoroutines();
        StartCoroutine(HablarRutina());
    }

    private IEnumerator HablarRutina()
    {
        // Activar Talk en todas las bocas
        foreach (Animator anim in mouthAnimators)
        {
            if (anim != null && anim.HasParameterOfType("Talk", AnimatorControllerParameterType.Bool))
                anim.SetBool("Talk", true);
        }

        yield return new WaitForSeconds(talkDuration);

        // Desactivar Talk
        foreach (Animator anim in mouthAnimators)
        {
            if (anim != null && anim.HasParameterOfType("Talk", AnimatorControllerParameterType.Bool))
                anim.SetBool("Talk", false);
        }
    }
}
