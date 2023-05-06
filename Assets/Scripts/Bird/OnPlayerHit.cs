using System;
using System.Collections;
using System.Collections.Generic;
using Bird;
using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnPlayerHit : MonoBehaviour
{
    private BirdController bird;
    private GameObject baby;
    
    private void Start()
    {
        bird = GetComponentInParent<BirdController>();
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag.Equals("Damaging") && bird.isHoldingBebe)
        {
            baby = bird.heldBaby;
            bird.DropBaby();

            var rb = baby.GetComponent<Rigidbody2D>();
            rb.gravityScale = 0.25f;
        }
    }
}
