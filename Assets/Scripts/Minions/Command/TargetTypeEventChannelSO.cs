using UnityEngine;
using UnityEngine.Events;

namespace Enemy.Command
{
    [CreateAssetMenu(menuName = "Events/Target Type Event Channel")]
    public class TargetTypeEventChannelSO : ScriptableObject
    {
        public UnityAction<TargetType> OnEventRaised;
        public void RaiseEvent(TargetType value) => OnEventRaised?.Invoke(value);
    }
}