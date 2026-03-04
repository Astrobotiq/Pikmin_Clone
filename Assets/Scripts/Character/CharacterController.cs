using Input;
using UnityEngine;

namespace Character
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Input Channels")]
        [SerializeField] private Vector2EventChannelSO _moveEventChannel;

        [Header("Settings")]
        [SerializeField] private float _moveSpeed = 6f;
        [SerializeField] private float _rotationSpeed = 15f;
        [SerializeField] private Transform cameraTransform;
    
        [Header("Gravity & Jump")]
        [SerializeField] private float _gravityValue = -9.81f;
        [SerializeField] private float _fallMultiplier = 2.5f;
        [SerializeField] private float _jumpHeight = 1.0f;

        // State Variables
        private CharacterController _characterController;
    
        private Vector2 _inputVector;
        private Vector3 _playerVelocity; // Dikey hızı (Y) tutar
        private bool _isGrounded;

        private bool _jumpRequested; // Input ile fizik update arasındaki kopukluğu önlemek için

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
        }

        private void OnEnable()
        {
            _moveEventChannel.OnEventRaised += OnMoveInput;
        }

        private void OnDisable()
        {
            _moveEventChannel.OnEventRaised -= OnMoveInput;
        }

        private void Update()
        {
            // 1. Durum Kontrolü
            _isGrounded = _characterController.isGrounded;

            // 2. Yerçekimi ve Dikey Kuvvetleri Hesapla
            CalculateVerticalMovement();

            // 3. Yatay Hareketi Hesapla
            Vector3 movementVector = CalculateHorizontalMovement();

            // 4. Hepsini Birleştir ve TEK SEFERDE Uygula
            // (Yatay Hareket + Dikey Hız) * DeltaTime
            Vector3 finalMove = movementVector * _moveSpeed + _playerVelocity;
        
            _characterController.Move(finalMove * Time.deltaTime);

            // 5. Karakteri Döndür
            if (movementVector.sqrMagnitude > 0)
            {
                RotateCharacter(movementVector);
            }
        }

        // --- Event Listeners ---

        private void OnMoveInput(Vector2 input)
        {
            _inputVector = input;
        }

        private void OnJumpInput()
        {
            // Input geldiğinde direkt zıplatmak yerine talep oluşturuyoruz.
            // Eğer yerdeysek zıplama izni veriyoruz.
            if (_isGrounded)
            {
                _jumpRequested = true;
            }
        }

        // --- Calculation Methods ---

        private void CalculateVerticalMovement()
        {
            if (_isGrounded && _playerVelocity.y < 0)
            {
                _playerVelocity.y = -2f; // Yere yapışık kalması için
            }

            // Zıplama Talebi İşleme
            if (_jumpRequested && _isGrounded)
            {
                _playerVelocity.y = Mathf.Sqrt(_jumpHeight * -2f * _gravityValue);
                _jumpRequested = false; // Talebi tükettik
            }

            // Yerçekimi Uygulama (Fall Multiplier ile)
            if (_playerVelocity.y < 0)
            {
                // Düşerken daha hızlı
                _playerVelocity.y += _gravityValue * _fallMultiplier * Time.deltaTime;
            }
            else
            {
                // Yükselirken normal
                _playerVelocity.y += _gravityValue * Time.deltaTime;
            }
        }

        private Vector3 CalculateHorizontalMovement()
        {
            if (_inputVector.sqrMagnitude == 0) return Vector3.zero;

            Vector3 cameraForward = cameraTransform.forward;
            Vector3 cameraRight = cameraTransform.right;

            cameraForward.y = 0f;
            cameraRight.y = 0f;
            cameraForward.Normalize();
            cameraRight.Normalize();

            return (cameraForward * _inputVector.y + cameraRight * _inputVector.x).normalized;
        }

        private void RotateCharacter(Vector3 direction)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
        }
    }
}