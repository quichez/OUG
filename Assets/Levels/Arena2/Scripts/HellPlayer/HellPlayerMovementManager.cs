using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.NetworkVariable;
using UnityEngine.InputSystem;

public class HellPlayerMovementManager : NetworkBehaviour
{
    public NetworkVariableVector3 Position = new NetworkVariableVector3(new NetworkVariableSettings
    {
        WritePermission = NetworkVariablePermission.ServerOnly,
        ReadPermission = NetworkVariablePermission.Everyone
    });
    [SerializeField] Camera _cam;

    CharacterController _charController => GetComponent<CharacterController>();
    PlayerInputControls _inputs;
    Vector2 _moveReq;

    string id => GetComponent<NetworkObject>().GetInstanceID().ToString();

    private void MoveRequest(InputAction.CallbackContext ctx) => _moveReq = ctx.ReadValue<Vector2>();    

    private void Move()
    {
        if (NetworkManager.Singleton.IsServer)
        {            
            Vector3 move = new Vector3(_moveReq.x, 0.0f, _moveReq.y);
            move = _cam.transform.right * move.x + _cam.transform.forward * move.z;
            move.y = 0.0f;
            transform.position += move * Time.deltaTime * 5;
            Position.Value = transform.position;
        }
        else
        {           
            // Essentially the same as above.
            MovePlayerServerRpc();
        }
    }

    [ServerRpc]
    private void MovePlayerServerRpc(ServerRpcParams rpcParams = default)
    {
        Debug.Log(id);
        Vector3 move = new Vector3(_moveReq.x, 0.0f, _moveReq.y);
        move = _cam.transform.right * move.x + _cam.transform.forward * move.z;
        move.y = 0.0f;
        Position.Value  += move * Time.deltaTime * 5;
        transform.position = Position.Value;
    }






    private void Awake()
    {
        _inputs = new PlayerInputControls();
    }

    private void OnEnable()
    {        
        _inputs.Player.Move.performed += MoveRequest;
        _inputs.Player.Move.Enable();
    }

    private void OnDisable()
    {
        _inputs.Player.Move.performed -= MoveRequest;
        _inputs.Player.Move.Disable();
    }

    public override void NetworkStart()
    {
        Debug.Log(NetworkManager.Singleton.IsServer);
        Position.Value = transform.position;
    }

    private void Update()
    {
        Move();
    }
}
