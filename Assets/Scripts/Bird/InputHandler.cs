using System;
using System.Collections;
using System.Collections.Generic;
using Bird;
using Unity.Mathematics;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Networking.Types;
using UnityEngine.UIElements;

public class InputHandler : MonoBehaviour
{
    
    [SerializeField] private float rotationResponseModifier = 500f;
    [SerializeField] private float flapPower = 1f;
    [SerializeField] private float flapDirectionPower = 1f;
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float moveSpeedUpdateInterval = 1f;
    [SerializeField] private int flapChargeAmount = 3;
    [SerializeField] private FlapCharge objectToSpawn;

    private Rigidbody2D rb;
    private PlayerInput playerInput;
    private Vector2 mousePosition;
    private SpriteRenderer spriteRenderer;
    private Queue<FlapCharge> flapCharges;
    private Queue<Vector3> chargePos;
    public float tilt;

    private void Awake()
    {
        playerInput = new PlayerInput();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        flapCharges = new Queue<FlapCharge>();
        chargePos = new Queue<Vector3>();

        float offset = 0f;
        
        for (int i = 0; i < flapChargeAmount; i++)
        {
            var flap = Instantiate(objectToSpawn);
            var basePos = flap.transform.position;
            flap.transform.parent = Camera.main.transform;
            flap.transform.position = basePos + new Vector3(offset, 0, 0);
            offset += 1f;
            flapCharges.Enqueue(flap);
        }
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
    // float mouseDistX = mousePosition.x - rb.transform.position.x;
    // float mouseDistY = mousePosition.y - rb.transform.position.y;
    // float angle = Mathf.Atan2(mouseDistX, mouseDistY) * Mathf.Rad2Deg; // desired rotation

    //float currentRotation = rb.
    // transform.Rotate(transform.forward, angle * Time.deltaTime * -10, Space.Self);

    // transform vertical velocity into horizontal velocity
    // Vector2 vertVel = rb.velocity - ((Vector2)(Vector3.Project(transform.up, rb.velocity)));
    // rb.velocity -= vertVel * Time.deltaTime;
    // rb.velocity = vertVel.magnitude * transform.right * Time.deltaTime / 10;

    // drag
    // Vector2 drag = rb.velocity - (Vector2)(Vector3.Project(transform.right, rb.velocity));
    // rb.AddForce( -drag * drag.magnitude * Time.deltaTime / 1000);

    private void FixedUpdate()
    {
        // Set Gravity
        // rb.velocity -= (Vector2.up * Time.deltaTime) * 3;

        // Get Mouse Direction
        Vector3 mouseDir = (new Vector3(mousePosition.x - rb.transform.position.x,
            mousePosition.y - rb.transform.position.y,
            rb.transform.position.z));

        FacePlayerToMouse();
        HandleGliding();
        AdjustPlayerFacingDirection();
    }

    private void HandleGliding()
    {
        //tilt = Mathf.Clamp (energy / 100, -8f, 5f);
        //tilt /= Time.deltaTime * 10;
        //if (((Vector2)transform.forward + rb.velocity.normalized).magnitude < 1.4)
        //    tilt += 3f;
 
        //if (tilt != 0)
        //    transform.Rotate (new Vector3 (0f, 0f, tilt * Time.deltaTime));
       
        //rb.velocity -= Vector2.up * Time.deltaTime;
        
        // // Velocity
        // Vector2 verticalVelocity = rb.velocity - (Vector2)Vector3.ProjectOnPlane (transform.up, rb.velocity);
        // //fall = vertvel.magnitude;
        // rb.velocity -= verticalVelocity * Time.deltaTime;
        // rb.velocity += verticalVelocity.magnitude * (Vector2)transform.right * Time.deltaTime / 10;
        // Debug.Log($"Bird current velocity: {rb.velocity}");

        // // Drag
        // Vector2 drag = rb.velocity - (Vector2)Vector3.ProjectOnPlane ((Vector2)transform.right, rb.velocity);
        // rb.AddForce (-drag * drag.magnitude * Time.deltaTime / 1000);

        //Vector2 moveVector = rb.velocity.normalized;
        float rotation = rb.transform.rotation.eulerAngles.z;
        Debug.Log($"Current Rotation: {rotation}");
        float lift = 0;
        float liftMultiplier = rotation % 180;

        if (rotation <= 30f && rotation >= 330f) {
            Debug.Log("Facing RIGHT");
        }
        if (rotation >= 210f && rotation <= 150f) {
            Debug.Log("Facing RIGHT");
        }

        if ((rotation <= 30f || rotation >= 330f ) 
           || (rotation >= 210f || rotation <= 150f)){
            // applyLift
            lift = (1 * liftMultiplier/10);
            Debug.Log("Lift");
        }
        rb.AddForce(Vector2.up * lift);
    }

    private void FacePlayerToMouse()
    {

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = transform.position.z; // maintain the same Z coordinate as the object

        Vector3 direction = mousePos - transform.position;
        
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        //float currentAngle =
        //    Mathf.MoveTowardsAngle(transform.eulerAngles.z, targetAngle, rotationResponseModifier * Time.deltaTime);

        Quaternion rotation = Quaternion.AngleAxis(targetAngle, Vector3.forward); 

        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationResponseModifier * Time.deltaTime);//Quaternion.Euler(0f, 0f, currentAngle);
        //rb.AddTorque(Vector3.Cross(rotation.eulerAngles, Vector3.up), ForceMode2D.Impulse);
    }

    private IEnumerator GetMoveSpeedRoutine(float prevSpeed)
    {
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
        if (flapCharges.Peek().isUseable)
        {
            var charge = flapCharges.Dequeue();
            charge.UseFlap();
            flapCharges.Enqueue(charge);


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

    private void AdjustPlayerFacingDirection()
    {
        var mouse = Input.mousePosition;
        var player = Camera.main.WorldToScreenPoint(transform.position);
        if (mouse.x < player.x)
        {
            spriteRenderer.flipY = true;
        }
        else
        {
            spriteRenderer.flipY = false;
        }
    }
}