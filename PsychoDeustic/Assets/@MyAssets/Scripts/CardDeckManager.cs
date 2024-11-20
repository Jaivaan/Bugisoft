using UnityEngine;
using TMPro;


public class CardDeckManager : MonoBehaviour
{
    public GameObject[] cards;
    public Transform[] cardPositions; 

    private GameObject[] selectedCards;
    public Transform[] centralPositions;
    public TMP_Text confirmationText;
    private int currentIndex = 0;
    public EnemyController enemyController;

    void Start()
    {
        
        selectedCards = GetRandomCards(6);

      
        for (int i = 0; i < selectedCards.Length; i++)
        {
            GameObject card = selectedCards[i];
            Transform position = cardPositions[i];

            card.transform.position = position.position; 
            card.transform.rotation = position.rotation;
            card.transform.Rotate(180, 0, 0);
            card.SetActive(true);
        }
    }

    public GameObject[] GetRandomCards(int count)
    {
        GameObject[] randomCards = new GameObject[count];
        System.Random random = new System.Random();
        for (int i = 0; i < count; i++)
        {
            int index;
            do
            {
                index = random.Next(0, cards.Length);
            }
            while (System.Array.Exists(randomCards, c => c == cards[index])); 

            randomCards[i] = cards[index];
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
        }
        else
        {
            Debug.LogWarning("No hay más posiciones centrales disponibles.");
        }
    }

    public void ConfirmPlay()
    {
        int aceCount = 0;
        

        for (int i = 0; i < centralPositions.Length; i++)
        {
            Transform centralPosition = centralPositions[i];

            foreach (GameObject card in selectedCards)
            {
                if (card != null && Vector3.Distance(card.transform.position, centralPosition.position) < 0.01f)
                {
                    if (card.name.Contains("Ace"))
                    {
                        aceCount++;
                    }
                    break; 
                }
            }
        }
        int declaredAces = currentIndex;

        confirmationText.text = $"{aceCount} Ases";
        confirmationText.gameObject.SetActive(true);

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

    private System.Collections.IEnumerator HideConfirmationText()
    {
        yield return new WaitForSeconds(2);
        confirmationText.gameObject.SetActive(false);
    }


}
