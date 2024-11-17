using UnityEngine;

public class CardDeckManager : MonoBehaviour
{
    public GameObject[] cards; // Asigna las 52 cartas del mazo
    public Transform[] cardPositions; // Asigna los 6 Empty GameObjects de la mesa

    private GameObject[] selectedCards; // Cartas seleccionadas

    void Start()
    {
        // Seleccionar seis cartas aleatorias del mazo
        selectedCards = GetRandomCards(6);

        // Colocar las cartas en las posiciones predefinidas
        for (int i = 0; i < selectedCards.Length; i++)
        {
            GameObject card = selectedCards[i];
            Transform position = cardPositions[i];

            card.transform.position = position.position; // Posicionar la carta
            card.transform.rotation = position.rotation; // Ajustar la rotación
            card.transform.Rotate(180, 0, 0);
            card.SetActive(true); // Activar la carta
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

    public void MoveCardToTable(GameObject card, Transform targetPosition)
    {
        // Mover la carta a una posición específica en la mesa
        card.transform.position = targetPosition.position;
        card.transform.rotation = targetPosition.rotation;
    }
}
