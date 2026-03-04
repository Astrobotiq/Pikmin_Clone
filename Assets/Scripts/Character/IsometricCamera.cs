using UnityEngine;

/// <summary>
/// Karakteri belirli bir ofset ve yumuşatma ile takip eden izometrik kamera kontrolcüsü.
/// Perspektif modunda "Telephoto" etkisi yaratmak için tasarlanmıştır.
/// </summary>
public class IsometricCamera : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform _target;

    [Header("Settings")]
    [Tooltip("Kameranın hedefi ne kadar gecikmeli takip edeceği. 0 = anlık, yüksek değer = daha yumuşak.")]
    [SerializeField] private float _smoothTime = 0.2f;
    
    [Tooltip("Kameranın hedefe olan sabit mesafesi (Offset). Oyun başında otomatik hesaplanır.")]
    [SerializeField] private Vector3 _offset;

    [Header("Auto Configure")]
    [Tooltip("İşaretlenirse, oyun başladığında kameranın o anki konumunu ofset olarak kaydeder.")]
    [SerializeField] private bool _calculateOffsetOnStart = true;

    private Vector3 _currentVelocity;

    private void Start()
    {
        if (_target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null) _target = player.transform;
            else Debug.LogError("IsometricCamera: Takip edilecek bir hedef (Target) atanmadı!");
        }

        if (_calculateOffsetOnStart && _target != null)
        {
            _offset = transform.position - _target.position;
        }
    }

    private void LateUpdate() 
    {
        if (_target == null) return;

        Vector3 targetPosition = _target.position + _offset;

        transform.position = Vector3.SmoothDamp(
            transform.position, 
            targetPosition, 
            ref _currentVelocity, 
            _smoothTime
        );
    }
}