using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
public class ButtonController : MonoBehaviour
{
    public Material redMaterial;
    public Material blueMaterial;

    public CardDeckManager cardDeckManager;
    public RoundManager roundManager;

    public bool isClickable = true;

    public bool isEnemyButton = false;

    public bool isDeathButton = false;

    void Start()
    {

        cardDeckManager = FindObjectOfType<CardDeckManager>();
    }

    public void OnHoverExit()
    {
        XRRayInteractor rayInteractor = FindObjectOfType<XRRayInteractor>();
        rayInteractor.GetComponent<LineRenderer>().material.color = Color.white;
    }

    public void OnClick()
    {
        Debug.Log("Botón clickeado");
        if (isClickable)
        {
            isClickable = false;
            GetComponent<MeshRenderer>().material = blueMaterial;

            GameManager.Instance.CheckIfDeathButton(this);
            cardDeckManager.ChangeRound();

        }
    }

    public void ResetButton()
    {
        {
            isClickable = true;
            isDeathButton = false;
            GetComponent<MeshRenderer>().material = redMaterial;
        }
    }
}



