using UnityEngine;

public class RoundManager : MonoBehaviour
{
    public Renderer planeRenderer; 
    public Material[] roundMaterials; 
    public GameObject[] physicalButtons; 
    private int currentRound = 0;

    private void Start()
    {
        UpdateRoundMaterial();
    }

    public void ButtonPressed(GameObject button)
    {
        if (System.Array.Exists(physicalButtons, b => b == button))
        {
            currentRound = (currentRound + 1) % roundMaterials.Length;
            UpdateRoundMaterial();
        }
    }

    private void UpdateRoundMaterial()
    {
        if (planeRenderer != null && roundMaterials.Length > 0)
        {
            planeRenderer.material = roundMaterials[currentRound];
            Debug.Log("Cambiando a la ronda: " + currentRound);
        }
    }
}
