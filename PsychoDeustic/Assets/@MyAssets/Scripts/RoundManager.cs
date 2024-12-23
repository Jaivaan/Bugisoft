using UnityEditor;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    public Material[] materials;
    public GameObject roundPlane;
    public ButtonController[] playerButtons;

    private int materialIndex = 0;

    void Start()
    {
        if (roundPlane == null || materials.Length < 2 || playerButtons.Length == 0)
        {
            Debug.LogError("Faltan referencias en RoundManager o no hay suficientes materiales.");
            return;
        }

        ToggleMaterial();
    }
    void Update()
    {
        foreach (ButtonController button in playerButtons)
        {
            if (!button.isClickable)
            {
                ToggleMaterial();
                button.isClickable = true;
                return;
            }
        }
    }



    private void ToggleMaterial()
    {
        if (roundPlane != null && materials.Length > 0)
        {
            if (materialIndex >= materials.Length)
            {
                materialIndex = 0; 
            }

            roundPlane.GetComponent<Renderer>().material = materials[materialIndex];
            Debug.Log("Material cambiado a: " + materialIndex);
            materialIndex++; 
        }
    }
}
