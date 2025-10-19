using System.Collections.Generic;
using UnityEngine;

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
    public Material[] hairMaterials;

    [Header("Chip")]
    public Renderer chipRenderer;
    public Material[] chipMaterials;

    [Header("Probabilidades")]
    [Range(0f, 1f)] public float accessoryChance = 0.7f; // 70% de posibilidad de tener accesorios
    [Range(1, 5)] public int maxAccessories = 3;         // Máximo que puede usar (si hay suficientes)

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

            if (hairMaterials.Length > 0)
            {
                Material randomHairMat = hairMaterials[Random.Range(0, hairMaterials.Length)];
                Renderer[] renderers = selectedHair.GetComponentsInChildren<Renderer>(true);

                foreach (Renderer r in renderers)
                {
                    Material[] mats = r.materials;
                    for (int i = 0; i < mats.Length; i++)
                        mats[i] = randomHairMat;
                    r.materials = mats;
                }
            }
        }

        // --- Accesorios (puede no tener, uno o varios) ---
        foreach (var a in accessories) a.SetActive(false);
        if (accessories.Length > 0 && Random.value < accessoryChance)
        {
            int cantidad = Random.Range(1, Mathf.Min(maxAccessories, accessories.Length) + 1);
            List<int> indices = new List<int>();
            for (int i = 0; i < accessories.Length; i++) indices.Add(i);

            for (int i = 0; i < cantidad; i++)
            {
                if (indices.Count == 0) break;
                int idx = Random.Range(0, indices.Count);
                accessories[indices[idx]].SetActive(true);
                indices.RemoveAt(idx);
            }
        }

        // --- Ojos ---
        foreach (var e in eyes) e.SetActive(false);
        if (eyes.Length > 0)
            eyes[Random.Range(0, eyes.Length)].SetActive(true);

        // --- Boca ---
        foreach (var m in mouths) m.SetActive(false);
        if (mouths.Length > 0)
            mouths[Random.Range(0, mouths.Length)].SetActive(true);

        // --- Cuerpo + cabeza ---
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
