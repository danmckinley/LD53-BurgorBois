using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeField] private Transform player;
    // Start is called before the first frame update

    public Vector3 offset;
    
    private void FixedUpdate()
    {
        transform.position = new Vector3(player.position.x + offset.x, player.position.y + offset.y, offset.z);
    }
}
