using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [Header("Efecto de Hover")]
    public float hoverScaleMultiplier = 1.05f;
    [Range(0f, 1f)] public float darkenAmount = 0.85f;
    public float transitionSpeed = 8f;

    [Header("Efecto de Click")]
    public float clickScaleMultiplier = 0.9f;      // Cuánto se encoge al presionar
    public float clickBounceSpeed = 12f;           // Qué tan rápido rebota

    private Vector3 originalScale;
    private Color originalColor;
    private Image targetImage;
    private bool isHovered = false;
    private bool isClicked = false;

    void Start()
    {
        originalScale = transform.localScale;
        targetImage = GetComponent<Image>();

        if (targetImage != null)
            originalColor = targetImage.color;
    }

    void Update()
    {
        // Determinar escala objetivo según hover y click
        Vector3 targetScale = originalScale;
        if (isClicked)
            targetScale = originalScale * clickScaleMultiplier;
        else if (isHovered)
            targetScale = originalScale * hoverScaleMultiplier;

        // Suavizar movimiento
        float speed = isClicked ? clickBounceSpeed : transitionSpeed;
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * speed);

        // Oscurecer sin afectar el alpha
        if (targetImage != null)
        {
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

    public void OnPointerDown(PointerEventData eventData)
    {
        isClicked = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isClicked = false;
    }
}
