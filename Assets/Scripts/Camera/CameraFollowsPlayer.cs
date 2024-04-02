using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class CameraFollowsPlayer : MonoBehaviour
{
    private Transform player;

    internal void Init()
    {
        if (GameObject.FindGameObjectWithTag("Player") == null)
        {
            Debug.LogError("Camera cannot get Player in Init()");
            return;
        }
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Awake()
    {
        if (GameObject.FindGameObjectWithTag("Player") == null) return;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null) return;
        transform.position = player.transform.position + new Vector3(0, 1, -5);
    }
}
