using UnityEngine;

public class PushObject : MonoBehaviour
{
    public float pushForce = 10f;
    public float pushDuration = 1f;
    private bool isPushing = false;
    private float pushTimer = 0f;
    private CharacterController playerController;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the collided object has a character controller (e.g., the player)
        playerController = other.GetComponent<CharacterController>();
        if (playerController != null)
        {
            isPushing = true;
            pushTimer = 0f;
        }
    }

    private void Update()
    {
        if (isPushing)
        {
            pushTimer += Time.deltaTime;
            if (pushTimer <= pushDuration)
            {
                // Calculate the push direction (away from the pushing object)
                Vector3 pushDirection = playerController.transform.position - transform.position;
                pushDirection.Normalize();

                // Apply the push force as movement to the character controller
                playerController.Move(pushDirection * pushForce * Time.deltaTime);
            }
            else
            {
                isPushing = false;
            }
        }
    }
}