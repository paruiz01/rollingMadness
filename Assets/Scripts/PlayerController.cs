using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float jumpForce;
    public float rollTorque;
    public float acceleration;
    public float boostTimer;
    public bool boosting;
    public float jumpBoostTimer;
    public bool jumpBoosting;
    public Text countText;
    public Text winText;
    public GameObject Wall;
    public float groundDistance = 0.2f;
    public Camera playerCamera;
    private Vector2 GamepadInput;
    private Rigidbody rb;
    private bool isGrounded = true;
    private float currentSpeed;

    private int count;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;
        SetCountText();
        winText.text = "";

        speed = 6;
        boostTimer = 0;
        boosting = false;

        playerCamera = Camera.main;
    }

    private void Update()
    {
        Debug.Log("Speed: " + speed);
        Debug.Log("Acceleration: " + acceleration);
        GamepadInput = Gamepad.current != null ? Gamepad.current.leftStick.ReadValue() : Vector2.zero;

        // Convert the input direction to be relative to the camera's view
        Vector3 cameraForward = playerCamera.transform.forward;
        cameraForward.y = 0f; // Ignore vertical component for flat movement
        Vector3 moveDirection = Quaternion.LookRotation(cameraForward) * new Vector3(GamepadInput.x, 0f, GamepadInput.y);

        // Jump
        if ((Input.GetButtonDown("Jump") || Gamepad.current.buttonSouth.wasPressedThisFrame) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }

        // Restart level
        if (Input.GetKeyDown(KeyCode.R))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }

        // Quit game
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        // Smooth acceleration
        currentSpeed = Mathf.Lerp(currentSpeed, speed, acceleration * Time.deltaTime);

        //Boost
        if (boosting)
        {
            boostTimer += Time.deltaTime;
            if (boostTimer > 3)
            {
                speed = 6;
                boostTimer = 0;
                boosting = false;
            }
        }

        //Jump boost
        if (jumpBoosting)
        {
            jumpBoostTimer += Time.deltaTime;
            if (jumpBoostTimer > 3)
            {
                jumpForce = 3;
                jumpBoostTimer = 0;
                jumpBoosting = false;

            }
        }

    }

    private void FixedUpdate()
    {
        // Smooth acceleration
        currentSpeed = Mathf.Lerp(currentSpeed, speed, acceleration * Time.fixedDeltaTime);

        // Get the camera's forward direction
        Vector3 cameraForward = playerCamera.transform.forward;
        cameraForward.y = 0f; // Ignore vertical component for flat movement
        cameraForward.Normalize();

        // Calculate the movement direction based on the camera's forward and gamepad input
        Vector3 movement = cameraForward * GamepadInput.y + playerCamera.transform.right * GamepadInput.x;
        movement.Normalize();

        // Move the player based on the movement direction
        rb.MovePosition(rb.position + movement * currentSpeed * Time.fixedDeltaTime);

        // Apply torque for rolling
        if (isGrounded)
        {
            Vector3 rollAxis = Vector3.Cross(Vector3.up, movement);
            rb.AddTorque(rollAxis * rollTorque);
        }


        // Check if the player is grounded
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, groundDistance))
        {
            if (hit.collider.CompareTag("ground"))
            {
                isGrounded = true;
            }
            else
            {
                isGrounded = false;
            }
        }
        else
        {
            isGrounded = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("collectible"))
        {
            other.gameObject.SetActive(false);
            count++;
            SetCountText();
            if (count >= 3)
            {
                Wall.gameObject.SetActive(false);
            }
        }
        if (other.gameObject.CompareTag("danger"))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }

        if (other.gameObject.CompareTag("speedBoost"))
        {
            boosting = true;
            speed = 30;
            Destroy(other.gameObject);
        }

        if (other.gameObject.CompareTag("jumpBoost"))
        {
            jumpBoosting = true;
            jumpForce = 30;
            Destroy(other.gameObject);
        }
    }

    private void SetCountText()
    {
        countText.text = "Count: " + count.ToString();

        if (count >= 12)
        {
            winText.text = "You Win! Press R to restart or ESC to exit";
        }
    }
}