using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    private Animator animator;
    private float shakeTimer = 0f;
    public float shakeInterval = 3f;

    private bool isDead = false;

    private Vector3 initialPosition;
    private Quaternion initialRotation;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        initialPosition = transform.position;
        initialRotation = transform.rotation;
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

    public void ReviveEnemy()
    {
        isDead = false;
        animator.Rebind();
        animator.Update(0f);

        transform.position = initialPosition;
        transform.rotation = initialRotation;
    }

}
