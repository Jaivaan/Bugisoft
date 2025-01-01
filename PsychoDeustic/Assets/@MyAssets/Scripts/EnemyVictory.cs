using UnityEngine;
using UnityEngine.SceneManagement; // Importar para el cambio de escenas
using System.Collections;

public class FollowWaypoints : MonoBehaviour
{
    public Transform[] waypoints; // Array de waypoints a seguir
    public float speed = 5f;      // Velocidad de movimiento
    public float rotationSpeed = 5f; // Velocidad de rotación en el eje Y
    public float delay = 1f;      // Retraso inicial antes de comenzar
    private int currentWaypointIndex = 0; // Índice del waypoint actual
    private bool canMove = false; // Controla si puede moverse

    // Control de luces
    public Light[] lights;  // Array de luces que quieres controlar
    public Material materialSinEmision;  // Material sin emisión
    public GameObject[] objectsToChange;  // Los objetos cuyos materiales cambiarás
    public AudioSource switchAudioSource; // AudioSource que reproducirá el sonido del switch
    public GameObject focusImage; // Imagen del foco rojo en el Canvas (o el propio Canvas)

    private const float fixedXRotation = -89.98f; // Rotación fija en el eje X

    void Start()
    {
        // Comenzar el movimiento tras el retraso
        Invoke("StartMoving", delay);
    }

    void Update()
    {
        if (canMove && waypoints.Length > 0)
        {
            MoveTowardsWaypoint();
        }
    }

    void StartMoving()
    {
        canMove = true;
    }

    void MoveTowardsWaypoint()
    {
        if (currentWaypointIndex >= waypoints.Length) return;

        // Punto objetivo actual
        Transform targetWaypoint = waypoints[currentWaypointIndex];

        // Dirección hacia el waypoint
        Vector3 direction = (targetWaypoint.position - transform.position).normalized;

        // Calcula el ángulo de rotación en el eje Y
        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        float smoothedAngle = Mathf.LerpAngle(transform.eulerAngles.y, targetAngle, rotationSpeed * Time.deltaTime);

        // Aplica solo la rotación en el eje Y, manteniendo el eje X fijo
        transform.rotation = Quaternion.Euler(fixedXRotation, smoothedAngle, 0f);

        // Mueve hacia el waypoint
        transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, speed * Time.deltaTime);

        // Comprueba si ha alcanzado el waypoint
        if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.1f)
        {
            currentWaypointIndex++; // Ir al siguiente waypoint

            // Si no hay más waypoints, apaga las luces, desactiva el foco y reproduce el sonido
            if (currentWaypointIndex >= waypoints.Length)
            {
                TurnOffLightsAndFocus(); // Realiza todas las acciones finales
            }
        }
    }

    void TurnOffLightsAndFocus()
    {
        // Apagamos todas las luces del array
        foreach (Light light in lights)
        {
            light.enabled = false; // Apaga las luces
        }

        // Cambiamos el material de cada objeto en el array a uno sin emisión
        foreach (GameObject obj in objectsToChange)
        {
            Renderer renderer = obj.GetComponent<Renderer>();  // Obtener el Renderer del objeto
            if (renderer != null)
            {
                renderer.material = materialSinEmision;  // Cambiar el material a uno sin emisión
            }
        }

        // Desactiva la imagen del foco rojo en el Canvas
        if (focusImage != null)
        {
            focusImage.SetActive(false);
        }
        else
        {
            Debug.LogWarning("No se ha asignado el foco rojo en el inspector.");
        }

        // Reproduce el sonido de switch
        PlaySwitchSound();

        // Cambiar de escena tras 2 segundos
        StartCoroutine(LoadInitialSceneAfterDelay(2f));
    }

    void PlaySwitchSound()
    {
        if (switchAudioSource != null)
        {
            switchAudioSource.enabled = true; // Asegura que el AudioSource está activo
            switchAudioSource.Play(); // Reproduce el sonido
        }
        else
        {
            Debug.LogWarning("No se ha asignado un AudioSource para el sonido del switch.");
        }
    }

    IEnumerator LoadInitialSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        SceneManager.LoadScene("menuScene");
    }
}
