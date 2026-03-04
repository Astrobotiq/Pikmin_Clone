using System.Collections.Generic;
using UnityEngine;

namespace Minions.Formations
{
    [CreateAssetMenu(menuName = "AI/Formation/Circle")]
    public class CircleFormation : FormationBase
    {
        [Tooltip("Çemberin yarıçapı")]
        public float Radius = 3f;

        public override List<Vector3> CalculatePositions(Transform leader, int minionCount)
        {
            var points = new List<Vector3>();
            float angleStep = 360f / minionCount;

            for (int i = 0; i < minionCount; i++)
            {
                float angle = i * angleStep * Mathf.Deg2Rad;
            
                // Trigonometri ile çember üzerindeki noktayı bul
                float x = Mathf.Cos(angle) * Radius;
                float z = Mathf.Sin(angle) * Radius;

                Vector3 offset = (leader.right * x) + (leader.forward * z);
                points.Add(leader.position + offset);
            }

            return points;
        }
    }
}