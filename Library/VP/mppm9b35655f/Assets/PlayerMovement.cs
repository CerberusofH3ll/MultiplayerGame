using Unity.Netcode;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    public float speed = 5f;
    private GameController gameController;
    private Rigidbody rb;

    private void Start()
    {
        Debug.Log($"{gameObject.name} PlayerMovement Start() called.");
        gameController = FindObjectOfType<GameController>();
        if (gameController == null)
        {
            Debug.LogError("GameController not found in the scene.");
        }

        // Get the Rigidbody component
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody not found on the player prefab. Please add one.");
        }

        Debug.Log($"{gameObject.name} OwnerClientId: {OwnerClientId}, LocalClientId: {NetworkManager.Singleton.LocalClientId}");
    }

    private void Update()
    {
        Debug.Log($"{gameObject.name} Update() called.");

        if (!IsOwner)
        {
            Debug.Log($"{gameObject.name} is not the owner. Skipping input.");
            return;
        }
        Debug.Log($"{gameObject.name} is the owner.");

        if (gameController != null)
        {
            Debug.Log($"{gameObject.name} gameActive value: {gameController.gameActive.Value}");
            if (!gameController.gameActive.Value)
            {
                Debug.Log("Game is not active. No input processing.");
                return;
            }
        }

        float moveX = 0f;
        float moveZ = 0f;

        if (IsHost)
        {
            moveX = (Input.GetKey(KeyCode.A) ? -1 : 0) + (Input.GetKey(KeyCode.D) ? 1 : 0);
            moveZ = (Input.GetKey(KeyCode.S) ? -1 : 0) + (Input.GetKey(KeyCode.W) ? 1 : 0);
        }
        else
        {
            moveX = (Input.GetKey(KeyCode.LeftArrow) ? -1 : 0) + (Input.GetKey(KeyCode.RightArrow) ? 1 : 0);
            moveZ = (Input.GetKey(KeyCode.DownArrow) ? -1 : 0) + (Input.GetKey(KeyCode.UpArrow) ? 1 : 0);
        }

        Vector3 movement = new Vector3(moveX, 0, moveZ).normalized * speed * Time.deltaTime;
        if (movement != Vector3.zero)
        {
            Debug.Log($"{gameObject.name} input movement: {movement}");
            MoveOnServerRpc(movement);
        }
    }

    [ServerRpc]
    private void MoveOnServerRpc(Vector3 movement, ServerRpcParams rpcParams = default)
    {
        // Use Rigidbody.MovePosition to let physics handle collisions.
        rb.MovePosition(rb.position + movement);
        Debug.Log($"{gameObject.name} position updated on server: {rb.position}");
    }

    // This method is called when a collision happens.
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            Debug.Log($"{gameObject.name} bumped into a wall!");
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlayPlayerWallImpact();
            }
        }
    }
}