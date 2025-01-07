using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ButtonController : MonoBehaviour
{
    public Material redMaterial; // Material rojo para el bot�n
    public Material blueMaterial; // Material azul para el bot�n

    public CardDeckManager cardDeckManager; // Referencia al gestor de mazos de cartas
    public RoundManager roundManager; // Referencia al gestor de rondas

    public bool isClickable = true; // Indica si el bot�n puede ser clickeado
    public bool isEnemyButton = false; // Identifica si el bot�n pertenece al enemigo
    public bool isDeathButton = false; // Indica si este bot�n es el "bot�n de muerte"

    void Start()
    {
        // Encuentra autom�ticamente el gestor de mazos en la escena
        cardDeckManager = FindObjectOfType<CardDeckManager>();
    }

    public void OnHoverExit()
    {
        // Cambia el color del rayo de interacci�n al salir del hover sobre el bot�n
        XRRayInteractor rayInteractor = FindObjectOfType<XRRayInteractor>();
        rayInteractor.GetComponent<LineRenderer>().material.color = Color.white;
    }

    public void OnClick()
    {
        Debug.Log("Bot�n clickeado");
        if (isClickable)
        {
            isClickable = false; // Evita que el bot�n del enemigo se pueda clicar nuevamente
            GetComponent<MeshRenderer>().material = blueMaterial; // Cambia el color del bot�n a azul

            GameManager.Instance.CheckIfDeathButton(this); // Verifica si es el bot�n de muerte
            cardDeckManager.ChangeRound(); // Cambia a la siguiente ronda
        }
    }

    public void ResetButton()
    {
        // Restablece el estado del bot�n a su estado inicial
        isClickable = true;
        isDeathButton = false;
        GetComponent<MeshRenderer>().material = redMaterial; // Cambia el color del bot�n a rojo
    }
}
