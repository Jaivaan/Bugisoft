using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartGame : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "RestartCube")
        {
            SceneManager.LoadScene("menuScene");
            Debug.Log("D");
        }
    }
}