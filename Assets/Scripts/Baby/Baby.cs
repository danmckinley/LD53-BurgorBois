using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class Baby : MonoBehaviour
{
    public bool isHeld = false;
    private PlayerInput playerInput;
    private GameObject bird;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private CircleCollider2D collider;

    private void Awake()
    {
        playerInput = new PlayerInput();
    }

    void OnEnable()
    {
        playerInput.Enable();
        playerInput.Movement.PickUpDrop.performed += _ => OnClick();

        bird = GameObject.Find("Birb");
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<CircleCollider2D>();
    }


    private void OnClick()
    {
        var birdPosition = bird.transform.position;
        var babyPosition = rb.transform.position;
        float distance = Vector3.Distance(birdPosition, babyPosition);

        if (isHeld == false && distance <= 2f)
        {
            isHeld = true;
            spriteRenderer.enabled = false;
            collider.enabled = false;
            bird.GetComponent<Animator>().SetBool("pickedUpBaby", true);
        }
        else if (isHeld)
        {
            isHeld = false;
            bird.GetComponent<Animator>().SetBool("pickedUpBaby", false);
            gameObject.transform.position = new Vector3(birdPosition.x, birdPosition.y - 1f, birdPosition.z);
            gameObject.transform.rotation = new Quaternion();
            collider.enabled = true;
            spriteRenderer.enabled = true;
        }
    }
}