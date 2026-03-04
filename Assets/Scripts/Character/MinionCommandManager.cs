using System.Collections.Generic;
using Enemy.Command;
using Enemy.Controls;
using Input;
using UnityEngine;
using UnityEngine.Serialization;

namespace Character
{
    public class MinionCommandManager : MonoBehaviour
    {
        [Header("Input Channels")]
        [SerializeField] private Vector2EventChannelSO targetDirectionChannel;
        [SerializeField] private VoidEventChannelSO selectRedChannel; // RB
        [SerializeField] private VoidEventChannelSO selectBlueChannel; // LB
        [SerializeField] private VoidEventChannelSO selectGreenChannel; // RT
        [SerializeField] private VoidEventChannelSO selectYellowChannel; // LT
    
        [Header("Action Channels")]
        [SerializeField] private VoidEventChannelSO actionAChannel;
        [SerializeField] private VoidEventChannelSO actionBChannel;
        [SerializeField] private VoidEventChannelSO actionXChannel;
        [SerializeField] private VoidEventChannelSO actionYChannel;
        
        [Header("UI Output Events (Broadcasters)")]
        [SerializeField] private MinionTypeEventChannelSO onMinionSelectionChanged;
        [SerializeField] private CommandTypeEventChannelSO onCommandChanged;
        [SerializeField] private TargetTypeEventChannelSO onTargetChanged;

        private MinionType _selectedType = MinionType.All;
        private CommandType _currentCommandType = CommandType.None;
        private TargetType _currentTargetType = TargetType.None;
        private Dictionary<MinionType, List<IMinion>> _minionGroups;

        private void OnEnable()
        {
            // Yön Inputu
            targetDirectionChannel.OnEventRaised += HandleDirectionInput;

            // Minion Seçimleri (RB, LB, RT, LT)
            selectRedChannel.OnEventRaised += () => SelectMinionGroup(MinionType.Red);
            selectBlueChannel.OnEventRaised += () => SelectMinionGroup(MinionType.Blue);
            selectGreenChannel.OnEventRaised += () => SelectMinionGroup(MinionType.Green);
            selectYellowChannel.OnEventRaised += () => SelectMinionGroup(MinionType.Yellow);

            // Aksiyon Komutları (A, B, X, Y)
            actionAChannel.OnEventRaised += () => SetActiveCommand(CommandType.A);
            actionBChannel.OnEventRaised += () => SetActiveCommand(CommandType.B);
            actionXChannel.OnEventRaised += () => SetActiveCommand(CommandType.X);
            actionYChannel.OnEventRaised += () => SetActiveCommand(CommandType.Y);
        }

        private void HandleDirectionInput(Vector2 direction)
        {
            TargetType newTarget = CalculateTargetFromDirection(direction);
            Debug.Log("Calculated Target: " + newTarget);
            _currentTargetType = (_currentTargetType == newTarget) ? TargetType.None : newTarget;
            Debug.Log("Updated Current Target: " + _currentTargetType);
            onTargetChanged.RaiseEvent(_currentTargetType);
        }

        private TargetType CalculateTargetFromDirection(Vector2 direction)
        {
            Debug.Log("Direction Input Received: " + direction);
            if (direction.x > 0) return TargetType.Right;
            if (direction.x < 0) return TargetType.Left;
            if (direction.y > 0) return TargetType.Up;
            if (direction.y < 0) return TargetType.Down;
    
            return TargetType.None;
        }

        private void SelectMinionGroup(MinionType type)
        {
            ResetSelectionContext();

            if (_selectedType == type)
                _selectedType = MinionType.All;
            else
                _selectedType = type;
            
            onMinionSelectionChanged.RaiseEvent(_selectedType);

        }

        private void SetActiveCommand(CommandType type)
        {
            _currentTargetType = TargetType.None;
            _currentCommandType = type;
            
            onCommandChanged.RaiseEvent(_currentCommandType);
            onTargetChanged.RaiseEvent(_currentTargetType);
        }

        private void ResetSelectionContext()
        {
            _currentCommandType = CommandType.None;
            _currentTargetType = TargetType.None;
            
            onCommandChanged.RaiseEvent(CommandType.None);
            onTargetChanged.RaiseEvent(TargetType.None);
        }

        private void SendCommandToGroup(int actionIndex)
        {
            if (!_minionGroups.TryGetValue(_selectedType, out var group)) return;

            foreach (var minion in group)
            {
                //minion.ExecuteAction(actionIndex, _currentTargetDirection);
            }
        }
        
        public void RegisterMinion(MinionType type, IMinion minion)
        {
            if (!_minionGroups.ContainsKey(type))
                _minionGroups[type] = new List<IMinion>();
        
            _minionGroups[type].Add(minion);
        }
    }
}