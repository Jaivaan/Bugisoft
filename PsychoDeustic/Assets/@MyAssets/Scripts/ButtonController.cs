using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ButtonController : MonoBehaviour
{
    public Material redMaterial; // Material rojo para el botón
    public Material blueMaterial; // Material azul para el botón

    public CardDeckManager cardDeckManager; // Referencia al gestor de mazos de cartas
    public RoundManager roundManager; // Referencia al gestor de rondas

    public bool isClickable = true; // Indica si el botón puede ser clickeado
    public bool isEnemyButton = false; // Identifica si el botón pertenece al enemigo
    public bool isDeathButton = false; // Indica si este botón es el "botón de muerte"

    void Start()
    {
        // Encuentra automáticamente el gestor de mazos en la escena
        cardDeckManager = FindObjectOfType<CardDeckManager>();
    }

    public void OnHoverExit()
    {
        // Cambia el color del rayo de interacción al salir del hover sobre el botón
        XRRayInteractor rayInteractor = FindObjectOfType<XRRayInteractor>();
        rayInteractor.GetComponent<LineRenderer>().material.color = Color.white;
    }

    public void OnClick()
    {
        Debug.Log("Botón clickeado");
        if (isClickable)
        {
            isClickable = false; // Evita que el botón del enemigo se pueda clicar nuevamente
            GetComponent<MeshRenderer>().material = blueMaterial; // Cambia el color del botón a azul

            GameManager.Instance.CheckIfDeathButton(this); // Verifica si es el botón de muerte
            cardDeckManager.ChangeRound(); // Cambia a la siguiente ronda
        }
    }

    public void ResetButton()
    {
        // Restablece el estado del botón a su estado inicial
        isClickable = true;
        isDeathButton = false;
        GetComponent<MeshRenderer>().material = redMaterial; // Cambia el color del botón a rojo
    }
}
