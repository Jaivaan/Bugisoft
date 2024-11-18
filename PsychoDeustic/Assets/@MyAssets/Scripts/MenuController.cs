using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuController : MonoBehaviour
{
    public GameObject menuPanel;  // Asigna aquí el panel que se moverá
    public Vector3 targetPosition = new Vector3(0, 5, 0);  // La posición a la que se moverá el panel (ajústala según sea necesario)
    public float moveSpeed = 1f;  // Velocidad de movimiento del panel

    public void StartGame()
    {
        // Llama a la corrutina para mover el panel antes de cambiar de escena
        StartCoroutine(MovePanelAndStartGame());
    }

    IEnumerator MovePanelAndStartGame()
    {
        // Mueve el panel hacia arriba poco a poco
        Vector3 startPosition = menuPanel.transform.position;
        float journeyLength = Vector3.Distance(startPosition, targetPosition);
        float startTime = Time.time;

        while (Vector3.Distance(menuPanel.transform.position, targetPosition) > 0.1f)
        {
            float distanceCovered = (Time.time - startTime) * moveSpeed;
            float fractionOfJourney = distanceCovered / journeyLength;

            menuPanel.transform.position = Vector3.Lerp(startPosition, targetPosition, fractionOfJourney);

            yield return null;  // Espera un frame
        }

        // Una vez que se haya movido el panel, cambia de escena
        SceneManager.LoadScene("SampleScene");  // Asegúrate de tener la escena correctamente configurada
    }

    public void ExitGame()
    {
        // Cierra la aplicación
        Application.Quit();
    }
}
