using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public Animator animator;      // Referencia al Animator
    public string animationTrigger = "PlayAnimation"; // Nombre del Trigger en el Animator
    public float interval = 5f;    // Intervalo de tiempo (en segundos) para activar la animación

    private float timer = 0f;      // Temporizador interno

    void Update()
    {
        // Incrementar el temporizador
        timer += Time.deltaTime;

        // Si el temporizador alcanza el intervalo, activar la animación
        if (timer >= interval)
        {
            // Activar el Trigger para iniciar la animación
            animator.SetTrigger(animationTrigger);

            // Reiniciar el temporizador
            timer = 0f;
        }
    }
}
