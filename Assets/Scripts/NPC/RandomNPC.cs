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

    [Header("Materiales del cuerpo y cabeza")]
    public Renderer bodyRenderer;   // asigna Msh_Body
    public Renderer headRenderer;   // asigna Msh_Head
    public Material[] bodyMaterials; // materiales posibles para cuerpo + cabeza

    [Header("Chip")]
    public Renderer chipRenderer;   // asigna Msh_Chip
    public Material[] chipMaterials; // materiales posibles para el chip

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

        // --- Material del cuerpo y cabeza ---
        if (bodyMaterials.Length > 0 && bodyRenderer != null && headRenderer != null)
        {
            Material randomMat = bodyMaterials[Random.Range(0, bodyMaterials.Length)];
            bodyRenderer.material = randomMat;
            headRenderer.material = randomMat;
        }

        // --- Material del chip ---
        if (chipMaterials.Length > 0 && chipRenderer != null)
            chipRenderer.material = chipMaterials[Random.Range(0, chipMaterials.Length)];
    }
}
