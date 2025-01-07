using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    private Animator animator; // Referencia al componente Animator del enemigo
    private float shakeTimer = 0f; // Temporizador para controlar la animaci�n de "sacudida"
    public float shakeInterval = 3f; // Intervalo de tiempo entre las animaciones de "sacudida"

    private bool isDead = false; // Estado del enemigo (si est� muerto o no)

    private Vector3 initialPosition; // Posici�n inicial del enemigo
    private Quaternion initialRotation; // Rotaci�n inicial del enemigo

    private void Awake()
    {
        // Obtiene el componente Animator del GameObject
        animator = GetComponent<Animator>();
        
        // Guarda la posici�n y rotaci�n inicial del enemigo
        initialPosition = transform.position;
        initialRotation = transform.rotation;

        // Verifica si el Animator est� asignado; si no, imprime un error
        if (animator == null)
        {
            Debug.LogError("No se encontr� el Animator en el EnemyAnimationController.");
        }
    }

    void Update()
    {
        // Si el enemigo est� muerto, no realiza ninguna acci�n
        if (isDead) return;

        // Incrementa el temporizador con el tiempo transcurrido desde el �ltimo frame
        shakeTimer += Time.deltaTime;

        // Si se alcanza el intervalo de sacudida, dispara la animaci�n y reinicia el temporizador
        if (shakeTimer >= shakeInterval)
        {
            animator.SetTrigger("isShaking"); // Activa el trigger para la animaci�n "isShaking"
            shakeTimer = 0f;
        }
    }

    public void Die()
    {
        // Marca al enemigo como "muerto", deteniendo su animaci�n
        isDead = true;
    }

    public void ReviveEnemy()
    {
        // Revive al enemigo restableciendo su estado inicial
        isDead = false;
        
        // Restablece el Animator a su estado inicial
        animator.Rebind();
        animator.Update(0f);

        // Restablece la posici�n y rotaci�n iniciales del enemigo
        transform.position = initialPosition;
        transform.rotation = initialRotation;
    }
}
