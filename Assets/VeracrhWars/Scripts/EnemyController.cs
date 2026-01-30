using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EnemyController : MonoBehaviour
{
    [Header("Chase")]
    [SerializeField] private float moveSpeed = 3.5f;
    [SerializeField] private float rotateSpeed = 12f;
    [SerializeField] private float stopDistance = 0.9f;

    private Rigidbody _rb;
    private Transform _player;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.useGravity = true;
        _rb.interpolation = RigidbodyInterpolation.Interpolate;
    }

    private void Start()
    {
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) _player = p.transform;
    }

    private void FixedUpdate()
    {
        if (_player == null) return;

        Vector3 toPlayer = _player.position - _rb.position;
        toPlayer.y = 0f;

        float dist = toPlayer.magnitude;
        if (dist <= stopDistance) return;

        Debug.DrawLine(_rb.position, _player.position, Color.red);

        Vector3 dir = toPlayer / dist;

        Vector3 newPos = _rb.position + dir * moveSpeed * Time.fixedDeltaTime;
        _rb.MovePosition(newPos);

        if (dir.sqrMagnitude > 0.0001f)
        {
            Quaternion target = Quaternion.LookRotation(dir, Vector3.up);
            Quaternion newRot = Quaternion.Slerp(_rb.rotation, target, rotateSpeed * Time.fixedDeltaTime);
            _rb.MoveRotation(newRot);
        }
    }

    public void Die()
    {
        if (WaveManager.Instance != null)
            WaveManager.Instance.UnregisterEnemy();

        Destroy(gameObject);
    }
}
