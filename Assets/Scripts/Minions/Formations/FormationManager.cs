using System.Collections.Generic;
using Enemy.Controls;
using EventBus;
using UnityEngine;

namespace Minions.Formations
{
    /// <summary>
    /// Minionları yöneten, formasyon noktalarını hesaplayan ve atamaları yapan ana sınıf.
    /// </summary>
    public class FormationManager : SingletonMonoBehaviour<FormationManager>
    {
        [Header("Settings")]
        [SerializeField] private FormationBase _currentFormation;
        [SerializeField] private float _recalculateDistance = 2.0f; // Lazy Follow eşiği
        [SerializeField] private float _updateRate = 0.2f; // Saniyede 5 kere kontrol et (Optimizasyon)

        [Header("Units")]
        [SerializeField] private List<MinionController> _minions = new List<MinionController>();

        private Vector3 _lastLeaderPosition;
        private float _nextCheckTime;

        private void Start()
        {
            _lastLeaderPosition = transform.position;
        }

        private void Update()
        {
            if (Time.time < _nextCheckTime) return;
            _nextCheckTime = Time.time + _updateRate;

            if (Vector3.Distance(transform.position, _lastLeaderPosition) > _recalculateDistance)
            {
                UpdateFormationPositions();
                _lastLeaderPosition = transform.position;
            }
        }

        public void AddMinion(MinionController minion)
        {
            if (!_minions.Contains(minion))
            {
                _minions.Add(minion);
                UpdateFormationPositions(); // Yeni asker gelince düzeni güncelle
                EventBus<MinionAddedEvent>.Raise(new MinionAddedEvent(_minions.Count));
            }
        }

        public void RemoveMinion(MinionController minion)
        {
            if (_minions.Contains(minion))
            {
                _minions.Remove(minion);
                UpdateFormationPositions();
                EventBus<MinionAddedEvent>.Raise(new MinionAddedEvent(_minions.Count));
            }
        }

        public void SetFormation(FormationBase newFormation)
        {
            _currentFormation = newFormation;
            UpdateFormationPositions(); // Formasyon değiştiği an uygula
        }

        /// <summary>
        /// Nearest Neighbor algoritması ile en uygun slotları minionlara dağıtır.
        /// </summary>
        private void UpdateFormationPositions()
        {
            if (_currentFormation == null || _minions.Count == 0) return;

            List<Vector3> points = _currentFormation.CalculatePositions(transform, _minions.Count);
            List<MinionController> availableMinions = new List<MinionController>(_minions);

            foreach (Vector3 point in points)
            {
                MinionController closestMinion = null;
                float minDistance = float.MaxValue;

                foreach (var minion in availableMinions)
                {
                    float dist = Vector3.SqrMagnitude(minion.transform.position - point);
                    if (dist < minDistance)
                    {
                        minDistance = dist;
                        closestMinion = minion;
                    }
                }

                if (closestMinion != null)
                {
                    closestMinion.UpdateFormationMove(point);
            
                    availableMinions.Remove(closestMinion);
                }
            }
        }
    }
}