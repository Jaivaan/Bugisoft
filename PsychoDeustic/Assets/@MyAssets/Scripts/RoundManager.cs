using UnityEditor;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    public Material[] materials; // Array de materiales para cambiar entre rondas
    public GameObject roundPlane; // Objeto que representará la "pantalla" o indicador de la ronda
    public ButtonController[] playerButtons; // Botones del jugador que se controlan en la ronda

    private int materialIndex = 0; // Índice actual del material que se está utilizando

    void Start()
    {
        // Verifica si se han asignado las referencias necesarias
        if (roundPlane == null || materials.Length < 2 || playerButtons.Length == 0)
        {
            Debug.LogError("Faltan referencias en RoundManager o no hay suficientes materiales.");
            return;
        }

        // Inicializa el material del plano de la ronda
        ToggleMaterial();
    }

    void Update()
    {
        // Recorre los botones del jugador
        foreach (ButtonController button in playerButtons)
        {
            // Si un botón no es clickeable, cambia el material y reinicia su estado
            if (!button.isClickable)
            {
                ToggleMaterial(); // Cambia al siguiente material
                button.isClickable = true; // Reactiva el botón para que pueda ser clickeado nuevamente
                return;
            }
        }
    }

    private void ToggleMaterial()
    {
        // Cambia el material del objeto asignado (roundPlane)
        if (roundPlane != null && materials.Length > 0)
        {
            // Si el índice excede el número de materiales, vuelve al primero
            if (materialIndex >= materials.Length)
            {
                materialIndex = 0;
            }

            // Asigna el material actual al objeto
            roundPlane.GetComponent<Renderer>().material = materials[materialIndex];
            Debug.Log("Material cambiado a: " + materialIndex);

            // Incrementa el índice para el próximo cambio
            materialIndex++;
        }
    }
}
