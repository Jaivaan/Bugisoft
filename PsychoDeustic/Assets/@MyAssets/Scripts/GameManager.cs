using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using TMPro;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public ButtonController[] buttons;
    public TMP_Text deathMessage;

    private ButtonController deathButton;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        ResetButtons();
    }

    public void CheckIfDeathButton(ButtonController clickedButton)
    {
        if (clickedButton == deathButton)
        {
            StartCoroutine(DeathSequence());
        }
    }

    private IEnumerator DeathSequence()
    {
        deathMessage.text = "Has muerto";
        deathMessage.gameObject.SetActive(true);

        yield return new WaitForSeconds(2);

        ResetButtons();
    }

    private void ResetButtons()
    {
        deathMessage.gameObject.SetActive(false);

        foreach (ButtonController button in buttons)
        {
            button.ResetButton();
        }

        deathButton = buttons[UnityEngine.Random.Range(0, buttons.Length)];

    }
}
