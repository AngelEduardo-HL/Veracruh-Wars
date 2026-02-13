using UnityEngine;

public class PlayerShield : MonoBehaviour
{
    [Header("Shield Visual")]
    [SerializeField] private GameObject shieldVisualPrefab; // esfera visual
    [SerializeField] private Vector3 shieldOffset = Vector3.up * 0.5f;

    private GameObject _shieldVisualInstance;
    public bool HasShield { get; private set; }

    private void Start()
    {
        if (shieldVisualPrefab != null)
        {
            _shieldVisualInstance = Instantiate(shieldVisualPrefab, transform);
            _shieldVisualInstance.transform.localPosition = shieldOffset;
            _shieldVisualInstance.SetActive(false);
        }
    }

    public void ActivateShield()
    {
        HasShield = true;
        if (_shieldVisualInstance != null) _shieldVisualInstance.SetActive(true);
    }

    public bool ConsumeShieldIfAny()
    {
        if (!HasShield) return false;

        HasShield = false;
        if (_shieldVisualInstance != null) _shieldVisualInstance.SetActive(false);
        return true;
    }
}