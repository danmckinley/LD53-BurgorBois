using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Networking.Types;
using UnityEngine.UIElements;

public class InputHandler : MonoBehaviour
{
    [SerializeField] private float flapPower = 1f;
    [SerializeField] private float flapDirectionPower = 1f;
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float moveSpeedUpdateInterval = 1f;

    private Rigidbody2D rb;
    private PlayerInput playerInput;
    private Vector2 mousePosition;
    
    private void Awake()
    {
        playerInput = new PlayerInput();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        StartCoroutine(GetMoveSpeedRoutine(moveSpeed));
    }

    void OnEnable()
    {
        playerInput.Enable();
        playerInput.Movement.Flap.performed += _ => Flap();
    }

    void Update()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(playerInput.Movement.Mouse.ReadValue<Vector2>());
    }

    private void FixedUpdate()
    {
        rb.transform.rotation = Quaternion.Euler(0, 0,
            Mathf.Atan2(mousePosition.y - rb.position.y, mousePosition.x - rb.position.x) * Mathf.Rad2Deg - 90);
        
        Vector3 dir = (new Vector3(mousePosition.x - rb.transform.position.x, mousePosition.y - rb.transform.position.y,
            rb.transform.position.z));
        
        
        Debug.Log($"Move speed {moveSpeed}");
        
        rb.AddForce(dir.normalized * moveSpeed);
    }

    private IEnumerator GetMoveSpeedRoutine(float prevSpeed)
    {
        /*
         * we could pass in the moduluo to this to calculate how much the cursor has changed to prevent
         * gaining superspeed by flicking down then up so bird follows mouse
         */
        
        float rotation = rb.transform.rotation.eulerAngles.z;
        float swoopMultiplier;
        swoopMultiplier = rotation % 180;
        
        if (rotation > 180)
        {
            swoopMultiplier = 180 - swoopMultiplier;
        }
        
        float relativeMouseSpeed = (float)Math.Pow(swoopMultiplier / 35, 2) * 0.6f;
        
        if (relativeMouseSpeed < prevSpeed)
        {
            yield return new WaitForSeconds(.5f);

            if (moveSpeed - 0.5f <= 0)
            {
                moveSpeed = 0;
            }
            else
            {
                moveSpeed -= 0.5f;
            }
            
        }
        else
        {
            moveSpeed = relativeMouseSpeed;
            yield return new WaitForSeconds(moveSpeedUpdateInterval);
        }
        StartCoroutine(GetMoveSpeedRoutine(moveSpeed));
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