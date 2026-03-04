using Enemy.Controls;
using UnityEngine;

namespace Minions.Command
{
    /// <summary>
    /// Minion'un formasyon içindeki belirlenen statik bir noktaya gitmesini ve orada konumlanmasını sağlar.
    /// </summary>
    public class JoinFormationCommand : BaseCommand
    {
        private readonly Mover _mover;
        private readonly Vector3 _targetPosition;
        private readonly float _stoppingDistance;

        // Formasyonlar hassas konumlanma gerektirir, toleransı düşük tutuyoruz.
        private const float DEFAULT_TOLERANCE = 0.5f;

        public JoinFormationCommand(Mover mover, Vector3 targetPosition, float stoppingDistance = DEFAULT_TOLERANCE)
        {
            _mover = mover;
            _targetPosition = targetPosition;
            _stoppingDistance = stoppingDistance;
        }

        public override void Execute()
        {
            base.Execute();
            _mover.MoveTo(_targetPosition);
        }

        public override void UpdateCommand()
        {
            if (_isFinished) return;

            // Mover üzerinde hedefe varıp varmadığımızı kontrol ediyoruz.
            if (_mover.HasReached(_targetPosition, _stoppingDistance))
            {
                _mover.Stop();
                // İleride buraya "Liderin rotasyonuna dön" mantığı eklenebilir.
                _isFinished = true; 
            }
            else
            {
                // NavMesh bazen yolu yeniden hesaplamaya ihtiyaç duyabilir, hedefi tazeliyoruz.
                // Not: Performans için her frame çağrılmayabilir, Mover içinde optimizasyon var varsayıyoruz.
                _mover.MoveTo(_targetPosition);
            }
        }

        public override void Cancel()
        {
            base.Cancel();
            _mover.Stop();
        }
    }
}