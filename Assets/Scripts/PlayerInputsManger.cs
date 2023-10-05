using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputsManger : MonoBehaviour
{
    public Vector2 move;
    public Vector2 look;
    public bool switchMode;
    public bool sprint;
    public bool jump;
    public bool pause;
    public bool lockCamera;
   void OnMove(InputValue value)
    {
        move =  value.Get<Vector2>();
    }

    void OnLook (InputValue value)
    {
        if(lockCamera)
            return;
        look = value.Get<Vector2>();
    }

    void OnSwitchMode(InputValue value)
    {
        switchMode = value.isPressed;
    }

    void OnSprint (InputValue value)
    {
        sprint = value.isPressed;
    }

    void OnJump (InputValue value)
    {
        jump = value.isPressed;
    }    

    void OnPause (InputValue value)
    {
        pause = value.isPressed;
    }
}
