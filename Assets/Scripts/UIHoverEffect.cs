using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Configuración del efecto")]
    public float scaleMultiplier = 1.05f;       // Cuánto crece el botón
    [Range(0f, 1f)] public float darkenAmount = 1f; // Qué tanto se oscurece (1 = sin cambio)
    public float transitionSpeed = 8f;          // Velocidad de la animación

    private Vector3 originalScale;
    private Color originalColor;
    private Image targetImage;
    private bool isHovered = false;

    void Start()
    {
        originalScale = transform.localScale;
        targetImage = GetComponent<Image>();

        if (targetImage != null)
            originalColor = targetImage.color;
    }

    void Update()
    {
        // Escala suave
        Vector3 targetScale = isHovered ? originalScale * scaleMultiplier : originalScale;
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * transitionSpeed);

        if (targetImage != null)
        {
            // Calculamos color sin afectar el alfa
            Color targetColor = originalColor;
            if (isHovered)
            {
                targetColor.r *= darkenAmount;
                targetColor.g *= darkenAmount;
                targetColor.b *= darkenAmount;
            }

            targetImage.color = Color.Lerp(targetImage.color, targetColor, Time.deltaTime * transitionSpeed);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
    }
}
