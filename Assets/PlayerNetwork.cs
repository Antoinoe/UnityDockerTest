using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerNetwork : NetworkBehaviour
{
    private NetworkVariable<int> _randomNumber = new NetworkVariable<int>(1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public override void OnNetworkSpawn()
    {
        _randomNumber.OnValueChanged += (int previousValue, int newValue) =>
        {
            Debug.Log($"{OwnerClientId} ; RandomNumber : {_randomNumber.Value}");
        };
        
    }
    
    private void Update()
    {
        
        if (!IsOwner) return;
        if (Input.GetKeyDown(KeyCode.T))
        {
            _randomNumber.Value = Random.Range(1, 100);
        }
        var moveDir = new Vector3(0,0,0);
        if (Input.GetKey(KeyCode.Z)) moveDir.z = 1.0f;
        if (Input.GetKey(KeyCode.S)) moveDir.z = -1.0f;
        if (Input.GetKey(KeyCode.D)) moveDir.x = 1.0f;
        if (Input.GetKey(KeyCode.Q)) moveDir.x = -1.0f;

        const float moveSpeed =5.0f;
        transform.position += moveDir * (moveSpeed * Time.deltaTime);
    }
}
