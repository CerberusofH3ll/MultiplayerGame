using Unity.Netcode;
using UnityEngine;

public class Coin : NetworkBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!IsServer) return;

        if (other.CompareTag("Player"))
        {
            Score score = other.GetComponent<Score>();
            if (score != null)
            {
                score.AddPoint(1);
            }

            // Play the coin collection sound
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlayCoinCollection();
            }

            GetComponent<NetworkObject>().Despawn();
        }
    }
}