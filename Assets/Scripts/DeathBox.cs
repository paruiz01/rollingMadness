using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBox : MonoBehaviour
{
    public Camera playerCamera; // Reference to the player camera

    private void OnTriggerEnter(Collider other)
    {
        // Destroy the player object
        Destroy(other.gameObject);

        // Reset the camera position and rotation
        if (playerCamera != null)
        {
            playerCamera.transform.position = Vector3.zero;
            playerCamera.transform.rotation = Quaternion.identity;
        }

        // Restart the current scene
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
}