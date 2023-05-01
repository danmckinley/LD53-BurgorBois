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
    }

    private void Start()
    {
        isHeld = false;
    }


    public void PickUp()
    {
        if (isHeld == false) // and bird is close
        {
            isHeld = true;
        }
    }

    public void Drop()
    {
        if (isHeld == true)
        {
            isHeld = false;
        }
    }

    private void OnClick()
    {
        Debug.Log("Sprite clicked: " + gameObject.name);

        var birdPosition = bird.transform.position;
        var babyPosition = rb.transform.position;
        float distance = Vector3.Distance(birdPosition, babyPosition);
        Debug.Log("Distance: " + distance);

        if (isHeld == false && distance <= 1.5f)
        {
            isHeld = true;
            this.gameObject.transform.localScale = new Vector3(0, 0, 0);
            bird.GetComponent<Animator>().SetBool("Flapping", true);
        }

    }
}


