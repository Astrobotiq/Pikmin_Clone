using UnityEngine;
using UnityEngine.Events;

namespace Enemy.Command
{
    [CreateAssetMenu(menuName = "Events/Command Type Event Channel")]
    public class CommandTypeEventChannelSO : ScriptableObject
    {
        public UnityAction<CommandType> OnEventRaised;
        public void RaiseEvent(CommandType value) => OnEventRaised?.Invoke(value);
    }
}


namespace Enemy.Command
{
}