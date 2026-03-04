using Enemy.Controls;

namespace EventBus
{
    /// <summary>
    /// Yeni bir minion eklendiğinde sistemdeki diğer sınıfları bilgilendirmek için kullanılan veri yapısı.
    /// </summary>
    public readonly struct MinionAddedEvent
    {
        public int Minion { get; }

        public MinionAddedEvent(int minion)
        {
            Minion = minion;
        }
    }
}