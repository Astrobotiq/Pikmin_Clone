using Enemy.Controls;
using Minions.Command;
using UnityEngine;

namespace Enemy.Command
{
    /// <summary>
    /// Hedef odaklı komutlar için temel sınıf. NavMesh üzerinde hedefe gitme ve etkileşim mantığını içerir.
    /// </summary>
    public abstract class BaseSmartCommand : ICommand
    {
        protected readonly Mover _mover;
        protected readonly IInteractable _target;
        protected bool _isFinished;
    
        protected float _interactionRange; 

        protected BaseSmartCommand(Mover mover, IInteractable target, float range)
        {
            _mover = mover;
            _target = target;
            _interactionRange = range;
            _isFinished = false;
        }

        public virtual void Execute()
        {
            // İlk çalışmada yapılacaklar
            if (_target == null || !_target.IsAvailable())
            {
                _isFinished = true;
                return;
            }
        }

        public virtual void UpdateCommand()
        {
            if (_isFinished) return;

            if (_target == null || !_target.IsAvailable())
            {
                _isFinished = true;
                _mover.Stop();
                return;
            }

            float distance = Vector3.Distance(_mover.transform.position, _target.GetPosition());

            if (distance <= _interactionRange)
            {
                _mover.Stop();
                PerformInteraction();
            }
            else
            {
                _mover.MoveTo(_target.GetPosition());
            }
        }

        public virtual void Cancel()
        {
            _mover.Stop();
            _isFinished = true;
        }

        public bool IsFinished() => _isFinished;

        protected abstract void PerformInteraction();
    }
}