using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class DocsPanelController : MonoBehaviour
{
    [Header("Referencias de imágenes")]
    public Image leftCredentialImage;
    public Image rightCredentialImage;
    public Sprite[] leftSprites;   // sprites posibles para la credencial izquierda
    public Sprite[] rightSprites;  // sprites posibles para la credencial derecha

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
        if (leftSprites == null || leftSprites.Length == 0 ||
            rightSprites == null || rightSprites.Length == 0)
        {
            Debug.LogError("DocsPanelController: faltan sprites asignados.");
            return;
        }

        // --- Datos base ---
        string id = GenerarID();
        string occupation = occupations[Random.Range(0, occupations.Length)];
        string district = districts[Random.Range(0, districts.Length)];

        // --- Sprites (siempre diferentes entre izquierda y derecha) ---
        Sprite leftSprite = leftSprites[Random.Range(0, leftSprites.Length)];
        Sprite rightSprite = rightSprites[Random.Range(0, rightSprites.Length)];

        leftCredentialImage.sprite = leftSprite;
        rightCredentialImage.sprite = rightSprite;

        // --- Llenar textos (izquierda) ---
        leftIDText.text = id;
        leftOccupationText.text = occupation;
        leftDistrictText.text = district;

        // --- Copiar textos a la derecha ---
        rightIDText.text = id;
        rightOccupationText.text = occupation;
        rightDistrictText.text = district;

        // --- Si es malote, aplicar dos errores distintos ---
        if (esMalote)
        {
            List<int> posiblesErrores = new List<int> { 0, 1, 2, 3 }; // 0=ID, 1=Occupation, 2=District, 3=Sprite
            int error1 = posiblesErrores[Random.Range(0, posiblesErrores.Count)];
            posiblesErrores.Remove(error1);
            int error2 = posiblesErrores[Random.Range(0, posiblesErrores.Count)];

            AplicarError(error1);
            AplicarError(error2);
        }
    }

    void AplicarError(int tipoError)
    {
        switch (tipoError)
        {
            case 0: // ID diferente
                rightIDText.text = GenerarID();
                break;

            case 1: // Ocupación diferente
                rightOccupationText.text = occupations[Random.Range(0, occupations.Length)];
                break;

            case 2: // Distrito diferente
                rightDistrictText.text = districts[Random.Range(0, districts.Length)];
                break;

            case 3: // Sprite especial (segundo elemento)
                if (rightSprites.Length > 1)
                    rightCredentialImage.sprite = rightSprites[1];
                break;
        }
    }

    string GenerarID()
    {
        string id = "";
        for (int i = 0; i < 8; i++)
            id += Random.Range(0, 10).ToString();
        return id;
    }
}
