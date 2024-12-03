using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    private Animator animator;  // Referencia al Animator
    private float shakeTimer = 0f;  // Temporizador para activar el Shake
    public float shakeInterval = 3f;  // Intervalo entre shakes

    // Flag para controlar si el enemigo está vivo o muerto
    private bool isDead = false;

    private void Awake()
    {
        // Obtener la referencia al Animator si no está asignada
        if (animator == null)
        {
            animator = GetComponent<Animator>();
            if (animator == null)
            {
                Debug.LogError("No se encontró el Animator en el EnemyAnimationController.");
            }
        }
    }

    void Update()
    {
        if (isDead) return; // Si el enemigo está muerto, no realizar más acciones

        // Incrementa el temporizador en cada frame
        shakeTimer += Time.deltaTime;

        // Si el temporizador alcanza el intervalo de tiempo
        if (shakeTimer >= shakeInterval)
        {
            // Activa el trigger para que se inicie la animación Shake
            animator.SetTrigger("isShaking");

            // Reinicia el temporizador para el siguiente shake
            shakeTimer = 0f;
        }
    }

    // Método público para marcar al enemigo como muerto
    public void Die()
    {
        isDead = true;
        // Opcional: Puedes desactivar el script si prefieres
        // this.enabled = false;
    }
}
