using System;
using Enemy.Command;
using Minions.Command;
using Minions.Formations;
using UnityEngine;

namespace Enemy.Controls
{
    /// <summary>
    /// Minion'un ana beyni. Komutları alır ve işler.
    /// </summary>
    public class MinionController : MonoBehaviour
    {
        [SerializeField] private Mover _mover;
        private ICommand _currentCommand;
        
        private bool _isInFormation;

        void Start()
        {
            EnterIdleState();
        }

        private void Update()
        {
            if (_currentCommand != null)
            {
                _currentCommand.UpdateCommand();

                if (_currentCommand.IsFinished())
                {
                    if (!_isInFormation)
                    {
                        EnterIdleState();
                    }
                }
            }
        }

        public void SetCommand(ICommand newCommand)
        {
            _currentCommand?.Cancel();
            _currentCommand = newCommand;
            _currentCommand.Execute();
        }
        
        /// <summary>
        /// LeadershipAura tarafından tetiklenir.
        /// Eğer minion zaten bir görevde değilse (Idle ise) ekibe katılır.
        /// </summary>
        public void JoinSquad()
        {
            if (_isInFormation) return; 
            
            if (!(_currentCommand is IdleCommand || _currentCommand == null)) return;

            _isInFormation = true;
            FormationManager.Instance.AddMinion(this);
            
            // İlk katılımda hemen bir reaksiyon vermesi için boş bir komut atanabilir
            // Ama asıl hareketi FormationManager Update'inde alacak.
        }
        
        /// <summary>
        /// FormationManager tarafından her karede veya belli aralıklarla çağrılır.
        /// Minion'u formasyondaki yerine gönderir.
        /// </summary>
        public void UpdateFormationMove(Vector3 targetPosition)
        {
            var cmd = new JoinFormationCommand(_mover, targetPosition);
            SetCommand(cmd);
        }
        
        public void OrderMove(Vector3 position)
        {
            LeaveSquad(); // Ekipten çık
            var cmd = new MoveCommand(_mover, position, stoppingDistance: 0.5f);
            SetCommand(cmd);
        }
        
        public void OrderAttack(IInteractable target)
        {
            LeaveSquad(); // Ekipten çık
            var cmd = new AttackCommand(_mover, target, range: 2.0f, cooldown: 1.5f);
            SetCommand(cmd);
        }
        
        /// <summary>
        /// Formasyondan çıkış işlemleri.
        /// </summary>
        private void LeaveSquad()
        {
            if (_isInFormation)
            {
                _isInFormation = false;
                FormationManager.Instance.RemoveMinion(this);
            }
        }
        
        private void EnterIdleState()
        {
            _currentCommand = null;
            var cmd = new IdleCommand(_mover);
            SetCommand(cmd);
        }
    }
}