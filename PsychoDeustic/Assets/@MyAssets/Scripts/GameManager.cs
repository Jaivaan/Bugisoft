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
    private Animator enemyAnimator;
    public float electrocutedDuration = 5f;

 
    public GameObject rayoPrefab;
    public Transform rayoSpawnPoint;


    public GameObject deadEffect;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        audioSource = GetComponent<AudioSource>();

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

        
        if (deathMessage != null)
            deathMessage.gameObject.SetActive(false);

        
        if (deadEffect != null)
            deadEffect.SetActive(false);

        ResetButtons();
    }

    public void CheckIfDeathButton(ButtonController clickedButton)
    {
        // Si el botón es del enemigo
        if (clickedButton.isEnemyButton)
        {
            if (clickedButton == enemyDeathButton)
            {
                audioSource.PlayOneShot(deathSound);
                StartCoroutine(DeathSequence(true));
            }
        }
        // Si el botón es del jugador
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

            // Logica de electrocucioin/animacion
            if (enemyAnimator != null)
            {
                enemyAnimator.SetTrigger("isElectrocuted");
                Debug.Log("Trigger 'Electrocuted' activado.");

                GameObject rayoInstance = Instantiate(rayoPrefab, rayoSpawnPoint.position, rayoSpawnPoint.rotation, enemy.transform);
                Debug.Log("Prefab de rayo instanciado.");

                EnemyAnimationController animationController = enemy.GetComponent<EnemyAnimationController>();
                if (animationController != null)
                {
                    animationController.Die();
                }
                else
                {
                    Debug.LogError("No se encontró el EnemyAnimationController en el enemigo.");
                }

                yield return new WaitForSeconds(electrocutedDuration);

                if (rayoInstance != null)
                {
                    Destroy(rayoInstance);
                    Debug.Log("Prefab de rayo destruido.");
                }
            }
        }
        else
        {
            deathMessage.text = "Has muerto";

            if (deadEffect != null)
            {
                deadEffect.SetActive(true);
            }

        }

        deathMessage.gameObject.SetActive(true);

        yield return new WaitForSeconds(5f);

        deathMessage.gameObject.SetActive(false);

        if (deadEffect != null)
        {
            deadEffect.SetActive(false);
        }
        ResetButtons();
    }

    private void ResetButtons()
    {
        deathMessage.gameObject.SetActive(false);

        if (deadEffect != null)
        {
            deadEffect.SetActive(false);
        }

        // Reset de botones
        foreach (ButtonController button in playerButtons)
        {
            button.ResetButton();
        }
        foreach (ButtonController button in enemyButtons)
        {
            button.ResetButton();
        }

        // Seleccionar un botón "mortal" al azar para el jugador
        playerDeathButton = playerButtons[UnityEngine.Random.Range(0, playerButtons.Length)];
        playerDeathButton.isDeathButton = true;

        // Seleccionar un botón "mortal" al azar para el enemigo
        enemyDeathButton = enemyButtons[UnityEngine.Random.Range(0, enemyButtons.Length)];
        enemyDeathButton.isDeathButton = true;
    }
}
