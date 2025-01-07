using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuController : MonoBehaviour
{
    public GameObject menuPanel; // Panel del men� que ser� movido
    public Vector3 targetPosition = new Vector3(0, 5, 0); // Posici�n objetivo a la que se mover� el panel
    public float moveSpeed = 1f; // Velocidad de movimiento del panel
    public AudioSource movementAudioSource; // Fuente de audio que reproduce un sonido mientras el panel se mueve

    // M�todo para iniciar el juego
    public void StartGame()
    {
        StartCoroutine(MovePanelAndStartGame()); // Inicia la rutina de mover el panel y cargar la escena del juego
    }

    IEnumerator MovePanelAndStartGame()
    {
        // Reproduce el sonido de movimiento si est� asignado
        if (movementAudioSource != null)
        {
            movementAudioSource.Play();
        }

        Vector3 startPosition = menuPanel.transform.position; // Posici�n inicial del panel
        float journeyLength = Vector3.Distance(startPosition, targetPosition); // Distancia total del recorrido
        float startTime = Time.time; // Tiempo inicial para calcular la interpolaci�n

        // Mueve el panel hacia la posici�n objetivo de manera suave
        while (Vector3.Distance(menuPanel.transform.position, targetPosition) > 0.1f)
        {
            float distanceCovered = (Time.time - startTime) * moveSpeed; // Distancia recorrida hasta el momento
            float fractionOfJourney = distanceCovered / journeyLength; // Fracci�n completada del recorrido

            // Interpola la posici�n del panel hacia el objetivo
            menuPanel.transform.position = Vector3.Lerp(startPosition, targetPosition, fractionOfJourney);

            yield return null; // Espera al siguiente frame antes de continuar
        }

        // Detiene el sonido de movimiento si estaba siendo reproducido
        if (movementAudioSource != null)
        {
            movementAudioSource.Stop();
        }

        // Carga la escena principal del juego
        SceneManager.LoadScene("mainGameScene");
    }

    // M�todo para salir del juego
    public void ExitGame()
    {
        Application.Quit(); // Cierra la aplicaci�n
    }
}
