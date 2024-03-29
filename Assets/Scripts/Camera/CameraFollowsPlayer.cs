using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class CameraFollowsPlayer : MonoBehaviour
{
    private Transform player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position + new Vector3(0, 1, -5);
    }
}
