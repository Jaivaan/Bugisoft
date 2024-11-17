using UnityEngine;

public class CardDeckManager : MonoBehaviour
{
    public GameObject[] cards;
    public Transform[] cardPositions; 

    private GameObject[] selectedCards; 

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

    public void MoveCardToTable(GameObject card, Transform targetPosition)
    {
      
        card.transform.position = targetPosition.position;
        card.transform.rotation = targetPosition.rotation;
    }
}
