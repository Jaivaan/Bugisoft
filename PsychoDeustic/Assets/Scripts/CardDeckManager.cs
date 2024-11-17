using UnityEngine;

public class CardDeckManager : MonoBehaviour
{
    public GameObject[] cards; // Asigna aquí las 52 cartas (hijos del Card Deck)
    public Transform leftHand; // Punto donde se colocarán las cartas en la mano izquierda
    public Transform table; // Punto donde se colocarán las cartas en la mesa

    private GameObject[] selectedCards; // Almacena las cartas seleccionadas

    void Start()
    {
        // Seleccionar seis cartas aleatorias
        selectedCards = GetRandomCards(6);

        // Colocar las cartas en la mano izquierda
        for (int i = 0; i < selectedCards.Length; i++)
        {
            GameObject card = selectedCards[i];
            card.transform.position = leftHand.position + new Vector3(0.1f * i, 0, 0); // Espaciado horizontal
            card.transform.rotation = leftHand.rotation;
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
                index = random.Next(0, cards.Length); // Seleccionar una carta aleatoria
            }
            while (System.Array.Exists(randomCards, c => c == cards[index])); // Evitar duplicados

            randomCards[i] = cards[index];
        }
        return randomCards;
    }

    public void MoveCardToTable(GameObject card)
    {
        card.transform.position = table.position; // Mover la carta a la mesa
        card.transform.rotation = table.rotation;
    }
}
