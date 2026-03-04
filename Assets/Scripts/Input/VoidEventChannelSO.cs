using UnityEngine;
using UnityEngine.Events;

namespace Input
{
    /// <summary>
    /// Parametresiz olaylar (Örn: Zıplama, Etkileşim) için Event Channel.
    /// </summary>
    [CreateAssetMenu(menuName = "Events/Void Event Channel")]
    public class VoidEventChannelSO : ScriptableObject
    {
        public UnityAction OnEventRaised;

        public void RaiseEvent()
        {
            OnEventRaised?.Invoke();
        }
    }
}