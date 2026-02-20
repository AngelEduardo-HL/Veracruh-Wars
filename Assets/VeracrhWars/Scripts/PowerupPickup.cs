using UnityEngine;

public class PowerupPickup : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        PlayerShield shield = other.GetComponent<PlayerShield>();

        if(shield == null)
        {
            return;
        }

        shield.ActivateShield();
        gameObject.SetActive(false);
    }
}
