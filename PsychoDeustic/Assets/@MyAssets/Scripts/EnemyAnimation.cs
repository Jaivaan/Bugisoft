using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    private Animator animator; // Referencia al componente Animator del enemigo
    private float shakeTimer = 0f; // Temporizador para controlar la animación de "sacudida"
    public float shakeInterval = 3f; // Intervalo de tiempo entre las animaciones de "sacudida"

    private bool isDead = false; // Estado del enemigo (si está muerto o no)

    private Vector3 initialPosition; // Posición inicial del enemigo
    private Quaternion initialRotation; // Rotación inicial del enemigo

    private void Awake()
    {
        // Obtiene el componente Animator del GameObject
        animator = GetComponent<Animator>();
        
        // Guarda la posición y rotación inicial del enemigo
        initialPosition = transform.position;
        initialRotation = transform.rotation;

        // Verifica si el Animator está asignado; si no, imprime un error
        if (animator == null)
        {
            Debug.LogError("No se encontró el Animator en el EnemyAnimationController.");
        }
    }

    void Update()
    {
        // Si el enemigo está muerto, no realiza ninguna acción
        if (isDead) return;

        // Incrementa el temporizador con el tiempo transcurrido desde el último frame
        shakeTimer += Time.deltaTime;

        // Si se alcanza el intervalo de sacudida, dispara la animación y reinicia el temporizador
        if (shakeTimer >= shakeInterval)
        {
            animator.SetTrigger("isShaking"); // Activa el trigger para la animación "isShaking"
            shakeTimer = 0f;
        }
    }

    public void Die()
    {
        // Marca al enemigo como "muerto", deteniendo su animación
        isDead = true;
    }

    public void ReviveEnemy()
    {
        // Revive al enemigo restableciendo su estado inicial
        isDead = false;
        
        // Restablece el Animator a su estado inicial
        animator.Rebind();
        animator.Update(0f);

        // Restablece la posición y rotación iniciales del enemigo
        transform.position = initialPosition;
        transform.rotation = initialRotation;
    }
}
