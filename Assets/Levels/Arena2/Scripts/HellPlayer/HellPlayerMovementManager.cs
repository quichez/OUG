using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.NetworkVariable;
using UnityEngine.InputSystem;

public class HellPlayerMovementManager : NetworkBehaviour
{   
    [SerializeField] Transform _cam;

    CharacterController _charController => GetComponent<CharacterController>();
    PlayerInputControls _inputs;
    Vector2 _moveReq;

    string id => GetComponent<NetworkObject>().GetInstanceID().ToString();

    private void MoveRequest(InputAction.CallbackContext ctx) => _moveReq = ctx.ReadValue<Vector2>();    

    private void Move()
    {
        Vector3 move = new Vector3(_moveReq.x, 0.0f, _moveReq.y);
        move = Vector3.ClampMagnitude(move, 1.0f);
        _charController.Move(move * 5.0f * Time.deltaTime);
    }

    [ServerRpc]
    private void MovePlayerServerRpc(ServerRpcParams rpcParams = default)
    {
        Vector3 move = new Vector3(_moveReq.x, 0.0f, _moveReq.y);
        move = Vector3.ClampMagnitude(move, 1.0f);
        _charController.Move(move * 5.0f * Time.deltaTime);
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
        if (!IsLocalPlayer)
        {
            _cam.GetComponent<AudioListener>().enabled = false;
            _cam.GetComponent<Camera>().enabled = false;
        }
    }

    private void Update()
    {
        if(IsLocalPlayer)
            Move();
    }
}
