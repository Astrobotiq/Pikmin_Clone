using System.Collections.Generic;
using UnityEngine;

namespace Minions.Formations
{
    [CreateAssetMenu(menuName = "AI/Formation/Wedge")]
    public class WedgeFormation : FormationBase
    {
        [Tooltip("V şeklinin açılma genişliği açısı")]
        public float SpreadAngle = 45f;

        public override List<Vector3> CalculatePositions(Transform leader, int minionCount)
        {
            var points = new List<Vector3>();
        
            // Lider merkezde, 0. indeks en yakın sol, 1. indeks en yakın sağ vb.
            for (int i = 0; i < minionCount; i++)
            {
                // Sol mu sağ mı? (-1 sol, 1 sağ)
                float sideMultiplier = (i % 2 == 0) ? -1f : 1f;
            
                // Kaçıncı sıra? (0,0 -> 1. sıra, 1,2 -> 2. sıra...)
                int row = Mathf.FloorToInt(i / 2f) + 1;

                // Arkaya doğru mesafe
                float backDist = row * Spacing;
                // Yana doğru mesafe (V açılımı)
                float sideDist = row * Spacing * Mathf.Tan(SpreadAngle * Mathf.Deg2Rad) * sideMultiplier;

                // Liderin rotasına göre yerel noktayı dünya koordinatına çevir
                Vector3 offset = (leader.forward * -backDist) + (leader.right * sideDist);
                points.Add(leader.position + offset);
            }

            return points;
        }
    }
}