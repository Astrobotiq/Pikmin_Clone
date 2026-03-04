using Enemy.Command;
using Enemy.Controls;
using UnityEngine;

namespace Minions.Command
{
    /// <summary>
    /// Verilen bir Transform hedefini sürekli takip eden komut.
    /// </summary>
    public class FollowTransformCommand : BaseCommand
    {
        private readonly Mover _mover;
        private readonly Transform _targetToFollow;
        private readonly float _stopDistance;
        private readonly float _repathInterval = 0.5f; // Her frame path hesaplama, performans koru.
        private float _nextRepathTime;

        public FollowTransformCommand(Mover mover, Transform target, float stopDistance = 2.0f)
        {
            _mover = mover;
            _targetToFollow = target;
            _stopDistance = stopDistance;
        }

        public override void Execute()
        {
            base.Execute();
            if (_targetToFollow == null)
            {
                _isFinished = true;
                return;
            }
            _mover.MoveTo(_targetToFollow.position);
        }

        public override void UpdateCommand()
        {
            if (_isFinished || _targetToFollow == null) return;

            // Performans Optimizasyonu: Path'i her frame değil, belirli aralıklarla güncelle.
            if (Time.time >= _nextRepathTime)
            {
                float distance = Vector3.Distance(_mover.transform.position, _targetToFollow.position);

                if (distance > _stopDistance)
                {
                    _mover.MoveTo(_targetToFollow.position);
                }
                else
                {
                    _mover.Stop();
                }
                
                _nextRepathTime = Time.time + _repathInterval;
            }
        }

        public override void Cancel()
        {
            base.Cancel();
            _mover.Stop();
        }
    }
}