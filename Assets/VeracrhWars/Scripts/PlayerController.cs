using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 6.0f;
    [SerializeField] private bool rotateToMoveDirection = true;
    [SerializeField] private float rotationSpeed = 18f;
    [SerializeField] private float sprintMultiplier = 1.5f;

    [Header("Shooting")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireRate = 8f;

    [Header("Animation")]
    [SerializeField] private Animator animator;
    [SerializeField] private float animDamp = 0.12f;

    private Rigidbody _rb;
    private Vector2 _moveInput;
    private Vector3 _velocity;
    private Camera camera;

    private bool _isFiring, _isAiming;
    private float _fireTimer;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.useGravity = true;
        _rb.interpolation = RigidbodyInterpolation.Interpolate;

        if (firePoint == null) firePoint = transform;
        if (animator == null) animator = GetComponentInChildren<Animator>();

        camera = Camera.main;
    }

    public void OnMove(InputValue value) => _moveInput = value.Get<Vector2>();
    public void OnFire(InputValue value) => _isFiring = value.isPressed;
    public void OnRightClick(InputValue value) => _isAiming = value.isPressed;

    private void Update()
    {
        Vector3 dir = new Vector3(_moveInput.x, 0f, _moveInput.y);
        if (dir.sqrMagnitude > 1f) dir.Normalize();

        float speed = moveSpeed;
        bool isRunning = (Keyboard.current != null && Keyboard.current.leftShiftKey.isPressed && dir.sqrMagnitude > 0.0001f);
        if (isRunning) speed *= sprintMultiplier;

        _velocity = dir * speed;

        // ROTACIÓN
        if (_isAiming) Aim();
        else if (rotateToMoveDirection && dir.sqrMagnitude > 0.0001f) Rotate(dir);

        // ANIMACIÓN
        if (animator != null)
        {
            float max = moveSpeed * sprintMultiplier;
            float normalizedSpeed = (_velocity.magnitude / max);
            animator.SetFloat("Speed", normalizedSpeed, animDamp, Time.deltaTime);
            // animator.SetBool("IsAiming", _isAiming);
        }

        // DISPARO
        _fireTimer -= Time.deltaTime;
        if (_isFiring && _fireTimer <= 0f)
        {
            Shoot();
            _fireTimer = 1f / fireRate;
        }
    }

    private void FixedUpdate()
    {
        Vector3 newPos = _rb.position + _velocity * Time.fixedDeltaTime;
        _rb.MovePosition(newPos);
    }

    private void Shoot()
    {
        if (bulletPrefab == null)
        {
            Debug.LogWarning("No bulletPrefab asignado en PlayerController.");
            return;
        }

        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }

    private void Rotate(Vector3 rotationTarget)
    {
        if (rotationTarget.sqrMagnitude <= 0.0001f) return;
        Quaternion targetRotation = Quaternion.LookRotation(rotationTarget);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void Aim()
    {
        Ray ray = camera.ScreenPointToRay(Mouse.current.position.value);
        RaycastHit hit;
        Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity);

        Vector3 point = hit.collider != null ? hit.point : ray.GetPoint(Mathf.Infinity);
        Vector3 direction = point - transform.position;
        direction.y = 0;
        Rotate(direction);
    }
}
