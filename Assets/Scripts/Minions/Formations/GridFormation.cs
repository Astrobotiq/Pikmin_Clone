using System.Collections.Generic;
using UnityEngine;

namespace Minions.Formations
{
    [CreateAssetMenu(menuName = "AI/Formation/Grid")]
    public class GridFormation : FormationBase
    {
        [Tooltip("Yan yana kaç kişi dizilsin?")]
        public int Columns = 3;

        public override List<Vector3> CalculatePositions(Transform leader, int minionCount)
        {
            var points = new List<Vector3>();

            for (int i = 0; i < minionCount; i++)
            {
                int row = i / Columns;
                int col = i % Columns;

                // Grid'i liderin arkasında ortalamak için ofset hesabı
                float xOffset = (col - (Columns - 1) * 0.5f) * Spacing;
                float zOffset = (row + 1) * Spacing; // +1 diyerek liderin hemen arkasından başlatıyoruz

                Vector3 offset = (leader.right * xOffset) - (leader.forward * zOffset);
                points.Add(leader.position + offset);
            }

            return points;
        }
    }
}