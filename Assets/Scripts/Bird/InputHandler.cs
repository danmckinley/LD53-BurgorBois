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
        //rb.gravityScale = 0f;
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
        // todo : remove hard coded rotation towards mouse - apply it as a rotational force instead?
        //rb.transform.rotation = Quaternion.Euler(0, 0,
        //    Mathf.Atan2(mousePosition.y - rb.position.y, mousePosition.x - rb.position.x) * Mathf.Rad2Deg - 90);

        // torque - difference between current rotation and desired rotation
        // Vector3 mouseDir = (new Vector3(mousePosition.x - rb.transform.position.x, mousePosition.y - rb.transform.position.y,
        //    rb.transform.position.z));
        
        //Vector2 rb.transform.right.x - mousePosition.x
        //float roll = Vector2.Angle((rb.transform.right), (mousePosition)); // figure out how to make torque
        //transform.Rotate(transform.right, roll * Time.deltaTime * -10, Space.World);

        //Vector2 mouseDir = new Vector2(mousePosition.x - rb.transform.position.x, mousePosition.y - rb.transform.position.y);
        float mouseDistX = mousePosition.x - rb.transform.position.x;
        float mouseDistY = mousePosition.y - rb.transform.position.y;
        float angle = Mathf.Atan2(mouseDistX, mouseDistY) * Mathf.Rad2Deg; // desired rotation

        //float currentRotation = rb.
        transform.Rotate(transform.forward, angle * Time.deltaTime * -10, Space.Self);

        // gravity 
        rb.velocity -= Vector2.up * Time.deltaTime;

        // transform vertical velocity into horizontal velocity
        Vector2 vertVel = rb.velocity - ((Vector2)(Vector3.Project(transform.up, rb.velocity)));
        rb.velocity -= vertVel * Time.deltaTime;
        rb.velocity = vertVel.magnitude * transform.right * Time.deltaTime / 10;

        // drag
        Vector2 drag = rb.velocity - (Vector2)(Vector3.Project(transform.right, rb.velocity));
        rb.AddForce( -drag * drag.magnitude * Time.deltaTime / 1000);

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