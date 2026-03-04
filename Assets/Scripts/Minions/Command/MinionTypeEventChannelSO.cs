using UnityEngine;
using UnityEngine.Events;

namespace Enemy.Command
{
    [CreateAssetMenu(menuName = "Events/Minion Type Event Channel")]
    public class MinionTypeEventChannelSO : ScriptableObject
    {
        public UnityAction<MinionType> OnEventRaised;
        public void RaiseEvent(MinionType value) => OnEventRaised?.Invoke(value);
    }
}