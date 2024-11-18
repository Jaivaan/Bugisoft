using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardController : MonoBehaviour
{
    private CardDeckManager cardDeckManager;
    public Transform targetPosition;

    void Start()
    {
        
        cardDeckManager = FindObjectOfType<CardDeckManager>();
    }

    public void OnClick()
    {
        Debug.Log("Carta seleccionada: " + gameObject.name);
        targetPosition.transform.Rotate(180, 0, 0);
        cardDeckManager.MoveCardToTable(gameObject, targetPosition);
    }
}
