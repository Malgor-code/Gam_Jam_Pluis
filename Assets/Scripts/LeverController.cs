using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class LeverController : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [Header("Configuración")]
    public RectTransform handle;         // Mango que se mueve
    public float minY = -50f;             // Límite inferior
    public float maxY = 50f;              // Límite superior
    public float activationThreshold = 40f; // Valor para considerar “activado”

    [Header("Eventos")]
    public UnityEvent OnLeverUp;          // Se activa cuando subes lo suficiente
    public UnityEvent OnLeverDown;        // Se activa cuando bajas lo suficiente

    private Vector2 startPos;
    private bool isDragging = false;

    void Start()
    {
        if (handle == null)
            handle = GetComponent<RectTransform>();

        startPos = handle.anchoredPosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isDragging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging) return;

        Vector2 localMouse;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform)handle.parent,
            eventData.position,
            eventData.pressEventCamera,
            out localMouse
        );

        // Mover solo en eje Y
        float newY = Mathf.Clamp(localMouse.y, minY, maxY);
        handle.anchoredPosition = new Vector2(startPos.x, newY);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;

        float y = handle.anchoredPosition.y;

        if (y >= activationThreshold)
        {
            OnLeverUp?.Invoke(); // Palanca subida
        }
        else if (y <= -activationThreshold)
        {
            OnLeverDown?.Invoke(); // Palanca bajada
        }

        // Regresa a su posición inicial
        handle.anchoredPosition = startPos;
    }
}
