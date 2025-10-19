using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class DocsPanelController : MonoBehaviour
{
    [Header("Referencias de imágenes")]
    public Image leftCredentialImage;
    public Image rightCredentialImage;

    [Header("Sprites")]
    public Sprite[] leftSprites;          // Sprites posibles para la credencial izquierda
    public Sprite[] rightNormalSprites;   // Sprites que usan NPC normales (solo uno)
    public Sprite[] rightMaloteSprites;   // Sprites que pueden usar malotes (1 o 2)

    [Header("Texto izquierdo (Documento 1)")]
    public TMP_Text leftIDText;
    public TMP_Text leftOccupationText;
    public TMP_Text leftDistrictText;

    [Header("Texto derecho (Documento 2)")]
    public TMP_Text rightIDText;
    public TMP_Text rightOccupationText;
    public TMP_Text rightDistrictText;

    [Header("Configuración de datos")]
    public string[] occupations = new string[]
    {
        "Engineer", "Carpenter", "Doctor", "Teacher", "Chef",
        "Mechanic", "Accountant", "Police Officer", "Electrician", "Lawyer"
    };

    public string[] districts = new string[]
    {
        "District 1 - Arbor Hills",
        "District 2 - Iron Valley",
        "District 3 - Sunridge",
        "District 4 - Oceanview",
        "District 5 - Maplewood",
        "District 6 - Pinecrest",
        "District 7 - Rosefield",
        "District 8 - Stonebridge",
        "District 9 - Willowpark",
        "District 10 - Clearwater"
    };

    public void GenerarDocumentos(bool esMalote)
    {
        if (leftSprites == null || leftSprites.Length == 0)
        {
            Debug.LogError("DocsPanelController: faltan sprites izquierdos.");
            return;
        }

        // --- Datos base ---
        string id = GenerarID();
        string occupation = occupations[Random.Range(0, occupations.Length)];
        string district = districts[Random.Range(0, districts.Length)];

        // --- Sprite izquierdo ---
        Sprite leftSprite = leftSprites[Random.Range(0, leftSprites.Length)];
        leftCredentialImage.sprite = leftSprite;

        // --- Sprite derecho según tipo ---
        if (!esMalote)
        {
            if (rightNormalSprites.Length == 0)
            {
                Debug.LogError("DocsPanelController: faltan sprites normales derechos.");
                return;
            }
            rightCredentialImage.sprite = rightNormalSprites[Random.Range(0, rightNormalSprites.Length)];
        }
        else
        {
            if (rightMaloteSprites.Length == 0)
            {
                Debug.LogError("DocsPanelController: faltan sprites de malotes derechos.");
                return;
            }

            // Los malotes pueden usar 1 o 2 sprites (decide aleatoriamente)
            int cantidadSprites = Random.Range(1, 3); // 1 o 2
            if (cantidadSprites == 1)
            {
                rightCredentialImage.sprite = rightMaloteSprites[Random.Range(0, rightMaloteSprites.Length)];
            }
            else
            {
                // Si usa 2, mezcla dos sprites visualmente distintos (solo ejemplo visual)
                Sprite baseSprite = rightMaloteSprites[Random.Range(0, rightMaloteSprites.Length)];
                Sprite overlaySprite = rightMaloteSprites[Random.Range(0, rightMaloteSprites.Length)];
                while (overlaySprite == baseSprite && rightMaloteSprites.Length > 1)
                    overlaySprite = rightMaloteSprites[Random.Range(0, rightMaloteSprites.Length)];

                // Muestra el sprite base (puedes modificar para combinar por UI)
                rightCredentialImage.sprite = baseSprite;
                Debug.Log($"Malote con 2 sprites: {baseSprite.name} + {overlaySprite.name}");
            }
        }

        // --- Llenar textos (izquierda) ---
        leftIDText.text = id;
        leftOccupationText.text = occupation;
        leftDistrictText.text = district;

        // --- Copiar textos a la derecha ---
        rightIDText.text = id;
        rightOccupationText.text = occupation;
        rightDistrictText.text = district;

        // --- Si es malote, aplicar errores ---
        if (esMalote)
        {
            List<int> posiblesErrores = new List<int> { 0, 1, 2, 3 }; // 0=ID, 1=Occupation, 2=District, 3=Sprite extra
            int error1 = posiblesErrores[Random.Range(0, posiblesErrores.Count)];
            posiblesErrores.Remove(error1);
            int error2 = posiblesErrores[Random.Range(0, posiblesErrores.Count)];

            AplicarError(error1, id);
            AplicarError(error2, id);
        }
    }

    void AplicarError(int tipoError, string idBase)
    {
        switch (tipoError)
        {
            case 0: // ID ligeramente diferente
                rightIDText.text = GenerarIDFalsa(idBase);
                break;

            case 1: // Ocupación diferente
                rightOccupationText.text = occupations[Random.Range(0, occupations.Length)];
                break;

            case 2: // Distrito diferente
                rightDistrictText.text = districts[Random.Range(0, districts.Length)];
                break;

            case 3: // Cambia sprite a otro malote
                if (rightMaloteSprites.Length > 1)
                    rightCredentialImage.sprite = rightMaloteSprites[Random.Range(0, rightMaloteSprites.Length)];
                break;
        }
    }

    string GenerarID()
    {
        string id = "";
        for (int i = 0; i < 9; i++)
            id += Random.Range(0, 10).ToString();
        return id;
    }

    string GenerarIDFalsa(string idReal)
    {
        char[] digitos = idReal.ToCharArray();
        int cantidadCambios = Random.Range(1, 3); // 1 o 2 cambios

        HashSet<int> indicesCambiados = new HashSet<int>();
        while (indicesCambiados.Count < cantidadCambios)
        {
            int i = Random.Range(0, digitos.Length);
            if (char.IsDigit(digitos[i]))
                indicesCambiados.Add(i);
        }

        foreach (int i in indicesCambiados)
        {
            char nuevo;
            do
            {
                nuevo = (char)('0' + Random.Range(0, 10));
            } while (nuevo == digitos[i]);
            digitos[i] = nuevo;
        }

        return new string(digitos);
    }
}
