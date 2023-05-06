using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class Baby : MonoBehaviour
{
    private Rigidbody2D rb;
    private CircleCollider2D collider;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<CircleCollider2D>();
        rb.gravityScale = 0f;
        collider.isTrigger = true;
    }
}