using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Steamworks;

public class PlayerObjectManager : NetworkBehaviour
{
    private CustomNetworkManager networkManager;

    private CustomNetworkManager NetworkManager
    {
        get
        {
            if (networkManager != null) { return networkManager; }

            return networkManager = CustomNetworkManager.singleton as CustomNetworkManager;
        }
    }
}
