using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Camera
{
    [CreateAssetMenu(fileName = "MinionCameraConfig", menuName = "Camera/Minion Camera Config")]
    public class MinionCameraConfigSO : ScriptableObject
    {
        [Tooltip("Minion sayısına göre sıralı kamera profilleri.")]
        public List<MinionCameraProfile> Profiles = new List<MinionCameraProfile>();

        /// <summary>
        /// Verilen minion sayısına en uygun profili döner.
        /// </summary>
        public MinionCameraProfile GetProfileForCount(int count)
        {
            var profile = Profiles
                .OrderByDescending(p => p.MinCount)
                .FirstOrDefault(p => count >= p.MinCount);
            
            return profile;
        }
    }
}