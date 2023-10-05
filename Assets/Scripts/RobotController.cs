using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class RobotController : MonoBehaviour
{
    [SerializeField] GameObject mainCam;
    [SerializeField] float moveSpeed = 8;
    [SerializeField] float sprintSpeed = 11;
    [SerializeField] float rollSpeed = 15;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float gravity = -10;
    [SerializeField] float jumpHeight = 2;
    [SerializeField] Transform cameraFollowTarget;

    private bool isGrounded;
    private bool ballMode = false;
    private bool boosting;
    private bool jumpBoosting;
    private float boostTimer;
    private float jumpBoostTimer;
    private int count;

    private Vector3 velocity;
    private float xRotation;
    private float yRotation;

    private PlayerInputsManger input;
    private CharacterController controller;
    private Animator animator;
    private AudioSource audioSource;

    public Text countText;
    public Text winText;
    public GameObject Wall;
    public AudioClip collectibleSound;

    // Start is called before the first frame update
    void Start()
    {
        input = GetComponent<PlayerInputsManger>();
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        SwitchMode();
        if (ballMode)
            MoveBall();
        else
            MoveNormal();

        JumpAndGravity();

        if (Input.GetKeyDown(KeyCode.R))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }

        if (boosting)
        {
            boostTimer += Time.deltaTime;
            if (boostTimer >= 3)
            {
                rollSpeed = 15;
                boostTimer = 0;
                boosting = false;
                Debug.Log("Boost Timer Reset");
            }
            else
            {
                Debug.Log("Boost Timer: " + boostTimer);
            }
        }

        //Jump boost
        if (jumpBoosting)
        {
            jumpBoostTimer += Time.deltaTime;
            if (jumpBoostTimer > 3)
            {
                jumpHeight = 2;
                jumpBoostTimer = 0;
                jumpBoosting = false;

            } else
            {
                Debug.Log("Jump boost timer: " + jumpBoostTimer);
            }
        }
    }

    private void LateUpdate()
    {
        CameraRotation();
    }

    void CameraRotation()
    {
        xRotation += input.look.y;
        yRotation += input.look.x;
        xRotation = Mathf.Clamp(xRotation, -10, 90);
        Quaternion rotation = Quaternion.Euler(xRotation, yRotation, 0);
        cameraFollowTarget.rotation = rotation;
    }

    void MoveNormal()
    {
        float speed = 0;
        Vector3 inputDir = new Vector3(input.move.x, 0, input.move.y);
        float targetRotation = 0;

        if (input.move != Vector2.zero)
        {
            speed = input.sprint ? sprintSpeed : moveSpeed;
            targetRotation = Quaternion.LookRotation(inputDir).eulerAngles.y + mainCam.transform.rotation.eulerAngles.y;
            Quaternion rotation = Quaternion.Euler(0, targetRotation, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 20 * Time.deltaTime);
            animator.SetFloat("speed", input.sprint ? 2 : input.move.magnitude);
        }
        else
        {
            animator.SetFloat("speed", 0);
        }

        Vector3 targetDirection = Quaternion.Euler(0, targetRotation, 0) * Vector3.forward;
        controller.Move(targetDirection * speed * Time.deltaTime);
    }

    void MoveBall()
    {
        float speed = 0;
        Vector3 inputDir = new Vector3(input.move.x, 0, input.move.y);
        float targetRotation = 0;
        if (input.move != Vector2.zero)
        {
            speed = rollSpeed;
            targetRotation = Quaternion.LookRotation(inputDir).eulerAngles.y + mainCam.transform.rotation.eulerAngles.y;
            Quaternion rotation = Quaternion.Euler(0, targetRotation, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 20 * Time.deltaTime);
        }
        animator.SetFloat("rollSpeed", input.move.magnitude);
        Vector3 targetDirection = Quaternion.Euler(0, targetRotation, 0) * Vector3.forward;
        controller.Move(targetDirection * speed * Time.deltaTime);
    }

    void SwitchMode()
    {
        if (input.switchMode)
        {
            ballMode = !ballMode;
            input.switchMode = false;
            animator.SetBool("ballMode", ballMode);
        }
    }

    void JumpAndGravity()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, .2f, groundLayer);
        if (isGrounded)
        {
            if (input.jump && ballMode)
            {
                velocity.y = MathF.Sqrt(jumpHeight * 2 * -gravity);
                input.jump = false;
            }
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
        }
        controller.Move(velocity * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("collectible"))
        {

            other.gameObject.SetActive(false);
            count++;
            SetCountText();
            audioSource.Play();
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
            if (!boosting)
            {
                boosting = true;
                rollSpeed = 40;
                Destroy(other.gameObject);
                boostTimer = 0;
            }
            else
            {
                rollSpeed = 40;
                Destroy(other.gameObject);
                boostTimer = 0;
                Debug.Log("Boost Timer Reset");
            }
        }

        if (other.gameObject.CompareTag("jumpBoost"))
        {
            if (!jumpBoosting)
            {
                jumpBoosting = true;
                jumpHeight = 20;
                Destroy(other.gameObject);
                jumpBoostTimer = 0;
            } else
            {
                jumpHeight = 20;
                Destroy(other.gameObject);
                jumpBoostTimer = 0;
                Debug.Log("Jump Boost Reset");
            }
        }
    }

    private void SetCountText()
    {
        countText.text = "Count: " + count.ToString();

        if (count >= 5)
        {
            winText.text = "You Win! Next level loading...";
            StartCoroutine(ChangeAfter2SecondsCoroutine());


        }
    }

    IEnumerator ChangeAfter2SecondsCoroutine()
    {
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}


