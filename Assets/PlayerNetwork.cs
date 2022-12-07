using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerNetwork : NetworkBehaviour
{
    public struct CustomData : INetworkSerializable
    {
        public int MyInt;
        public bool MyBool;
        public FixedString128Bytes MyString;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref MyInt);
            serializer.SerializeValue(ref MyBool);
            serializer.SerializeValue(ref MyString);
        }
    }
    private readonly NetworkVariable<CustomData> _randomNumber = new NetworkVariable<CustomData>(new CustomData
    {
        MyInt = 0,
        MyBool= false,
        MyString = ""
    }, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);


    public override void OnNetworkSpawn()
    {
        _randomNumber.OnValueChanged += (CustomData previousValue, CustomData newValue) =>
        {
            Debug.Log($"ID : {OwnerClientId.ToString()} - Message: {_randomNumber.Value.MyString.ToString()}" );
        };
        
    }
    
    private void Update()
    {
        if (!IsOwner) return;

        #region HandleCustomData

        if (Input.GetKeyDown(KeyCode.T))
        {
            _randomNumber.Value = new CustomData
            {
                MyInt = Random.Range(1, 100),
                MyBool = Random.Range(0, 2) == 1,
                MyString = $"Random Int : {Random.Range(1, 100).ToString()}, Random Bool : {(Random.Range(0, 2) == 1).ToString()}"
            };
        }

        #endregion

        
        #region Character Movement

        var moveDir = new Vector3(0,0,0);
        if (Input.GetKey(KeyCode.Z)) moveDir.z = 1.0f;
        if (Input.GetKey(KeyCode.S)) moveDir.z = -1.0f;
        if (Input.GetKey(KeyCode.D)) moveDir.x = 1.0f;
        if (Input.GetKey(KeyCode.Q)) moveDir.x = -1.0f;

        const float moveSpeed =5.0f;
        transform.position += moveDir * (moveSpeed * Time.deltaTime);

        #endregion
        
    }
}
