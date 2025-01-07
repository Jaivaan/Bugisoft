using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // Instancia singleton del GameManager

    public ButtonController[] playerButtons; // Botones del jugador
    public ButtonController[] enemyButtons; // Botones del enemigo

    public TMP_Text deathMessage; // Mensaje de muerte que se muestra en pantalla

    public ButtonController playerDeathButton; // Bot�n de muerte del jugador
    public ButtonController enemyDeathButton; // Bot�n de muerte del enemigo

    public AudioClip deathSound; // Sonido que se reproduce al morir
    private AudioSource audioSource; // Fuente de audio para reproducir el sonido

    public GameObject enemy; // Objeto del enemigo
    private Animator enemyAnimator; // Animator para controlar las animaciones del enemigo
    public float electrocutedDuration = 4f; // Duraci�n de la animaci�n de electrocutado

    public GameObject rayoPrefab; // Prefab del rayo que aparece al electrocutar
    public Transform rayoSpawnPoint; // Punto donde aparece el rayo

    public GameObject deadEffect; // Efecto visual de muerte del jugador

    private int roundCounter = 0; // Contador de rondas
    private bool lastDeathWasEnemy; // Indica si la �ltima muerte fue del enemigo

    private int playerDeathCount = 0; // Cantidad de muertes del jugador
    private int enemyDeathCount = 0; // Cantidad de muertes del enemigo

    private void Awake()
    {
        // Configuraci�n del singleton
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        audioSource = GetComponent<AudioSource>();

        // Configuraci�n inicial del enemigo
        if (enemy != null)
        {
            enemyAnimator = enemy.GetComponent<Animator>();
            if (enemyAnimator == null)
            {
                Debug.LogError("No se encontr� el Animator en el enemigo.");
            }
        }
        else
        {
            Debug.LogError("El GameObject del enemigo no est� asignado.");
        }

        if (deathMessage != null)
            deathMessage.gameObject.SetActive(false);

        if (deadEffect != null)
            deadEffect.SetActive(false);

        ResetButtons();
    }

    public void CheckIfDeathButton(ButtonController clickedButton)
    {
        // Verifica si se presion� el bot�n de muerte
        if (clickedButton.isEnemyButton)
        {
            if (clickedButton == enemyDeathButton)
            {
                audioSource.PlayOneShot(deathSound); // Reproduce el sonido de muerte
                StartCoroutine(DeathSequence(true)); // Inicia la secuencia de muerte para el enemigo
            }
        }
        else
        {
            if (clickedButton == playerDeathButton)
            {
                audioSource.PlayOneShot(deathSound); // Reproduce el sonido de muerte
                StartCoroutine(DeathSequence(false)); // Inicia la secuencia de muerte para el jugador
            }
        }
    }

    private IEnumerator DeathSequence(bool isEnemy)
    {
        lastDeathWasEnemy = isEnemy;

        if (isEnemy)
        {
            enemyDeathCount++;

            if (enemyAnimator != null)
            {
                enemyAnimator.SetTrigger("isElectrocuted"); // Activa la animaci�n de electrocutado
                GameObject rayoInstance = Instantiate(rayoPrefab, rayoSpawnPoint.position, rayoSpawnPoint.rotation, enemy.transform);

                EnemyAnimationController animationController = enemy.GetComponent<EnemyAnimationController>();
                if (animationController != null)
                {
                    animationController.Die(); // Marca al enemigo como muerto
                }

                yield return new WaitForSeconds(electrocutedDuration); // Espera la duraci�n de la animaci�n

                if (rayoInstance != null)
                {
                    Destroy(rayoInstance); // Destruye el rayo despu�s de la animaci�n
                }

                if (enemyDeathCount < 2 && animationController != null)
                {
                    animationController.ReviveEnemy(); // Revive al enemigo si a�n no ha perdido
                }
            }
        }
        else
        {
            playerDeathCount++;
            if (deadEffect != null)
            {
                deadEffect.SetActive(true); // Activa el efecto visual de muerte del jugador
            }
        }

        deathMessage.gameObject.SetActive(true); // Muestra el mensaje de muerte
        yield return new WaitForSeconds(5f); // Espera antes de ocultar el mensaje
        deathMessage.gameObject.SetActive(false);

        if (deadEffect != null)
        {
            deadEffect.SetActive(false); // Desactiva el efecto de muerte del jugador
        }

        roundCounter++;

        // Verifica si alguno ha alcanzado el l�mite de muertes
        if (enemyDeathCount >= 2 || playerDeathCount >= 2)
        {
            LoadNextScene(); // Carga la escena correspondiente
        }
        else
        {
            ResetButtons(); // Reinicia los botones para la siguiente ronda
        }
    }

    private void LoadNextScene()
    {
        // Carga la escena de victoria dependiendo de qui�n haya ganado
        if (enemyDeathCount >= 2)
        {
            SceneManager.LoadScene("playerVictory");
        }
        else if (playerDeathCount >= 2)
        {
            SceneManager.LoadScene("enemyVictory");
        }
    }

    private void ResetButtons()
    {
        // Oculta el mensaje de muerte
        deathMessage.gameObject.SetActive(false);

        if (deadEffect != null)
        {
            deadEffect.SetActive(false);
        }

        // Resetea todos los botones
        foreach (ButtonController button in playerButtons)
        {
            button.ResetButton();
        }
        foreach (ButtonController button in enemyButtons)
        {
            button.ResetButton();
        }

        // Selecciona botones aleatorios para ser los botones de muerte
        playerDeathButton = playerButtons[UnityEngine.Random.Range(0, playerButtons.Length)];
        playerDeathButton.isDeathButton = true;

        enemyDeathButton = enemyButtons[UnityEngine.Random.Range(0, enemyButtons.Length)];
        enemyDeathButton.isDeathButton = true;
    }
}
