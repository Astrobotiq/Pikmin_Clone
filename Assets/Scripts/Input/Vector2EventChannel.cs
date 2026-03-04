using UnityEngine;
using UnityEngine.Events;

namespace Input
{
    /// <summary>
    /// Vektör2 verisi taşıyan, input ve hareket sistemleri arasında köprü kuran Event Channel.
    /// </summary>
    [CreateAssetMenu(menuName = "Events/Vector2 Event Channel")]
    public class Vector2EventChannelSO : ScriptableObject
    {
        public UnityAction<Vector2> OnEventRaised;

        public void RaiseEvent(Vector2 value)
        {
            OnEventRaised?.Invoke(value);
        }
    }
}