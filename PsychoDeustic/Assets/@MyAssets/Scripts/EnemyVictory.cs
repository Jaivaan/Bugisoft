using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class WaypointFollower : MonoBehaviour
{
    [Header("Waypoints y movimiento")]
    public Transform[] waypoints; // Lista de puntos por los que se moverá el objeto
    public float speed = 5f; // Velocidad de movimiento
    public float rotationSpeed = 5f; // Velocidad de rotación hacia el siguiente waypoint
    public float startDelay = 1f; // Retraso inicial antes de comenzar el movimiento

    [Header("Offset de Rotación")]
    [Tooltip("Offset adicional en grados sobre el eje Y. Se suma a la rotación calculada.")]
    public float rotationOffsetY = 90f; // Ajuste adicional de rotación en el eje Y

    private int currentWaypointIndex = 0; // Índice del waypoint actual
    private bool canMove = false; // Controla si el objeto puede moverse

    [Header("Acciones finales")]
    public Light[] lights; // Luces que se apagarán al finalizar el camino
    public Material materialSinEmision; // Material que se asignará a objetos sin emisión
    public GameObject[] objectsToChange; // Lista de objetos a los que se les cambiará el material
    public AudioSource switchAudioSource; // Fuente de audio para reproducir un sonido al terminar
    public GameObject focusImage; // Imagen que se desactivará al finalizar

    void Start()
    {
        // Inicia el movimiento después del retraso especificado
        Invoke(nameof(StartMoving), startDelay);
    }

    void Update()
    {
        // Mueve el objeto hacia el waypoint actual si está habilitado
        if (canMove && waypoints.Length > 0)
        {
            MoveTowardsWaypoint();
        }
    }

    void StartMoving()
    {
        // Permite que el objeto comience a moverse
        canMove = true;
    }

    void MoveTowardsWaypoint()
    {
        // Si todos los waypoints han sido alcanzados, no hace nada
        if (currentWaypointIndex >= waypoints.Length) return;

        Transform currentWaypoint = waypoints[currentWaypointIndex];

        // Calcula la dirección hacia el waypoint actual
        Vector3 direction = currentWaypoint.position - transform.position;

        // Si está lo suficientemente cerca del waypoint, pasa al siguiente
        if (direction.magnitude < 0.1f)
        {
            currentWaypointIndex++;
            if (currentWaypointIndex >= waypoints.Length)
            {
                OnFinishedPath(); // Llama a la acción final si se han recorrido todos los waypoints
            }
            return;
        }

        // Ajusta la dirección para que no afecte el eje Y
        Vector3 directionXZ = new Vector3(direction.x, 0f, direction.z);

        // Rota el objeto hacia el waypoint
        if (directionXZ.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionXZ);
            targetRotation *= Quaternion.Euler(0f, rotationOffsetY, 0f); // Aplica el offset de rotación
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        // Mueve el objeto hacia el waypoint
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, currentWaypoint.position, step);
    }

    void OnFinishedPath()
    {
        // Apaga todas las luces asignadas
        foreach (Light light in lights)
        {
            if (light != null)
                light.enabled = false;
        }

        // Cambia el material de los objetos asignados
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

        // Desactiva la imagen de foco si está asignada
        if (focusImage != null)
        {
            focusImage.SetActive(false);
        }
        else
        {
            Debug.LogWarning("No se ha asignado la imagen del foco (focusImage).");
        }

        // Reproduce el sonido del switch si está asignado
        if (switchAudioSource != null)
        {
            switchAudioSource.enabled = true;
            switchAudioSource.Play();
        }
        else
        {
            Debug.LogWarning("No se ha asignado un AudioSource para el sonido del switch.");
        }

        // Inicia la carga de la escena inicial después de un retraso
        StartCoroutine(LoadInitialSceneAfterDelay(2f));
    }

    IEnumerator LoadInitialSceneAfterDelay(float delay)
    {
        // Espera un tiempo antes de cargar la escena inicial
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("menuScene");
    }
}
