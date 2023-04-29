using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    private Vector2 movementinput;
    private Rigidbody2D rb;
    private PlayerInput playerInput;

    private void Awake()
    {
        playerInput = new PlayerInput();
        rb = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        playerInput.Enable();
        playerInput.Movement.FLAP.performed += _ => Flap();
    }
    
    void Update()
    {
        Debug.Log("I'm working");
    }

    private void Flap()
    {
        // DO DA FLAP
        Debug.Log("flap");
        rb.AddForce(new Vector3(0f, 1000f, 0f));
    }
}