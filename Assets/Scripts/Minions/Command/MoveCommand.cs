using Enemy.Controls;
using Minions.Command;
using UnityEngine;

namespace Enemy.Command
{
    /// <summary>
    /// Sabit bir Vector3 noktasına gitme işlemini yöneten komut.
    /// </summary>
    public class MoveCommand : ICommand
    {
        private readonly Mover _mover;
        private readonly Vector3 _destination;
        private readonly float _stoppingDistance;
        private bool _isFinished;

        public MoveCommand(Mover mover, Vector3 destination, float stoppingDistance = 0.5f)
        {
            _mover = mover;
            _destination = destination;
            _stoppingDistance = stoppingDistance;
            _isFinished = false;
        }

        public void Execute()
        {
            _mover.MoveTo(_destination);
        }

        public void UpdateCommand()
        {
            if (_isFinished) return;

            if (_mover.HasReached(_destination, _stoppingDistance))
            {
                _mover.Stop();
                _isFinished = true;
            }
        }

        public void Cancel()
        {
            _mover.Stop();
            _isFinished = true;
        }

        public bool IsFinished() => _isFinished;
    }
}