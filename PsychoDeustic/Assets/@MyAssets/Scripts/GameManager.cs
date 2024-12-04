using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public ButtonController[] playerButtons;
    public ButtonController[] enemyButtons;
    public TMP_Text deathMessage;

    public ButtonController playerDeathButton;
    public ButtonController enemyDeathButton;
    public AudioClip deathSound;
    private AudioSource audioSource;

    public GameObject enemy;
    public float electrocutedDuration = 5f; // Duración en segundos

    // Referencia al Animator del enemigo
    private Animator enemyAnimator;

    // Variables para el prefab de rayo
    public GameObject rayoPrefab; // Asigna tu prefab de rayo en el Inspector
    public Transform rayoSpawnPoint; // Asigna el RayoSpawnPoint en el Inspector

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        audioSource = GetComponent<AudioSource>();

        // Obtener el Animator una vez al inicio
        if (enemy != null)
        {
            enemyAnimator = enemy.GetComponent<Animator>();
            if (enemyAnimator == null)
            {
                Debug.LogError("No se encontró el Animator en el enemigo.");
            }
        }
        else
        {
            Debug.LogError("El GameObject del enemigo no está asignado.");
        }

        ResetButtons();
    }

    public void CheckIfDeathButton(ButtonController clickedButton)
    {
        if (clickedButton.isEnemyButton)
        {
            if (clickedButton == enemyDeathButton)
            {
                audioSource.PlayOneShot(deathSound);
                StartCoroutine(DeathSequence(true));
            }
        }
        else
        {
            if (clickedButton == playerDeathButton)
            {
                audioSource.PlayOneShot(deathSound);
                StartCoroutine(DeathSequence(false));
            }
        }
    }

    private IEnumerator DeathSequence(bool isEnemy)
    {
        if (isEnemy)
        {
            deathMessage.text = "El enemigo ha muerto";

            if (enemyAnimator != null)
            {
                // Activar el trigger "Electrocuted"
                enemyAnimator.SetTrigger("isElectrocuted");
                Debug.Log("Trigger 'Electrocuted' activado.");

                // Instanciar el prefab de rayo en la posición del spawn point
                GameObject rayoInstance = Instantiate(rayoPrefab, rayoSpawnPoint.position, rayoSpawnPoint.rotation, enemy.transform);
                Debug.Log("Prefab de rayo instanciado.");

                // Obtener y desactivar el EnemyAnimationController para detener otras animaciones
                EnemyAnimationController animationController = enemy.GetComponent<EnemyAnimationController>();
                if (animationController != null)
                {
                    animationController.Die(); // Marca al enemigo como muerto
                }
                else
                {
                    Debug.LogError("No se encontró el EnemyAnimationController en el enemigo.");
                }

                // Esperar la duración de la animación de "Electrocuted"
                yield return new WaitForSeconds(electrocutedDuration);

                // Destruir el prefab de rayo
                if (rayoInstance != null)
                {
                    Destroy(rayoInstance);
                    Debug.Log("Prefab de rayo destruido.");
                }

            }
            else
            {
                Debug.LogError("No se encontró el Animator en el enemigo.");
            }
        }
        else
        {
            deathMessage.text = "Has muerto";
        }

        deathMessage.gameObject.SetActive(true);

        yield return new WaitForSeconds(5);

        ResetButtons();
    }

    private void ResetButtons()
    {
        deathMessage.gameObject.SetActive(false);

        foreach (ButtonController button in playerButtons)
        {
            button.ResetButton();
        }
        foreach (ButtonController button in enemyButtons)
        {
            button.ResetButton();
        }

        playerDeathButton = playerButtons[UnityEngine.Random.Range(0, playerButtons.Length)];
        playerDeathButton.isDeathButton = true;

        enemyDeathButton = enemyButtons[UnityEngine.Random.Range(0, enemyButtons.Length)];
        enemyDeathButton.isDeathButton = true;
    }
}
