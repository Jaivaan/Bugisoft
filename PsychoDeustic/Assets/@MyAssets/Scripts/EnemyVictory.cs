using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class WaypointFollower : MonoBehaviour
{
    [Header("Waypoints y movimiento")]
    public Transform[] waypoints;         // Array de waypoints a seguir
    public float speed = 5f;             // Velocidad de movimiento
    public float rotationSpeed = 5f;      // Velocidad de rotación
    public float startDelay = 1f;         // Retraso inicial antes de empezar a moverse

    [Header("Offset de Rotación")]
    [Tooltip("Offset adicional en grados sobre el eje Y. Se suma a la rotación calculada.")]
    public float rotationOffsetY = 90f;   // Ajuste de 90° por defecto

    private int currentWaypointIndex = 0;
    private bool canMove = false;

    [Header("Acciones finales")]
    public Light[] lights;                // Luces que se apagarán al terminar
    public Material materialSinEmision;   // Material sin emisión para aplicar al terminar
    public GameObject[] objectsToChange;  // Objetos a los que se les cambia el material
    public AudioSource switchAudioSource; // Sonido para reproducir al terminar
    public GameObject focusImage;         // Imagen que se desactiva al terminar

    void Start()
    {
        // Empieza a moverse tras 'startDelay' segundos
        Invoke(nameof(StartMoving), startDelay);
    }

    void Update()
    {
        if (canMove && waypoints.Length > 0)
        {
            MoveTowardsWaypoint();
        }
    }

    /// <summary>
    /// Método para activar el movimiento.
    /// </summary>
    void StartMoving()
    {
        canMove = true;
    }

    /// <summary>
    /// Mueve y rota el objeto hacia el waypoint actual.
    /// </summary>
    void MoveTowardsWaypoint()
    {
        // Si hemos superado el último waypoint, no hacemos nada
        if (currentWaypointIndex >= waypoints.Length) return;

        // Tomamos el waypoint actual
        Transform currentWaypoint = waypoints[currentWaypointIndex];

        // Calculamos la dirección hacia él
        Vector3 direction = currentWaypoint.position - transform.position;

        // Si estamos suficientemente cerca, pasamos al siguiente waypoint
        if (direction.magnitude < 0.1f)
        {
            currentWaypointIndex++;
            if (currentWaypointIndex >= waypoints.Length)
            {
                // Ejecutamos las acciones finales
                OnFinishedPath();
            }
            return;
        }

        // Ignoramos la Y para rotar solo en el plano XZ
        Vector3 directionXZ = new Vector3(direction.x, 0f, direction.z);

        // Solo giramos si el vector no es muy pequeño
        if (directionXZ.sqrMagnitude > 0.001f)
        {
            // LookRotation alinea la Z del objeto con directionXZ
            Quaternion targetRotation = Quaternion.LookRotation(directionXZ);

            // Aplicamos un offset adicional en el eje Y
            targetRotation *= Quaternion.Euler(0f, rotationOffsetY, 0f);

            // Suavizamos la rotación
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }

        // Avanzamos hacia el waypoint
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, currentWaypoint.position, step);
    }

    /// <summary>
    /// Función que se llama cuando el objeto ha recorrido todos los waypoints.
    /// </summary>
    void OnFinishedPath()
    {
        // Apaga las luces
        foreach (Light light in lights)
        {
            if (light != null)
                light.enabled = false;
        }

        // Cambia el material de los objetos a uno sin emisión
        foreach (GameObject obj in objectsToChange)
        {
            if (obj != null)
            {
                Renderer renderer = obj.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.material = materialSinEmision;
                }
            }
        }

        // Desactiva la imagen de foco, si está asignada
        if (focusImage != null)
        {
            focusImage.SetActive(false);
        }
        else
        {
            Debug.LogWarning("No se ha asignado la imagen del foco (focusImage).");
        }

        // Reproduce el sonido si existe
        if (switchAudioSource != null)
        {
            switchAudioSource.enabled = true;
            switchAudioSource.Play();
        }
        else
        {
            Debug.LogWarning("No se ha asignado un AudioSource para el sonido del switch.");
        }

        // Cambia de escena tras 2 segundos
        StartCoroutine(LoadInitialSceneAfterDelay(2f));
    }

    /// <summary>
    /// Corrutina que espera un tiempo y luego carga la escena 'menuScene'.
    /// </summary>
    IEnumerator LoadInitialSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("menuScene");
    }
}
