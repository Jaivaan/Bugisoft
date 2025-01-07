using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardController : MonoBehaviour
{
    private CardDeckManager cardDeckManager; // Referencia al gestor del mazo de cartas

    void Start()
    {
        // Busca y asigna automáticamente el gestor del mazo en la escena
        cardDeckManager = FindObjectOfType<CardDeckManager>();
    }

    public void OnClick()
    {
        Debug.Log("Carta seleccionada: " + gameObject.name); // Imprime en consola el nombre de la carta seleccionada

        if (cardDeckManager.IsFirstPlayerMove) // Comprueba si es el primer movimiento del jugador
        {
            cardDeckManager.ClearCentralCards(); // Limpia las cartas del centro de la mesa
            cardDeckManager.provisional.Add(gameObject); // Agrega esta carta a la lista provisional secundario(desde el que tienes al principio)
            cardDeckManager.IsFirstPlayerMove = false; // Marca que el primer movimiento ya ocurrió
        }

        cardDeckManager.MoveCardToTable(gameObject); // Mueve la carta a la mesa
        this.transform.Rotate(90, -180, 0); // Rota la carta para mostrarla visualmente
    }
}
