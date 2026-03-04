using System.Collections.Generic;
using UnityEngine;

namespace Minions.Formations
{
    /// <summary>
    /// Tüm formasyon şekillerinin türeyeceği temel soyut sınıf (Strategy Pattern).
    /// </summary>
    public abstract class FormationBase : ScriptableObject
    {
        [Tooltip("Minionlar arası boşluk")]
        public float Spacing = 2f;

        /// <summary>
        /// Verilen lider pozisyonuna ve yönüne göre hedef noktaları hesaplar.
        /// </summary>
        public abstract List<Vector3> CalculatePositions(Transform leader, int minionCount);
    }
}