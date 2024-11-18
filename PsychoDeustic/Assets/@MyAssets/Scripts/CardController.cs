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
        cardDeckManager.MoveCardToTable(gameObject);
        this.transform.Rotate(90, -180, 0);
    }
}
