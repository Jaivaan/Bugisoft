using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuController : MonoBehaviour
{
    public GameObject menuPanel;
    public Vector3 targetPosition = new Vector3(0, 5, 0);
    public float moveSpeed = 1f;
    public AudioSource movementAudioSource;

    public void StartGame()
    {
        StartCoroutine(MovePanelAndStartGame());
    }

    IEnumerator MovePanelAndStartGame()
    {
        if (movementAudioSource != null)
        {
            movementAudioSource.Play();
        }

        Vector3 startPosition = menuPanel.transform.position;
        float journeyLength = Vector3.Distance(startPosition, targetPosition);
        float startTime = Time.time;

        while (Vector3.Distance(menuPanel.transform.position, targetPosition) > 0.1f)
        {
            float distanceCovered = (Time.time - startTime) * moveSpeed;
            float fractionOfJourney = distanceCovered / journeyLength;

            menuPanel.transform.position = Vector3.Lerp(startPosition, targetPosition, fractionOfJourney);

            yield return null;
        }

        if (movementAudioSource != null)
        {
            movementAudioSource.Stop();
        }

        SceneManager.LoadScene("mainGameScene");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
