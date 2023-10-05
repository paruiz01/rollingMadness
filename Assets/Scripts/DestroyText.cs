using UnityEngine;
using System.Collections;

public class DestroyText: MonoBehaviour
{
    public GameObject popUpText;
    public KeyCode hideKey = KeyCode.Space;
    public float displayDuration = 10f;

    private void Start()
    {
        // Show the pop-up text when the player spawns
        ShowPopUpText();

        // Start the coroutine to destroy the text box after the display duration
        StartCoroutine(DestroyPopUpTextAfterDelay(displayDuration));
    }

    private void Update()
    {
        // Check if the hide key is pressed to hide the pop-up text
        if (Input.GetKeyDown(hideKey))
        {
            HidePopUpText();
        }
    }

    private void ShowPopUpText()
    {
        // Show the pop-up text
        popUpText.SetActive(true);
    }

    private void HidePopUpText()
    {
        // Hide the pop-up text
        popUpText.SetActive(false);
    }

    private IEnumerator DestroyPopUpTextAfterDelay(float delay)
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(delay);

        // Destroy the pop-up text game object
        Destroy(popUpText);
    }
}