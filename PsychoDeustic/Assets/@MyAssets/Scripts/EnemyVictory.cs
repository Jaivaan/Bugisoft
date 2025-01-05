using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class WaypointFollower : MonoBehaviour
{
    [Header("Waypoints y movimiento")]
    public Transform[] waypoints;         
    public float speed = 5f;             
    public float rotationSpeed = 5f;
    public float startDelay = 1f;

    [Header("Offset de Rotación")]
    [Tooltip("Offset adicional en grados sobre el eje Y. Se suma a la rotación calculada.")]
    public float rotationOffsetY = 90f;

    private int currentWaypointIndex = 0;
    private bool canMove = false;

    [Header("Acciones finales")]
    public Light[] lights;
    public Material materialSinEmision;
    public GameObject[] objectsToChange;
    public AudioSource switchAudioSource;
    public GameObject focusImage;

    void Start()
    {
        Invoke(nameof(StartMoving), startDelay);
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

        Transform currentWaypoint = waypoints[currentWaypointIndex];

        Vector3 direction = currentWaypoint.position - transform.position;

        if (direction.magnitude < 0.1f)
        {
            currentWaypointIndex++;
            if (currentWaypointIndex >= waypoints.Length)
            {
                OnFinishedPath();
            }
            return;
        }

        Vector3 directionXZ = new Vector3(direction.x, 0f, direction.z);

        if (directionXZ.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionXZ);

            targetRotation *= Quaternion.Euler(0f, rotationOffsetY, 0f);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }

        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, currentWaypoint.position, step);
    }

    void OnFinishedPath()
    {
        foreach (Light light in lights)
        {
            if (light != null)
                light.enabled = false;
        }

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

        if (focusImage != null)
        {
            focusImage.SetActive(false);
        }
        else
        {
            Debug.LogWarning("No se ha asignado la imagen del foco (focusImage).");
        }

        if (switchAudioSource != null)
        {
            switchAudioSource.enabled = true;
            switchAudioSource.Play();
        }
        else
        {
            Debug.LogWarning("No se ha asignado un AudioSource para el sonido del switch.");
        }

        StartCoroutine(LoadInitialSceneAfterDelay(2f));
    }

    IEnumerator LoadInitialSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("menuScene");
    }
}
