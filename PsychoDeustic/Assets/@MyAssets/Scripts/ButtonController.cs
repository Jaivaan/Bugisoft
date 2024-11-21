using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
public class ButtonController : MonoBehaviour
{
    public Material redMaterial; // Material para "clickeable"
    public Material blueMaterial; // Material para "no clickeable"

    public bool isClickable = true;

    public bool isEnemyButton = false;

    public bool isDeathButton = false;

    public void OnHoverEnter()
    {
        /*
        if (isClickable)
        {
            // Cambiar color del botón a azul
            isClickable = false;
            GetComponent<Renderer>().material = blueMaterial;

            // Notificar al GameManager
            GameManager.Instance.CheckIfDeathButton(this);
        }
        */
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
            
        }
    }

    public void ResetButton()
    {
        isClickable = true;
        isDeathButton = false;
        GetComponent<MeshRenderer>().material = redMaterial;
    }
}
