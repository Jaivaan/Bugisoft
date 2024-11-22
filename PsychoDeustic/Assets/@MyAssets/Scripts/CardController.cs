using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardController : MonoBehaviour
{
    private CardDeckManager cardDeckManager;

    void Start()
    {
        
        cardDeckManager = FindObjectOfType<CardDeckManager>();
    }

    public void OnClick()
    {
        Debug.Log("Carta seleccionada: " + gameObject.name);

        if (cardDeckManager.IsFirstPlayerMove)
        {
            cardDeckManager.ClearCentralCards();
            cardDeckManager.IsFirstPlayerMove = false;
        }
        cardDeckManager.MoveCardToTable(gameObject);
        this.transform.Rotate(90, -180, 0);
    }
}
