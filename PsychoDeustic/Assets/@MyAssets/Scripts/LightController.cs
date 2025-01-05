using UnityEngine;
using System.Collections;

public class LightController : MonoBehaviour
{
    public Light[] lights;
    public Material materialSinEmision;
    public Material materialConEmision;
    public GameObject[] objectsToChange;
    public AudioClip switchSound;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        StartCoroutine(TurnOnLightsAndChangeMaterial(2f));
    }

    IEnumerator TurnOnLightsAndChangeMaterial(float delay)
    {
        yield return new WaitForSeconds(delay);

        foreach (Light light in lights)
        {
            light.enabled = true;
        }

        foreach (GameObject obj in objectsToChange)
        {
            Renderer renderer = obj.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material = materialConEmision;
            }
        }

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
