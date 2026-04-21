using Unity.Netcode;
using UnityEngine;

public class Score : NetworkBehaviour
{
    // NetworkVariable to keep track of the player's score (default value is 0)
    public NetworkVariable<int> playerScore = new NetworkVariable<int>(0);

    public override void OnNetworkSpawn()
    {
        // Subscribe to score changes to update UI or log changes (optional)
        playerScore.OnValueChanged += OnScoreChanged;
    }

    public override void OnNetworkDespawn()
    {
        playerScore.OnValueChanged -= OnScoreChanged;
    }

    // This method is called whenever the score changes
    private void OnScoreChanged(int oldScore, int newScore)
    {
        Debug.Log($"{gameObject.name}'s score changed from {oldScore} to {newScore}");
        // Optionally, update your UI here.
    }

    // Method to add points. This should only be called on the server.
    public void AddPoint(int points)
    {
        if (!IsServer)
            return;
        playerScore.Value += points;
    }
}