using UnityEngine;

public class ArmController : MonoBehaviour
{
    [Header("Objetivos de los Controladores")]
    public Transform leftHandTarget;  // Objeto que representa la posición del mando izquierdo
    public Transform rightHandTarget; // Objeto que representa la posición del mando derecho

    [Header("Huesos del Modelo del Personaje")]
    public Transform leftHandBone;   // Hueso de la mano izquierda del personaje
    public Transform rightHandBone;  // Hueso de la mano derecha del personaje

    [Header("Opciones de Suavizado")]
    public float positionLerpSpeed = 10f; // Velocidad de interpolación para la posición
    public float rotationLerpSpeed = 10f; // Velocidad de interpolación para la rotación

    void Update()
    {
        // Actualiza la posición y rotación del hueso de la mano izquierda
        if (leftHandTarget != null && leftHandBone != null)
        {
            leftHandBone.position = Vector3.Lerp(leftHandBone.position, leftHandTarget.position, Time.deltaTime * positionLerpSpeed);
            leftHandBone.rotation = Quaternion.Lerp(leftHandBone.rotation, leftHandTarget.rotation, Time.deltaTime * rotationLerpSpeed);
        }

        // Actualiza la posición y rotación del hueso de la mano derecha
        if (rightHandTarget != null && rightHandBone != null)
        {
            rightHandBone.position = Vector3.Lerp(rightHandBone.position, rightHandTarget.position, Time.deltaTime * positionLerpSpeed);
            rightHandBone.rotation = Quaternion.Lerp(rightHandBone.rotation, rightHandTarget.rotation, Time.deltaTime * rotationLerpSpeed);
        }
    }
}
