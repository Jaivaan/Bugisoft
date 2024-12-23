using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Collections;
using System.Linq;



public class CardDeckManager : MonoBehaviour
{
    public GameObject[] cards;
    public Transform[] cardPositions;
    public Transform[] enemyCardPositions;
    private GameObject[] selectedCards;
    private GameObject[] enemySelectedCards;
    public Transform[] centralPositions;
    public List<GameObject> cartasRestantes;
    public List<GameObject> provisional;
    private int currentIndex = 0;
    public EnemyController enemyController;
    public bool IsFirstPlayerMove = true;
    public Material green;
    public Material red;

    public Transform deckTransform;
    private List<GameObject> deckList;

    public enum RoundType
    {
        Aces,
        Kings,
        Queens,
        Jacks
    }

    public RoundType currentRound = RoundType.Aces;

    void Start()
    {
        Debug.Log("Nueva ronda: " + currentRound);
        deckList = new List<GameObject>(cards);

        foreach (GameObject card in deckList)
        {
            ReturnCardToDeck(card);
        }

        DealNewRound();
    }

    public void ChangeRound()
    {

        foreach (GameObject card in selectedCards)
        {
            ReturnCardToDeck(card);
        }
        foreach (GameObject card in enemySelectedCards)
        {
            ReturnCardToDeck(card);
        }
        ClearCentralCards();

        DealNewRound();

        currentRound = (RoundType)(((int)currentRound + 1) % 4);
        Debug.Log("Nueva ronda: " + currentRound);
    }

    private void DealNewRound()
    {
        Shuffle(deckList);

        selectedCards = deckList.Take(6).ToArray();
        for (int i = 0; i < selectedCards.Length; i++)
        {
            GameObject card = selectedCards[i];
            card.SetActive(true);
            card.transform.position = cardPositions[i].position;
            card.transform.rotation = cardPositions[i].rotation;
            card.transform.Rotate(180, 0, 0);  
        }

        enemySelectedCards = deckList.Skip(6).Take(6).ToArray();
        for (int i = 0; i < enemySelectedCards.Length; i++)
        {
            GameObject card = enemySelectedCards[i];
            card.SetActive(true);
            card.transform.position = enemyCardPositions[i].position;
            card.transform.rotation = enemyCardPositions[i].rotation; 
        }
    }

    private void ReturnCardToDeck(GameObject card)
    {
        if (card != null)
        {
            card.transform.position = deckTransform.position;
            card.transform.rotation = deckTransform.rotation;
            card.SetActive(false);
        }
    }

    private void Shuffle(List<GameObject> list)
    {
        System.Random rng = new System.Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            GameObject value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    private void ResetPlayerCards()
    {
        selectedCards = GetRandomCards(6);
        foreach (GameObject card in selectedCards)
        {
            card.SetActive(true);
        }
    }

    private void ResetEnemyCards()
    {
        enemySelectedCards = GetRandomCards(6);
        foreach (GameObject card in enemySelectedCards)
        {
            card.SetActive(true);
        }
    }

    public bool IsCorrectRoundType(GameObject card)
    {
        switch (currentRound)
        {
            case RoundType.Aces:
                return card.name.Contains("Ace");
            case RoundType.Kings:
                return card.name.Contains("King");
            case RoundType.Queens:
                return card.name.Contains("Queen");
            case RoundType.Jacks:
                return card.name.Contains("Jack");
            default:
                return false;
        }
    }

    public GameObject[] GetRandomCards(int count, List<GameObject> excludedCards = null)
    {
        GameObject[] randomCards = new GameObject[count];
        System.Random random = new System.Random();

        List<GameObject> availableCards = new List<GameObject>(cards);

        if (excludedCards != null)
        {
            availableCards.RemoveAll(card => excludedCards.Contains(card));
        }

        for (int i = 0; i < count; i++)
        {
            if (availableCards.Count == 0)
            {
                Debug.LogError("No hay suficientes cartas disponibles para asignar.");
                break;
            }

            int index = random.Next(0, availableCards.Count);
            randomCards[i] = availableCards[index];

            availableCards.RemoveAt(index);
        }
        return randomCards;
    }

    public void MoveCardToTable(GameObject card)
    {

        if (currentIndex < centralPositions.Length)
        {
            Transform targetPosition = centralPositions[currentIndex];
            card.transform.position = targetPosition.position;
            card.transform.rotation = targetPosition.rotation;
            currentIndex++;
            Debug.Log($"Carta {card.name} movida al centro en posición {currentIndex - 1}.");
        }
        else
        {
            Debug.LogWarning("No hay mas posiciones centrales disponibles.");
        }

       
    }

    public void ConfirmPlay()
    {
        int validCardCount = 0;
        int count = 0;


        for (int i = 0; i < centralPositions.Length; i++)
        {
            Transform centralPosition = centralPositions[i];
            
            foreach (GameObject card in selectedCards)
            {
                if (card != null && Vector3.Distance(card.transform.position, centralPosition.position) < 0.01f)
                {
                    count++;
                    if (IsCorrectRoundType(card))
                    {
                        validCardCount++;
                    }
                    provisional.Add(card);
                    
                    break; 

                }
            }
        }
        int declaredAces = currentIndex;

        cartasRestantes = new List<GameObject>(selectedCards);
        foreach (GameObject prov in provisional)
        {
            cartasRestantes.Remove(prov);
        }
        selectedCards = cartasRestantes.ToArray();

        enemyController.EvaluatePlayerMove(declaredAces, GetPlayedCards());
    }

    private GameObject[] GetPlayedCards()
    {
        GameObject[] playedCards = new GameObject[currentIndex];
        for (int i = 0; i < currentIndex; i++)
        {
            foreach (GameObject card in provisional)
            {
                if (card != null && Vector3.Distance(card.transform.position, centralPositions[i].position) < 0.01f)
                {
                    playedCards[i] = card;
                    break;
                }
            }
        }
        return playedCards;
    }

    public GameObject[] GetEnemyCards()
    {
        return enemySelectedCards;
    }

    public void SetEnemyCards(GameObject[] cards)
    {
        enemySelectedCards = cards;
    }

    public void ClearCentralCards()
    {
        foreach (GameObject card in provisional)
        {
            if (card != null)
            {
                ReturnCardToDeck(card);
            }
        }
        provisional.Clear();

        foreach (GameObject card in enemyController.provisional)
        {
            if (card != null)
            {
                ReturnCardToDeck(card);
            }
        }
        enemyController.provisional.Clear();

        currentIndex = 0;
    }
    
    public void EvaluateEnemyMove()
{
    int declaredAcesByEnemy = enemyController.cardsToPlay;

    GameObject[] enemyPlayedCards = enemyController.provisional.ToArray();

    CheckEnemyCards(declaredAcesByEnemy, enemyPlayedCards);
}

private void CheckEnemyCards(int declaredAces, GameObject[] playedCards)
{
    int realCount = 0;

    for (int i = 0; i < playedCards.Length; i++)
    {
        GameObject card = playedCards[i];
        if (card != null)
        {
            card.transform.Rotate(0, 180, 0);

            if (IsCorrectRoundType(card))
            {
                    StartCoroutine(ShowResultThenRevert(card, green, 1f));
                    realCount++;
            }else
            {
                    StartCoroutine(ShowResultThenRevert(card, red, 1f));
            }
        }
    }

    if (realCount == declaredAces)
    {
        Debug.Log("El enemigo estaba diciendo la verdad. Penalización para el jugador.");
        enemyController.PlayerPenalty();
    }
    else
    {
        Debug.Log($"El enemigo estaba mintiendo. Declaró {declaredAces} Ases pero tenía {realCount}.");
        StartCoroutine(WaitThenEnemyPenalty());
    }
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


    private IEnumerator WaitThenEnemyPenalty()
{
    yield return new WaitForSeconds(5f);
    enemyController.EnemyPenalty();
}


}
