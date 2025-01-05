using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    private Animator animator;
    private float shakeTimer = 0f;
    public float shakeInterval = 3f;

    private bool isDead = false;

    private void Awake()
    {
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
        if (isDead) return;

        shakeTimer += Time.deltaTime;

        if (shakeTimer >= shakeInterval)
        {
            animator.SetTrigger("isShaking");
            shakeTimer = 0f;
        }
    }

    public void Die()
    {
        isDead = true;
    }
}
