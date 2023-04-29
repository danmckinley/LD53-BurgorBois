using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{

    [SerializeField] private InputActionReference movement;
    private Vector2 movementinput;
    private Rigidbody2D rb; 
    private PlayerInput controller;
    private void Awake()
    {
       controller = new PlayerInput();
        rb = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        controller.Movement.FLAP.performed += _ => Flap();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Flap()
    { // DO DA FLAP
    Debug.Log("flap");
    rb.AddForce(new Vector3(0f,10f,0f));
    }
}
