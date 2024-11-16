using UnityEngine;

public class ArmController : MonoBehaviour
{
    [Header("Objetivos de los Controladores")]
    public Transform leftHandTarget;  // Objeto que representa la posici�n del mando izquierdo
    public Transform rightHandTarget; // Objeto que representa la posici�n del mando derecho

    [Header("Huesos del Modelo del Personaje")]
    public Transform leftHandBone;   // Hueso de la mano izquierda del personaje
    public Transform rightHandBone;  // Hueso de la mano derecha del personaje

    [Header("Opciones de Suavizado")]
    public float positionLerpSpeed = 10f; // Velocidad de interpolaci�n para la posici�n
    public float rotationLerpSpeed = 10f; // Velocidad de interpolaci�n para la rotaci�n

    void Update()
    {
        // Actualiza la posici�n y rotaci�n del hueso de la mano izquierda
        if (leftHandTarget != null && leftHandBone != null)
        {
            leftHandBone.position = Vector3.Lerp(leftHandBone.position, leftHandTarget.position, Time.deltaTime * positionLerpSpeed);
            leftHandBone.rotation = Quaternion.Lerp(leftHandBone.rotation, leftHandTarget.rotation, Time.deltaTime * rotationLerpSpeed);
        }

        // Actualiza la posici�n y rotaci�n del hueso de la mano derecha
        if (rightHandTarget != null && rightHandBone != null)
        {
            rightHandBone.position = Vector3.Lerp(rightHandBone.position, rightHandTarget.position, Time.deltaTime * positionLerpSpeed);
            rightHandBone.rotation = Quaternion.Lerp(rightHandBone.rotation, rightHandTarget.rotation, Time.deltaTime * rotationLerpSpeed);
        }
    }
}
