using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

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

    private int roundCounter = 0;
    private bool lastDeathWasEnemy;

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
        lastDeathWasEnemy = isEnemy;

        if (isEnemy)
        {
            deathMessage.text = "El enemigo ha muerto";

            if (enemyAnimator != null)
            {
                enemyAnimator.SetTrigger("isElectrocuted");
                GameObject rayoInstance = Instantiate(rayoPrefab, rayoSpawnPoint.position, rayoSpawnPoint.rotation, enemy.transform);

                EnemyAnimationController animationController = enemy.GetComponent<EnemyAnimationController>();
                if (animationController != null)
                {
                    animationController.Die();
                }

                yield return new WaitForSeconds(electrocutedDuration);

                if (rayoInstance != null)
                {
                    Destroy(rayoInstance);
                }
            }
        }
        else
        {
            //deathMessage.text = "Has muerto";

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

        roundCounter++;

        if (roundCounter >= 2)
        {
            LoadNextScene();
        }
        else
        {
            ResetButtons();
        }
    }

    private void LoadNextScene()
    {
        if (lastDeathWasEnemy)
        {
            SceneManager.LoadScene("menuScene");
        }
        else
        {
            SceneManager.LoadScene("enemyVictory");
        }
    }

    private void ResetButtons()
    {
        deathMessage.gameObject.SetActive(false);

        if (deadEffect != null)
        {
            deadEffect.SetActive(false);
        }

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
