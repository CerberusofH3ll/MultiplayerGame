using UnityEngine;
using TMPro;
using Unity.Netcode;
using System.Collections;

public class GameController : NetworkBehaviour
{
    public float gameDuration = 300f; // 5 minutes in seconds
    public TMP_Text statusText;

    // NetworkVariable so the gameActive state is replicated to all clients.
    public NetworkVariable<bool> gameActive = new NetworkVariable<bool>(false);

    public override void OnNetworkSpawn()
    {
        // This method is called once the object is spawned on all clients.
        Debug.Log("GameController OnNetworkSpawn() called. IsServer: " + IsServer);
        if (IsServer)
        {
            Debug.Log("Server starting game immediately in OnNetworkSpawn.");
            UpdateStatusTextClientRpc("Game Start!");
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlayGameStart();
            }
            StartCoroutine(ClearStatusAfterDelay(2f));
            StartGame();
        }
    }

    IEnumerator ClearStatusAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        UpdateStatusTextClientRpc("");
    }

    public void StartGame()
    {
        gameActive.Value = true;  // Set the network variable to true.
        Debug.Log("Game Started!");
        StartCoroutine(EndGameAfterTime(gameDuration));
    }

    IEnumerator EndGameAfterTime(float duration)
    {
        yield return new WaitForSeconds(duration);
        EndGame();
    }

    public void EndGame()
    {
        gameActive.Value = false; // Set the network variable to false.
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayGameEnd();
        }
        Debug.Log("Game Ended!");
        UpdateStatusTextClientRpc("Game Over!");
    }

    [ClientRpc]
    private void UpdateStatusTextClientRpc(string text)
    {
        if (statusText != null)
        {
            statusText.text = text;
        }
        else
        {
            Debug.LogWarning("StatusText is not assigned in GameController.");
        }
    }
}