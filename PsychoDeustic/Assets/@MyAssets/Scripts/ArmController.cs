using UnityEngine;

public class ArmController : MonoBehaviour
{
    [Header("Objetivos de los Controladores")]
    public Transform leftHandTarget; 
    public Transform rightHandTarget; 

    [Header("Huesos del Modelo del Personaje")]
    public Transform leftHandBone;  
    public Transform rightHandBone;  

    [Header("Opciones de Suavizado")]
    public float positionLerpSpeed = 10f; 
    public float rotationLerpSpeed = 10f;

    void Update()
    {
       
        if (leftHandTarget != null && leftHandBone != null)
        {
            leftHandBone.position = Vector3.Lerp(leftHandBone.position, leftHandTarget.position, Time.deltaTime * positionLerpSpeed);
            leftHandBone.rotation = Quaternion.Lerp(leftHandBone.rotation, leftHandTarget.rotation, Time.deltaTime * rotationLerpSpeed);
        }

        
        if (rightHandTarget != null && rightHandBone != null)
        {
            rightHandBone.position = Vector3.Lerp(rightHandBone.position, rightHandTarget.position, Time.deltaTime * positionLerpSpeed);
            rightHandBone.rotation = Quaternion.Lerp(rightHandBone.rotation, rightHandTarget.rotation, Time.deltaTime * rotationLerpSpeed);
        }
    }
}
