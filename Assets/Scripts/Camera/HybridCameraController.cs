using EventBus;
using UnityEngine;
using Unity.Cinemachine;
using Input;
using Unity.Cinemachine.TargetTracking;

namespace Camera
{
    /// <summary>
    /// Cinemachine 3.x için Hibrit Kamera Kontrolcüsü.
    /// TPS ve Top-Down açıları arasında geçiş yapar.
    /// </summary>
    [RequireComponent(typeof(CinemachineCamera))] // İsim değişti
    [RequireComponent(typeof(CinemachineOrbitalFollow))]
    public class HybridCameraController : MonoBehaviour
    {
        [System.Serializable]
        public struct ViewSettings
        {
            [Tooltip("Kameranın dikey ofseti (Yükseklik).")]
            public float VerticalOffset;

            [Tooltip("Kameranın hedefe uzaklığı (Radius).")]
            public float Radius;

            [Tooltip("Kameranın aşağı bakış açısı (Derece).")]
            public float TiltAngle;
            
            public static ViewSettings Lerp(ViewSettings a, ViewSettings b, float t)
            {
                return new ViewSettings
                {
                    VerticalOffset = Mathf.Lerp(a.VerticalOffset, b.VerticalOffset, t),
                    Radius = Mathf.Lerp(a.Radius, b.Radius, t),
                    TiltAngle = Mathf.Lerp(a.TiltAngle, b.TiltAngle, t)
                };
            }
        }

        [Header("Dependencies")] [SerializeField]
        private Vector2EventChannelSO _cameraInputChannel;
        [SerializeField] private MinionCameraConfigSO _cameraConfig;

        [Header("Control Settings")] [SerializeField]
        private float _rotationSpeed = 120f;

        [SerializeField] private float _zoomSensitivity = 2f;
        [SerializeField] private float _smoothTime = 0.1f;
        [SerializeField] private float _profileTransitionSpeed = 2f; // Minion sayısı değişince geçiş hızı
        
        private ViewSettings _currentTpsTarget;
        private ViewSettings _currentTopDownTarget;
        
        private ViewSettings _activeTpsSettings;
        private ViewSettings _activeTopDownSettings;

        // CM 3.x Bileşenleri
        private CinemachineCamera _cmCamera;
        private CinemachineOrbitalFollow _orbitalFollow;

        // State Variables
        private Vector2 _currentInput;
        private float _targetZoomFactor = 0f;
        private float _currentZoomFactor = 0f;
        private float _zoomVelocity;

        private void Awake()
        {
            _cmCamera = GetComponent<CinemachineCamera>();
            _orbitalFollow = GetComponent<CinemachineOrbitalFollow>();

            if (_orbitalFollow == null)
            {
                Debug.LogError("CinemachineOrbitalFollow bileşeni eksik! Lütfen ekleyin.");
                enabled = false;
            }

            _targetZoomFactor = 0f;
            
            SetMinionCount(0);
            
            // Başlangıçta yumuşak geçiş olmasın, direkt atayalım
            _activeTpsSettings = _currentTpsTarget;
            _activeTopDownSettings = _currentTopDownTarget;
        }

        private void OnEnable()
        {
            if (_cameraInputChannel != null)
                _cameraInputChannel.OnEventRaised += HandleCameraInput;
            EventBus<MinionAddedEvent>.OnEvent += HandleMinionAdded;
        }

        private void OnDisable()
        {
            if (_cameraInputChannel != null)
                _cameraInputChannel.OnEventRaised -= HandleCameraInput;
            EventBus<MinionAddedEvent>.OnEvent -= HandleMinionAdded;
        }
        
        void HandleMinionAdded(MinionAddedEvent obj) => SetMinionCount(obj.Minion);
        
        /// <summary>
        /// Minion sayısı değiştiğinde bu metodu dışarıdan çağırın.
        /// </summary>
        public void SetMinionCount(int count)
        {
            MinionCameraProfile profile = _cameraConfig.GetProfileForCount(count);
            _currentTpsTarget = profile.TpsSettings;
            _currentTopDownTarget = profile.TopDownSettings;
        }

        private void LateUpdate()
        {

            ProcessProfiles(); // Hedef profile doğru yumuşak geçiş
            ProcessZoom();
            ProcessRotation();
            ApplyTransform();
        }

        private void HandleCameraInput(Vector2 input)
        {
            _currentInput = input;
        }
        
        private void ProcessProfiles()
        {
            // Mevcut ayarlardan hedef minion profili ayarlarına doğru zamanla geçiş yap (Smooth Lerp)
            float dt = Time.deltaTime * _profileTransitionSpeed;
            
            _activeTpsSettings = ViewSettings.Lerp(_activeTpsSettings, _currentTpsTarget, dt);
            _activeTopDownSettings = ViewSettings.Lerp(_activeTopDownSettings, _currentTopDownTarget, dt);
        }

        private void ProcessZoom()
        {
            float zoomDelta = _currentInput.y * _zoomSensitivity * Time.deltaTime;
            _targetZoomFactor = Mathf.Clamp01(_targetZoomFactor + zoomDelta);
            _currentZoomFactor =
                Mathf.SmoothDamp(_currentZoomFactor, _targetZoomFactor, ref _zoomVelocity, _smoothTime);
        }

        private void ProcessRotation()
        {
            // CM 3.x'te eksen kontrolü
            if (Mathf.Abs(_currentInput.x) > 0.01f)
            {
                float rotationDelta = _currentInput.x * _rotationSpeed * Time.deltaTime;

                // Horizontal Axis değerini güncelliyoruz
                _orbitalFollow.HorizontalAxis.Value += rotationDelta;
            }
        }

        private void ApplyTransform()
        {
            if (_cmCamera.Follow == null) return;

            ViewSettings finalSettings = ViewSettings.Lerp(_activeTpsSettings, _activeTopDownSettings, _currentZoomFactor);

            _orbitalFollow.Radius = finalSettings.Radius;
            _orbitalFollow.TargetOffset = new Vector3(0, finalSettings.VerticalOffset, 0);

            Vector3 directionToPlayer = _cmCamera.Follow.position - transform.position;
            directionToPlayer.y = 0;

            if (directionToPlayer.sqrMagnitude > 0.001f)
            {
                Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);
                Quaternion finalRotation = Quaternion.Euler(finalSettings.TiltAngle, lookRotation.eulerAngles.y, 0f);
                transform.rotation = finalRotation;
            }
        }
    }
}