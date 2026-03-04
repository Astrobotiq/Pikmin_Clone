using UnityEngine;

namespace Camera
{
    [System.Serializable]
    public struct MinionCameraProfile
    {
        [Tooltip("Bu ayarların devreye girmesi için gereken minimum minion sayısı.")]
        public int MinCount;

        [Header("TPS Modu Ayarları")]
        public HybridCameraController.ViewSettings TpsSettings;

        [Header("Top-Down Modu Ayarları")]
        public HybridCameraController.ViewSettings TopDownSettings;
    }
}