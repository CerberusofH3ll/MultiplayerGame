using Unity.Netcode;
using UnityEngine;

public class CoinSpawner : NetworkBehaviour
{
    public GameObject coinPrefab;
    public float spawnInterval = 5f;
    public Vector2 spawnAreaMin;
    public Vector2 spawnAreaMax;

    private GameController gameController;

    private void Start()
    {
        gameController = FindObjectOfType<GameController>();

        if (IsServer && gameController != null)
        {
            Debug.Log("CoinSpawner: Spawner started on server.");
            InvokeRepeating(nameof(SpawnCoin), 2f, spawnInterval);
        }
        else
        {
            Debug.Log("CoinSpawner: Not running on the server. Spawner will not work here.");
        }
    }

    void SpawnCoin()
    {
        if (gameController != null && !gameController.gameActive.Value)
        {
            Debug.Log("CoinSpawner: Game is not active, skipping coin spawn.");
            return;
        }

        Vector3 spawnPos = new Vector3(
            Random.Range(spawnAreaMin.x, spawnAreaMax.x),
            0.5f,  // Adjust height
            Random.Range(spawnAreaMin.y, spawnAreaMax.y)
        );

        GameObject coinInstance = Instantiate(coinPrefab, spawnPos, Quaternion.identity);
        NetworkObject coinNetworkObject = coinInstance.GetComponent<NetworkObject>();

        if (coinNetworkObject != null)
        {
            coinNetworkObject.Spawn();
            Debug.Log("CoinSpawner: Coin spawned at " + spawnPos);
        }
        else
        {
            Debug.LogError("CoinSpawner: No NetworkObject found on the Coin prefab!");
        }
    }
}