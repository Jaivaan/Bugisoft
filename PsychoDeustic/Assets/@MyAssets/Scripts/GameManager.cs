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
    public float electrocutedDuration = 5f;

    private Animator enemyAnimator;

    public GameObject rayoPrefab;
    public Transform rayoSpawnPoint;

    public TemblorCamara temblorCamara;

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
                Debug.LogError("No se encontro el Animator en el enemigo.");
            }
        }
        else
        {
            Debug.LogError("El GameObject del enemigo no esta asignado.");
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
                    Debug.LogError("No se encontro el EnemyAnimationController en el enemigo.");
                }

                yield return new WaitForSeconds(electrocutedDuration);

                if (rayoInstance != null)
                {
                    Destroy(rayoInstance);
                    Debug.Log("Prefab de rayo destruido.");
                }

            }
            else
            {
                Debug.LogError("No se encontro el Animator en el enemigo.");
            }
        }
        else
        {
            deathMessage.text = "Has muerto";
            /*
            temblorCamara = Camera.main.GetComponent<temblorCamara>();
            if (temblorCamara != null)
            {
                StartCoroutine(temblorCamara.Shake(2f, 0.1f));
            }

            yield return new WaitForSeconds(2f);
            */
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
