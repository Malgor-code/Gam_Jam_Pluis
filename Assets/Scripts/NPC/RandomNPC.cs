using UnityEngine;
using System.Collections.Generic;

public class RandomNPC : MonoBehaviour
{
    [Header("Modelos 3D")]
    public GameObject[] hairs;
    public GameObject[] accessories;
    public GameObject[] eyes;
    public GameObject[] mouths;

    [Header("Materiales del cuerpo y cabeza")]
    public Renderer bodyRenderer;
    public Renderer headRenderer;
    public Material[] bodyMaterials;

    [Header("Materiales del cabello")]
    public Material[] hairMaterials; // materiales posibles para el pelo

    [Header("Chip")]
    public Renderer chipRenderer;
    public Material[] chipMaterials;

    // --- registro de materiales usados ---
    private static List<Material> usedMaterials = new List<Material>();

    void Start()
    {
        RandomizeAppearance();
    }

    void RandomizeAppearance()
    {
        // --- Pelo ---
        GameObject selectedHair = null;
        foreach (var h in hairs) h.SetActive(false);
        if (hairs.Length > 0)
        {
            selectedHair = hairs[Random.Range(0, hairs.Length)];
            selectedHair.SetActive(true);

            // aplicar material aleatorio si tiene Renderer
            if (hairMaterials.Length > 0)
            {
                Renderer hairRenderer = selectedHair.GetComponentInChildren<Renderer>();
                if (hairRenderer != null)
                    hairRenderer.material = hairMaterials[Random.Range(0, hairMaterials.Length)];
            }
        }

        // --- Accesorios ---
        foreach (var a in accessories) a.SetActive(false);
        if (accessories.Length > 0)
            accessories[Random.Range(0, accessories.Length)].SetActive(true);

        // --- Ojos ---
        foreach (var e in eyes) e.SetActive(false);
        if (eyes.Length > 0)
            eyes[Random.Range(0, eyes.Length)].SetActive(true);

        // --- Boca ---
        foreach (var m in mouths) m.SetActive(false);
        if (mouths.Length > 0)
            mouths[Random.Range(0, mouths.Length)].SetActive(true);

        // --- Material cuerpo + cabeza (sin repetir entre NPCs) ---
        if (bodyMaterials.Length > 0 && bodyRenderer != null && headRenderer != null)
        {
            List<Material> disponibles = new List<Material>(bodyMaterials);
            disponibles.RemoveAll(mat => usedMaterials.Contains(mat));

            if (disponibles.Count == 0)
            {
                usedMaterials.Clear();
                disponibles = new List<Material>(bodyMaterials);
            }

            Material randomMat = disponibles[Random.Range(0, disponibles.Count)];
            usedMaterials.Add(randomMat);

            bodyRenderer.material = randomMat;
            headRenderer.material = randomMat;
        }

        // --- Chip ---
        if (chipMaterials.Length > 0 && chipRenderer != null)
            chipRenderer.material = chipMaterials[Random.Range(0, chipMaterials.Length)];
    }
}
