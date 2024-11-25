using UnityEngine;
using System.Collections;

public class LightController : MonoBehaviour
{
    public Light[] lights;  // Array de luces que quieres encender
    public Material materialSinEmision;  // Material sin emisi�n
    public Material materialConEmision;  // Material con emisi�n
    public GameObject[] objectsToChange;  // Los objetos cuyos materiales cambiar�s
    public AudioClip switchSound;  // El sonido de "switch" que se reproducir�
    private AudioSource audioSource;  // El componente AudioSource para reproducir el sonido

    void Start()
    {
        // Aseguramos que el objeto tenga un AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>(); // Si no tiene, lo a�adimos
        }

        // Llamamos a la funci�n para encender las luces y cambiar el material despu�s de 2 segundos
        StartCoroutine(TurnOnLightsAndChangeMaterial(2f));  // Esperamos 2 segundos
    }

    IEnumerator TurnOnLightsAndChangeMaterial(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Activamos todas las luces del array
        foreach (Light light in lights)
        {
            light.enabled = true; // Enciende las luces
        }

        // Cambiamos el material de cada objeto en el array a uno con emisi�n
        foreach (GameObject obj in objectsToChange)
        {
            Renderer renderer = obj.GetComponent<Renderer>();  // Obtener el Renderer del objeto
            if (renderer != null)
            {
                renderer.material = materialConEmision;  // Cambiar el material a uno con emisi�n
            }
        }

        // Reproducimos el sonido de "switch"
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
