using UnityEngine;

namespace Enemy.Command
{
    public interface IInteractable
    {
        Vector3 GetPosition();
        bool IsAvailable(); // Örn: Ağaç kesildiyse false döner.
        void Interact();    // Örn: Düşmansa hasar al, ağaçsa odun ver.
    }
}