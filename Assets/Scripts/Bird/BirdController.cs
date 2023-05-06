using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Bird
{
    public class BirdController : MonoBehaviour
    {
        [SerializeField] private float flapPower = 1f;
        [SerializeField] private float flapDirectionPower = 1f;
        [SerializeField] private float moveSpeed = 1f;
        [SerializeField] private float moveSpeedUpdateInterval = 1f;
        [SerializeField] private int flapChargeAmount = 3;
        [SerializeField] private FlapCharge objectToSpawn;
        [SerializeField] private float clickCooldownTime = 1f;
        
        private Rigidbody2D rb;
        private PlayerInput playerInput;
        private Vector2 mousePosition;
        private SpriteRenderer spriteRenderer;
        private Queue<FlapCharge> flapCharges;
        private bool isOnClickCooldown;

        public GameObject heldBaby;
        public bool isHoldingBebe;

        private void Awake()
        {
            playerInput = new PlayerInput();
            rb = GetComponent<Rigidbody2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            flapCharges = new Queue<FlapCharge>();
            isHoldingBebe = false;
            isOnClickCooldown = false;

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
            playerInput.Movement.PickUpDrop.performed += _ => OnClick();
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
            FacePlayerToMouse();

            AdjustPlayerFacingDirection();
        }

        private void FacePlayerToMouse()
        {
            float rotationSpeed = 500f;

            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = transform.position.z; // maintain the same Z coordinate as the object

            Vector3 direction = mousePos - transform.position;
            float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            float currentAngle =
                Mathf.MoveTowardsAngle(transform.eulerAngles.z, targetAngle, rotationSpeed * Time.deltaTime);

            transform.rotation = Quaternion.Euler(0f, 0f, currentAngle);
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

        private void OnClick()
        {
            if (!isOnClickCooldown)
            {
                if (!isHoldingBebe)
                {
                    Collider2D[] hitBabyColliders = Physics2D.OverlapCircleAll(new Vector2(rb.position.x, rb.position.y), 2f);

                    foreach (Collider2D babyCollider in hitBabyColliders)
                    {
                        if (babyCollider.tag.Equals("Baby"))
                        {
                            var babySprite = babyCollider.gameObject.GetComponent<SpriteRenderer>();

                            isHoldingBebe = true;
                            babySprite.enabled = false;
                            babyCollider.enabled = false;
                            gameObject.GetComponent<Animator>().SetBool("pickedUpBaby", true);
                            heldBaby = babyCollider.gameObject;
                            isOnClickCooldown = true;
                            StartCoroutine(OnClickCooldownRoutine());
                            break;
                        }
                    }
                }
                else
                {
                    DropBaby();
                }
            }
            
        }

        private IEnumerator OnClickCooldownRoutine()
        {
            yield return new WaitForSeconds(clickCooldownTime);
            isOnClickCooldown = false;
        }

        public void DropBaby()
        {
            var heldBabySpriteRenderer = heldBaby.GetComponent<SpriteRenderer>();
            var heldBabyCollider = heldBaby.GetComponent<Collider2D>();
            var heldBabyRigidbody = heldBaby.GetComponent<Rigidbody2D>();

            isHoldingBebe = false;
            gameObject.GetComponent<Animator>().SetBool("pickedUpBaby", false);
            heldBaby.transform.position = new Vector3(rb.transform.position.x, rb.transform.position.y - 1f,
                rb.transform.position.z);
            heldBaby.transform.rotation = new Quaternion();

            heldBabySpriteRenderer.enabled = true;
            heldBabyCollider.isTrigger = false;
            heldBabyCollider.enabled = true;
            heldBabyRigidbody.gravityScale = 0.5f;
            isOnClickCooldown = true;
            StartCoroutine(OnClickCooldownRoutine());
        }
    }
}