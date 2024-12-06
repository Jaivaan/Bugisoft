using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private System.Random random;
    public Material green;
    public Material red;
    public ButtonController[] enemyButtons;
    public CardDeckManager cardDeckManager;
    public List<GameObject> provisional;
    public int cardsToPlay;

    void Start()
    {
        random = new System.Random();
    }

    public void EvaluatePlayerMove(int declaredCards, GameObject[] playedCards)
    {
        Debug.Log($"El jugador ha declarado: {declaredCards}");

        bool believesPlayer = random.Next(0, 100) < 70;

        if (believesPlayer)
        {
            Debug.Log("El enemigo te cree. Su turno.");
            EnemyTurn();
        }
        else
        {
            Debug.Log("El enemigo piensa que estas mintiendo. Levanta tus cartas.");
            CheckPlayerCards(declaredCards, playedCards);
        }
    }

    private void CheckPlayerCards(int declaredAces, GameObject[] playedCards)
    {
        int realAces = 0;

        for (int i = 0; i < playedCards.Length; i++)
        {
            GameObject card = playedCards[i];
            if (card != null)
            {
                card.transform.Rotate(0, 180, 0);

                if (card.name.Contains("Ace"))
                {
                    Renderer rend = card.GetComponent<Renderer>();
                    Material[] mats = rend.materials;
                    mats[0] = green;
                    mats[1] = green;
                    rend.materials = mats;
                    realAces++;
                }
                else
                {
                    Renderer rend = card.GetComponent<Renderer>();
                    Material[] mats = rend.materials;
                    mats[0] = red;
                    mats[1] = red;
                    rend.materials = mats;
                }
            }
        }
        if (realAces == declaredAces)
        {
            Debug.Log("El jugador estaba diciendo la verdad. Penalizacion para el enemigo.");
            EnemyPenalty();
        }
        else
        {
            Debug.Log($"El jugador estaba mintiendo. Declara {declaredAces} Ases pero tenia {realAces}.");
            PlayerPenalty();
        }
    }

    public void EnemyPenalty()
    {
        Debug.Log("El enemigo pulsa boton.");

        ButtonController button = GetRandomAvailableButton();

        Debug.Log($"El enemigo presiona el boton: {button.name}");

        button.OnClick();
    }

    private ButtonController GetRandomAvailableButton()
    {
        var availableButtons = new List<ButtonController>();
        foreach (var button in enemyButtons)
        {
            if (button.isClickable)
            {
                availableButtons.Add(button);
            }
        }

        if (availableButtons.Count == 0) return null;

        int randomIndex = UnityEngine.Random.Range(0, availableButtons.Count);
        return availableButtons[randomIndex];
    }

    public void PlayerPenalty()
    {
        Debug.Log("El jugador pulsa boton.");
    }

    private void EnemyTurn()
    {
        Debug.Log("El enemigo esta jugando...");
        cardDeckManager.ClearCentralCards();

        GameObject[] enemyCards = cardDeckManager.GetEnemyCards();
        cardsToPlay = UnityEngine.Random.Range(1, 4);

        for (int i = 0; i < cardsToPlay; i++)
        {
            if (i < enemyCards.Length)
            {
                GameObject card = enemyCards[i];
                cardDeckManager.MoveCardToTable(card);
                card.transform.Rotate(90, 0, 0);
                Debug.Log($"El enemigo juega la carta: {card.name}");
                provisional.Add(card);
            }
        }

        GameObject[] newArray = new GameObject[enemyCards.Length - cardsToPlay];
        System.Array.Copy(enemyCards, cardsToPlay, newArray, 0, newArray.Length);
        cardDeckManager.SetEnemyCards(newArray);

        Debug.Log($"El enemigo ha jugado {cardsToPlay} Ases.");
        cardDeckManager.IsFirstPlayerMove = true;
    }
}
