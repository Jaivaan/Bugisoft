using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private System.Random random;
    public Material green;
    public Material red;

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
            Debug.Log("El enemigo piensa que estás mintiendo. Levanta tus cartas.");
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
                Renderer cardRenderer = card.GetComponent<Renderer>();

                if (card.name.Contains("Ace"))
                {
                    cardRenderer.materials[0] = green;
                    cardRenderer.materials[1] = green;
                    cardRenderer.materials[2] = green;
                    realAces++;
                }
                else
                {
                    cardRenderer.materials[0] = red;
                    cardRenderer.materials[1] = red;
                    cardRenderer.materials[2] = red;
                }
            }
        }
        if (realAces == declaredAces)
        {
            Debug.Log("El jugador estaba diciendo la verdad. Penalización para el enemigo.");
            EnemyPenalty();
        }
        else
        {
            Debug.Log($"El jugador estaba mintiendo. Declaró {declaredAces} Ases pero tenía {realAces}.");
            PlayerPenalty();
        }
    }

    private void EnemyPenalty()
    {
        Debug.Log("El enemigo pulsa boton.");
    }

    private void PlayerPenalty()
    {
        Debug.Log("El jugador pulsa boton.");
    }

    private void EnemyTurn()
    {
        Debug.Log("El enemigo está jugando...");
    }
}
