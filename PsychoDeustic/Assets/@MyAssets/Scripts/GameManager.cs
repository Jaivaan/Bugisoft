using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using TMPro;
using System.Diagnostics;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public ButtonController[] playerButtons;
    public ButtonController[] enemyButtons;
    public TMP_Text deathMessage;

    public ButtonController playerDeathButton;
    public ButtonController enemyDeathButton;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        ResetButtons();
    }

    public void CheckIfDeathButton(ButtonController clickedButton)
    {
        if (clickedButton.isEnemyButton)
        {
            if (clickedButton == enemyDeathButton)
            {
                StartCoroutine(DeathSequence(true));
            }
        }
        else
        {
            if (clickedButton == playerDeathButton)
            {
                StartCoroutine(DeathSequence(false));
            }
        }
    }

    private IEnumerator DeathSequence(bool isEnemy)
    {
        if (isEnemy == true)
        {
            deathMessage.text = "El enemigo ha muerto";
        }
        else
        {
            deathMessage.text = "Has muerto";
        }
        
        deathMessage.gameObject.SetActive(true);

        yield return new WaitForSeconds(2);

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
