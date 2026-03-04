using Enemy.Command;
using TMPro;
using UnityEngine;
// TextMeshPro kütüphanesi (Standart UI Text için bunu kullanmalısın)

namespace UI.Minion
{
    /// <summary>
    /// Event Channel'lardan gelen veriyi dinleyerek UI üzerindeki metinleri güncelleyen sınıf.
    /// Observer Pattern uygular.
    /// </summary>
    public class MinionHUD : MonoBehaviour
    {
        [Header("Listening Channels")]
        [SerializeField] private MinionTypeEventChannelSO _minionSelectionChannel;
        [SerializeField] private CommandTypeEventChannelSO _actionSelectionChannel;
        [SerializeField] private TargetTypeEventChannelSO _targetSelectionChannel;

        [Header("UI Text Elements")]
        [SerializeField] private TextMeshProUGUI _minionTypeText;
        [SerializeField] private TextMeshProUGUI _actionTypeText;
        [SerializeField] private TextMeshProUGUI _targetTypeText;

        [Header("Display Settings")]
        [SerializeField] private string _defaultText = "Waiting...";
        [SerializeField] private Color _activeColor = Color.green;
        [SerializeField] private Color _inactiveColor = Color.gray;

        private void OnEnable()
        {
            if (_minionSelectionChannel != null)
                _minionSelectionChannel.OnEventRaised += UpdateMinionUI;

            if (_actionSelectionChannel != null)
                _actionSelectionChannel.OnEventRaised += UpdateActionUI;

            if (_targetSelectionChannel != null)
                _targetSelectionChannel.OnEventRaised += UpdateTargetUI;
        }

        private void OnDisable()
        {
            if (_minionSelectionChannel != null)
                _minionSelectionChannel.OnEventRaised -= UpdateMinionUI;

            if (_actionSelectionChannel != null)
                _actionSelectionChannel.OnEventRaised -= UpdateActionUI;

            if (_targetSelectionChannel != null)
                _targetSelectionChannel.OnEventRaised -= UpdateTargetUI;
        }

        private void Start()
        {
            // Oyun başladığında UI'ı varsayılan duruma getir
            ResetUI();
        }

        private void UpdateMinionUI(MinionType selectedType)
        {
            if (selectedType == MinionType.All) // Veya None mantığına göre
            {
                SetText(_minionTypeText, "ALL UNITS", _activeColor);
                return;
            }

            // Enum ismini direkt yazdırıyoruz
            SetText(_minionTypeText, $"UNIT: {selectedType}", _activeColor);
        }

        private void UpdateActionUI(CommandType commandType)
        {
            if (commandType == CommandType.None)
            {
                SetText(_actionTypeText, "NO ACTION", _inactiveColor);
                return;
            }

            SetText(_actionTypeText, $"ACTION: {commandType}", _activeColor);
        }

        private void UpdateTargetUI(TargetType targetType)
        {
            if (targetType == TargetType.None)
            {
                SetText(_targetTypeText, "NO TARGET", _inactiveColor);
                return;
            }

            SetText(_targetTypeText, $"AIM: {targetType}", _activeColor);
        }

        private void SetText(TextMeshProUGUI textElement, string content, Color color)
        {
            textElement.text = content;
            textElement.color = color;
        }

        private void ResetUI()
        {
            SetText(_minionTypeText, "SELECT UNIT", _inactiveColor);
            SetText(_actionTypeText, "WAITING CMD", _inactiveColor);
            SetText(_targetTypeText, "WAITING DIR", _inactiveColor);
        }
    }
}
