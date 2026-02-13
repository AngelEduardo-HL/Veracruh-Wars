using UnityEngine;

public class PlayerAttackRadius : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        EnemyController e = other.GetComponentInParent<EnemyController>();
    }

    private void OnTriggerExit(Collider other)
    {
        EnemyController e = other.GetComponentInParent<EnemyController>();
    }
}
