using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    [SerializeField] private float flapPower = 1f;
    [SerializeField] private float flapDirectionPower = 1f;
    
    [SerializeField] private float moveSpeed = 1f;
    
    private Rigidbody2D rb;
    private PlayerInput playerInput;
    private Vector2 mousePosition;


    
    private void Awake()
    {
        playerInput = new PlayerInput();
        rb = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        playerInput.Enable();
        playerInput.Movement.Flap.performed += _ => Flap();
    }
    
    void Update()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(playerInput.Movement.Mouse.ReadValue<Vector2>());
        
        if (rb.transform.rotation.eulerAngles.z < 270 && rb.transform.rotation.eulerAngles.z > 90f)
        {
            Debug.Log("zoom");
            //Vector2 newPosition = (new Vector2 (mousePosition.x - rb.transform.position.x , mousePosition.y - rb.transform.position.y)).normalize;
            //Vector3 dir = new Vector3(newPosition.x , newPosition.y , rb.transform.position.z);
            //Vector3 dir=(new Vector3 (mousePosition.x - rb.transform.position.x , mousePosition.y - rb.transform.position.y , rb.transform.position.z)).Normalize;
            rb.AddForce(transform.forward*((float)Math.Pow(2,rb.transform.position.z)));
        }
        Debug.Log(mousePosition.x +" "+ mousePosition.y);
    } 

    private void FixedUpdate()
    {
        rb.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(mousePosition.y - rb.position.y, mousePosition.x - rb.position.x) * Mathf.Rad2Deg -  90);
    }

    private void Flap()
    {
        // DO DA FLAP
        Debug.Log("flap");
        if (rb.position.x > mousePosition.x)
        {
            rb.AddForce(new Vector3(-flapDirectionPower, flapPower, 0f));
        }
        else
        {
            rb.AddForce(new Vector3(flapDirectionPower, flapPower, 0f));
        }

        
    }
}
    