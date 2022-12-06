using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerNetwok : NetworkBehaviour
{
    private void Update()
    {
        if (!IsOwner) return;
        var moveDir = new Vector3(0,0,0);
        if (Input.GetKey(KeyCode.Z)) moveDir.z = 1.0f;
        if (Input.GetKey(KeyCode.S)) moveDir.z = -1.0f;
        if (Input.GetKey(KeyCode.D)) moveDir.x = 1.0f;
        if (Input.GetKey(KeyCode.Q)) moveDir.x = -1.0f;

        const float moveSpeed =5.0f;
        transform.position += moveDir * (moveSpeed * Time.deltaTime);
    }
}
