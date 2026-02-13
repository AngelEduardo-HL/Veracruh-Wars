using UnityEngine;

public class ShieldPickup : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerShield shield = other.GetComponent<PlayerShield>();
        if (shield != null)
        {
            shield.ActivateShield();
        }

        Destroy(gameObject);
    }
}
