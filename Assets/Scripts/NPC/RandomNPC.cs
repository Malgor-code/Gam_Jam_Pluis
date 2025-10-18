using UnityEngine;

public class RandomNPC : MonoBehaviour
{
    [Header("Modelos 3D")]
    public GameObject[] hairs;
    public GameObject[] accessories;

    [Header("Sprites 2D")]
    public SpriteRenderer eyes;
    public SpriteRenderer mouth;
    public Sprite[] eyeSprites;
    public Sprite[] mouthSprites;

    [Header("Materiales y colores")]
    public Renderer bodyRenderer;
    public Color[] skinColors;
    public Color[] shirtColors;

    [Header("Chip")]
    public Renderer chipRenderer;
    public Color[] chipColors;

    void Start()
    {
        RandomizeAppearance();
    }

    void RandomizeAppearance()
    {
        // --- Pelo ---
        foreach (var h in hairs) h.SetActive(false);
        if (hairs.Length > 0)
            hairs[Random.Range(0, hairs.Length)].SetActive(true);

        // --- Accesorios ---
        foreach (var a in accessories) a.SetActive(false);
        if (accessories.Length > 0)
            accessories[Random.Range(0, accessories.Length)].SetActive(true);

        // --- Rostro ---
        if (eyes != null && eyeSprites.Length > 0)
            eyes.sprite = eyeSprites[Random.Range(0, eyeSprites.Length)];

        if (mouth != null && mouthSprites.Length > 0)
            mouth.sprite = mouthSprites[Random.Range(0, mouthSprites.Length)];

        // --- Material del cuerpo ---
        if (bodyRenderer != null)
        {
            Material mat = bodyRenderer.material;
            mat.SetColor("_SkinColor", skinColors[Random.Range(0, skinColors.Length)]);
            mat.SetColor("_ShirtColor", shirtColors[Random.Range(0, shirtColors.Length)]);
        }

        // --- Chip ---
        if (chipRenderer != null && chipColors.Length > 0)
            chipRenderer.material.color = chipColors[Random.Range(0, chipColors.Length)];
    }
}
