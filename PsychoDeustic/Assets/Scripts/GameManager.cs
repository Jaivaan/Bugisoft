using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using TMPro;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public ButtonController[] buttons; // Asigna aquí los botones
    public TMP_Text deathMessage; // Asigna el texto de la UI

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
        // Ocultar mensaje de muerte
        deathMessage.gameObject.SetActive(false);

        // Resetear todos los botones
        foreach (ButtonController button in buttons)
        {
            button.ResetButton();
        }

        // Elegir un botón aleatorio como "mortal"
        deathButton = buttons[UnityEngine.Random.Range(0, buttons.Length)];

    }
}
