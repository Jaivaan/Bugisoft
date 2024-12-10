using UnityEngine;

public class RoundManager : MonoBehaviour
{
    public Material[] materials;
    public GameObject roundPlane;
    public ButtonController[] playerButtons;

    private int previousMaterialIndex = -1;

    void Start()
    {
        if (roundPlane == null || materials.Length < 2 || playerButtons.Length == 0)
        {
            Debug.LogError("Faltan referencias en RoundManager o no hay suficientes materiales.");
            return;
        }

        ToggleMaterial(true);
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

    private void ToggleMaterial(bool isFirstTime = false)
    {
        if (roundPlane != null)
        {
            int newMaterialIndex;

            do
            {
                newMaterialIndex = Random.Range(0, materials.Length);
            }
            while (!isFirstTime && newMaterialIndex == previousMaterialIndex);

            roundPlane.GetComponent<Renderer>().material = materials[newMaterialIndex];
            previousMaterialIndex = newMaterialIndex;

            Debug.Log("Material cambiado a: " + newMaterialIndex);
        }
    }
}
