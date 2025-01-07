using UnityEngine;
using System.Collections;

public class LightController : MonoBehaviour
{
    public Light[] lights; // Array de luces que serán controladas
    public Material materialSinEmision; // Material sin emisión (estado apagado)
    public Material materialConEmision; // Material con emisión (estado encendido)
    public GameObject[] objectsToChange; // Objetos cuyo material cambiará
    public AudioClip switchSound; // Sonido que se reproduce al activar las luces
    private AudioSource audioSource; // Componente de audio para reproducir sonidos

    void Start()
    {
        // Obtiene el componente de AudioSource o lo añade si no existe
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Inicia una rutina para encender las luces y cambiar los materiales después de un retraso
        StartCoroutine(TurnOnLightsAndChangeMaterial(2f));
    }

    IEnumerator TurnOnLightsAndChangeMaterial(float delay)
    {
        // Espera el tiempo especificado antes de realizar las acciones
        yield return new WaitForSeconds(delay);

        // Activa todas las luces del array
        foreach (Light light in lights)
        {
            if (light != null)
            {
                light.enabled = true;
            }
        }

        // Cambia el material de los objetos asignados al material con emisión
        foreach (GameObject obj in objectsToChange)
        {
            Renderer renderer = obj.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material = materialConEmision;
            }
        }

        // Reproduce el sonido de switch si está asignado
        if (switchSound != null)
        {
            audioSource.PlayOneShot(switchSound);
        }
        else
        {
            Debug.LogWarning("No se ha asignado un sonido de switch en el inspector.");
        }
    }
}
