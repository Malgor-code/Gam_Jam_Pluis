using UnityEngine;

public class RandomNPC : MonoBehaviour
{
    [Header("Opciones")]
    public GameObject[] hairs;
    public GameObject[] accessories;
    public Renderer eyesRenderer;
    public Renderer chipRenderer;

    [Header("Colores")]
    public Color[] eyeColors;
    public Color[] chipColors;

    void Start()
    {
        RandomizeAppearance();
    }

    void RandomizeAppearance()
    {
        // Desactiva todos los modelos
        foreach (var h in hairs) h.SetActive(false);
        foreach (var a in accessories) a.SetActive(false);

        // Activa uno aleatorio de cada grupo
        if (hairs.Length > 0)
            hairs[Random.Range(0, hairs.Length)].SetActive(true);
        if (accessories.Length > 0)
            accessories[Random.Range(0, accessories.Length)].SetActive(true);

        // Colores aleatorios
        if (eyesRenderer != null && eyeColors.Length > 0)
            eyesRenderer.material.color = eyeColors[Random.Range(0, eyeColors.Length)];

        if (chipRenderer != null && chipColors.Length > 0)
            chipRenderer.material.color = chipColors[Random.Range(0, chipColors.Length)];
    }
}