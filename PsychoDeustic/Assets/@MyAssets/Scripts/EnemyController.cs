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

        bool believesPlayer = random.Next(0, 100) < 1;

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
        int realCount = 0;

        for (int i = 0; i < playedCards.Length; i++)
        {
            GameObject card = playedCards[i];
            if (card != null)
            {
                card.transform.Rotate(0, 180, 0);

                if (cardDeckManager.IsCorrectRoundType(card))
                {
                    StartCoroutine(ShowResultThenRevert(card, green, 1f));
                    realCount++;
                }
                else
                {
                    StartCoroutine(ShowResultThenRevert(card, red, 1f));
                }
            }
        }
        if (realCount == declaredAces)
        {
            Debug.Log("El jugador estaba diciendo la verdad. Penalizacion para el enemigo.");
            StartCoroutine(WaitThenEnemyPenalty());
        }
        else
        {
            Debug.Log($"El jugador estaba mintiendo. Declara {declaredAces} Ases pero tenia {realCount}.");
            PlayerPenalty();
        }
    }

    private IEnumerator WaitThenEnemyPenalty()
    {
        yield return new WaitForSeconds(2f);
        EnemyPenalty();
    }

    private IEnumerator ShowResultThenRevert(GameObject card, Material newMat, float waitTime)
    {
        if (card == null) yield break;
        Renderer rend = card.GetComponent<Renderer>();
        if (rend == null) yield break;

        Material[] originalMats = rend.materials;

        Material[] matsChanged = rend.materials;
        matsChanged[0] = newMat;
        if (matsChanged.Length > 1)
            matsChanged[1] = newMat;

        rend.materials = matsChanged;

        yield return new WaitForSeconds(waitTime);

        rend.materials = originalMats;
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
