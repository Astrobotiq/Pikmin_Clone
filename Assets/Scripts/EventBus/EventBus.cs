using System;

namespace EventBus
{
    /// <summary>
    /// Sistemdeki bileşenlerin birbirine gevşek bağlı (loosely coupled) şekilde haberleşmesini sağlayan, tahsisat yapmayan (zero-allocation) olay veri yolu.
    /// </summary>
    public static class EventBus<T> where T : struct
    {
        public static event Action<T> OnEvent;

        public static void Raise(T eventArgs)
        {
            OnEvent?.Invoke(eventArgs);
        }
    }
}