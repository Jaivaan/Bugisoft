using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Collections;


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
    public TMP_Text confirmationText;
    private int currentIndex = 0;
    public EnemyController enemyController;
    public bool IsFirstPlayerMove = true;

    void Start()
    {
        List<GameObject> assignedCards = new List<GameObject>();

        selectedCards = GetRandomCards(6);
        assignedCards.AddRange(selectedCards);

        

        for (int i = 0; i < selectedCards.Length; i++)
        {
            GameObject card = selectedCards[i];
            Transform position = cardPositions[i];

            card.transform.position = position.position; 
            card.transform.rotation = position.rotation;
            card.transform.Rotate(180, 0, 0);
            card.SetActive(true);
        }

        enemySelectedCards = GetRandomCards(6, assignedCards);

        for (int i = 0; i < enemySelectedCards.Length; i++)
        {
            GameObject card = enemySelectedCards[i];
            Transform position = enemyCardPositions[i];

            card.transform.position = position.position;
            card.transform.rotation = position.rotation;
            card.SetActive(true);
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
            Debug.LogWarning("No hay m�s posiciones centrales disponibles.");
        }

       
    }

    public void ConfirmPlay()
    {
        int aceCount = 0;
        int count = 0;


        for (int i = 0; i < centralPositions.Length; i++)
        {
            Transform centralPosition = centralPositions[i];
            
            foreach (GameObject card in selectedCards)
            {
                if (card != null && Vector3.Distance(card.transform.position, centralPosition.position) < 0.01f)
                {
                    count++;
                    if (card.name.Contains("Ace"))
                    {
                        aceCount++;
                    }
                    provisional.Add(card);
                    
                    break; 

                }
            }
        }
        int declaredAces = currentIndex;

        confirmationText.text = $"{aceCount} Ases";
        confirmationText.gameObject.SetActive(true);

        cartasRestantes = new List<GameObject>(selectedCards);
        foreach (GameObject prov in provisional)
        {
            cartasRestantes.Remove(prov);
        }
        selectedCards = cartasRestantes.ToArray();

        enemyController.EvaluatePlayerMove(declaredAces, GetPlayedCards());

        StartCoroutine(HideConfirmationText());
    }

    private GameObject[] GetPlayedCards()
    {
        GameObject[] playedCards = new GameObject[currentIndex];
        for (int i = 0; i < currentIndex; i++)
        {
            foreach (GameObject card in selectedCards)
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
        for (int i = 0; i < currentIndex; i++)
        {
            foreach (GameObject card in provisional)
            {
                if (card != null && Vector3.Distance(card.transform.position, centralPositions[i].position) < 0.01f)
                {
                    Debug.Log($"Eliminando carta: {card.name}");
                    card.SetActive(false);
                    //break;
                }
            }
            provisional.Clear();

            foreach (GameObject card in enemyController.provisional)
            {
                if (card != null && Vector3.Distance(card.transform.position, centralPositions[i].position) < 0.01f)
                {
                    Debug.Log($"Eliminando carta: {card.name}");
                    card.SetActive(false);
                    //break;
                }
            }
            enemyController.provisional.Clear();
        }
        currentIndex = 0;
    }


    private System.Collections.IEnumerator HideConfirmationText()
    {
        yield return new WaitForSeconds(2);
        confirmationText.gameObject.SetActive(false);
    }


}
