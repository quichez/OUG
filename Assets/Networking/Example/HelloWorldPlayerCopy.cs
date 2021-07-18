
using MLAPI;
using MLAPI.Messaging;
using MLAPI.NetworkVariable;
using UnityEngine;
using UnityEngine.InputSystem;

namespace HelloWorld
{
    public class HelloWorldPlayerCopy : NetworkBehaviour
    {
        public NetworkVariableVector3 Position = new NetworkVariableVector3(new NetworkVariableSettings
        {
            WritePermission = NetworkVariablePermission.ServerOnly,
            ReadPermission = NetworkVariablePermission.Everyone
        });

        private InputMaster _inputs;
        private Vector3 _mvt;

        // Javier this is the callback stuff for movement don't kill it
        public void Move_performed(InputAction.CallbackContext obj)
        {
            _mvt = new Vector3(obj.ReadValue<Vector2>().x, 0.0f, obj.ReadValue<Vector2>().y).normalized;
        }

        public override void NetworkStart()
        {
            _inputs = new InputMaster();
            _inputs.Movement.Move.performed += Move_performed;
            _inputs.Movement.Enable();
            Move();
        }

        private void OnEnable()
        {
            _inputs.Movement.Enable();
        }

        private void OnDisable()
        {
            _inputs.Movement.Disable();
        }

        // Javier this is the server move thing.
        public void Move()
        {
            // I have code that works with camera 
            if (NetworkManager.Singleton.IsServer)
            {
                var randomPosition = GetRandomPositionOnPlane();
                transform.position = randomPosition;
                Position.Value = randomPosition;
            }
            else
            {
                SubmitPositionRequestServerRpc();
            }
        }

        [ServerRpc]
        void SubmitPositionRequestServerRpc(ServerRpcParams rpcParams = default)
        {
            Position.Value = GetRandomPositionOnPlane();
            Position.Value += _mvt * 0.01f;
        }

        static Vector3 GetRandomPositionOnPlane()
        {
            return new Vector3(Random.Range(-3f, 3f), 1f, Random.Range(-3f, 3f));
        }

        void Update()
        {
            transform.position = Position.Value;
        }
    }
}