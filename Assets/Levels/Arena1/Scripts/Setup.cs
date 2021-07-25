using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;

public class Setup : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
#if UNITY_EDITOR
        NetworkManager.StartHost();
#else
        NetworkManager.StartClient();
#endif
    }
}
