using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
public class ButtonController : MonoBehaviour
{
    public Material redMaterial; // Material para "clickeable"
    public Material blueMaterial; // Material para "no clickeable"

    private bool isClickable = true;

    public void OnHoverEnter()
    {
        if (isClickable)
        {
            // Cambiar color del botón a azul
            isClickable = false;
            GetComponent<Renderer>().material = blueMaterial;

            // Notificar al GameManager
            GameManager.Instance.CheckIfDeathButton(this);
        }
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
            GameManager.Instance.CheckIfDeathButton(this); // Verificar si este botón es el "mortal"
            
        }
    }

    public void ResetButton()
    {
        isClickable = true;
        GetComponent<MeshRenderer>().material = redMaterial;
    }
}
