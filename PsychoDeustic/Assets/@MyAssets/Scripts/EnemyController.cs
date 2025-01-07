using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class EnemyController : MonoBehaviour
{
    private System.Random random; // Generador de n�meros aleatorios
    public Material green; // Material para indicar una carta correcta
    public Material red; // Material para indicar una carta incorrecta
    public ButtonController[] enemyButtons; // Botones controlados por el enemigo
    public CardDeckManager cardDeckManager; // Referencia al gestor del mazo de cartas
    public List<GameObject> provisional; // Lista de cartas jugadas provisionalmente por el enemigo
    public int cardsToPlay; // Cantidad de cartas que el enemigo decidir� jugar

    public TMP_Text text; // Texto para mostrar mensajes en la interfaz

    void Start()
    {
        random = new System.Random(); // Inicializa el generador de n�meros aleatorios
    }

    // Eval�a el movimiento del jugador y decide c�mo reaccionar el enemigo
    public void EvaluatePlayerMove(int declaredCards, GameObject[] playedCards)
    {
        Debug.Log($"El jugador ha declarado: {declaredCards}");
        text.text = $"El jugador ha declarado: {declaredCards}";

        // Decide aleatoriamente si el enemigo cree en la declaraci�n del jugador (60% de probabilidad)
        bool believesPlayer = random.Next(0, 100) < 60;

        if (believesPlayer)
        {
            Debug.Log("El enemigo te cree. Su turno.");
            text.text = "El enemigo te cree. Su turno.";
            EnemyTurn(); // Si cree, pasa a su turno
        }
        else
        {
            Debug.Log("El enemigo piensa que est�s mintiendo. Levanta tus cartas.");
            text.text = "El enemigo piensa que est�s mintiendo. Levanta tus cartas.";
            CheckPlayerCards(declaredCards, playedCards); // Si no cree, revisa las cartas jugadas por el jugador
        }
    }

    // Verifica las cartas jugadas por el jugador para comprobar si minti�
    private void CheckPlayerCards(int declaredAces, GameObject[] playedCards)
    {
        int realCount = 0;

        for (int i = 0; i < playedCards.Length; i++)
        {
            GameObject card = playedCards[i];
            if (card != null)
            {
                card.transform.Rotate(0, 180, 0); // Gira la carta para mostrar su valor

                if (cardDeckManager.IsCorrectRoundType(card))
                {
                    StartCoroutine(ShowResultThenRevert(card, green, 1f)); // Marca como correcta
                    realCount++;
                }
                else
                {
                    StartCoroutine(ShowResultThenRevert(card, red, 1f)); // Marca como incorrecta
                }
            }
        }

        // Decide penalizaci�n dependiendo de si el jugador dec�a la verdad
        if (realCount == declaredAces)
        {
            Debug.Log("El jugador estaba diciendo la verdad. Penalizaci�n para el enemigo.");
            StartCoroutine(WaitThenEnemyPenalty());
        }
        else
        {
            Debug.Log($"El jugador estaba mintiendo. Declar� {declaredAces} {cardDeckManager.currentRound} pero ten�a {realCount}.");
            PlayerPenalty();
        }
    }

    // Penalizaci�n al enemigo tras un breve retraso
    private IEnumerator WaitThenEnemyPenalty()
    {
        yield return new WaitForSeconds(2f);
        EnemyPenalty();
    }

    // Muestra un resultado visual en la carta y luego lo revierte
    private IEnumerator ShowResultThenRevert(GameObject card, Material newMat, float waitTime)
    {
        if (card == null) yield break;

        Renderer rend = card.GetComponent<Renderer>();
        if (rend == null) yield break;

        Material[] originalMats = rend.materials;
        Material[] matsChanged = rend.materials;

        matsChanged[0] = newMat; // Cambia el material
        if (matsChanged.Length > 1)
            matsChanged[1] = newMat;

        rend.materials = matsChanged;

        yield return new WaitForSeconds(waitTime);

        rend.materials = originalMats; // Restaura el material original
    }

    // Penalizaci�n al enemigo
    public void EnemyPenalty()
    {
        Debug.Log("El enemigo pulsa bot�n.");
        text.text = "El enemigo pulsa bot�n.";

        ButtonController button = GetRandomAvailableButton(); // Selecciona un bot�n aleatorio
        if (button != null)
        {
            Debug.Log($"El enemigo presiona el bot�n: {button.name}");
            button.OnClick();
        }
    }

    // Obtiene un bot�n aleatorio que a�n est� disponible para ser presionado
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

    // Penalizaci�n al jugador
    public void PlayerPenalty()
    {
        Debug.Log("El jugador pulsa bot�n.");
        text.text = "El jugador pulsa bot�n.";
    }

    // Turno del enemigo para jugar sus cartas
    private void EnemyTurn()
    {
        Debug.Log("El enemigo est� jugando...");
        cardDeckManager.ClearCentralCards();

        GameObject[] enemyCards = cardDeckManager.GetEnemyCards();
        cardsToPlay = UnityEngine.Random.Range(1, 4); // Decide cu�ntas cartas jugar

        for (int i = 0; i < cardsToPlay; i++)
        {
            if (i < enemyCards.Length)
            {
                GameObject card = enemyCards[i];
                cardDeckManager.MoveCardToTable(card); // Mueve la carta al tablero
                card.transform.Rotate(90, 0, 0); // Gira la carta
                Debug.Log($"El enemigo juega la carta: {card.name}");
                provisional.Add(card); // Agrega la carta a la lista provisional
            }
        }

        // Actualiza las cartas restantes del enemigo
        GameObject[] newArray = new GameObject[enemyCards.Length - cardsToPlay];
        System.Array.Copy(enemyCards, cardsToPlay, newArray, 0, newArray.Length);
        cardDeckManager.SetEnemyCards(newArray);

        Debug.Log($"El enemigo ha jugado {cardsToPlay}");
        text.text = $"El enemigo ha jugado {cardsToPlay}";

        // Marca que es el turno del jugador
        cardDeckManager.IsFirstPlayerMove = true;
    }
}
