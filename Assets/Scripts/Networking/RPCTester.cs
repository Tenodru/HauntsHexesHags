using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPCTester : NetworkBehaviour
{
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backslash))
        {
            Debug.Log("<color=orange>RPCTester - TEST BUTTON PRESSED.</color>");
            RPCTEST();
            TEST_GrabPlayerList();
        }
    }

    [ClientRpc]
    public void RPCTEST()
    {
        Debug.Log("<color=green>RPCTester - REEEEEEEE.</color>");

    }

    public void TEST_GrabPlayerList()
    {
        Debug.Log("<color= #ffb04c>RPCTester - PLAYERLIST</color>");
        foreach (var kvp in NetworkServer.connections)
        {
            Debug.Log($"Key: {kvp.Key}, Value: {kvp.Value}");
        }
    }
}
