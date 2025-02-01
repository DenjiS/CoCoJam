using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    private const string XAxisName = "Horizontal";
    private const string ZAxisName = "Vertical";
    private const string MouseXAxisName = "Mouse X";
    private const string MouseYAxisName = "Mouse Y";

    [SerializeField, Min(.1f)] private float _moveSpeed = 2f;
    [SerializeField, Min(.1f)] private float _jumpForce = 3f;

    [Header("Aim")]
    [SerializeField] private Transform _cameraTransform;
    [SerializeField, Min(.1f)] private float _aimSpeed = 1f;
    [SerializeField] private float _xMinAngle = -50f;
    [SerializeField] private float _xMaxAngle = 50f;

    private Transform _transform;
    private CharacterController _characterController;

    private void Awake()
    {
        _transform = transform;
        _characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (ReferenceEquals(_characterController, null) == false)
        {
            HandleMovement();
            HandleAim();
            HandleJump();

            if (_characterController.isGrounded == false)
                _characterController.SimpleMove(Physics.gravity);
        }
    }

    private void HandleMovement()
    {
        _characterController.SimpleMove(
            new Vector3(
                Input.GetAxis(XAxisName),
                0,
                Input.GetAxis(ZAxisName)
                ).normalized * _moveSpeed
            );
    }

    private void HandleAim()
    {
        _transform.Rotate(Vector3.up, Input.GetAxis(MouseXAxisName) * _aimSpeed);
        _cameraTransform.localEulerAngles = Vector3.right *
            Mathf.Clamp(
                _cameraTransform.localEulerAngles.x - Input.GetAxis(MouseYAxisName) * _aimSpeed,
                _xMinAngle,
                _xMaxAngle
                );
    }

    private void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            _characterController.SimpleMove(Vector3.up * _jumpForce);
    }
}
