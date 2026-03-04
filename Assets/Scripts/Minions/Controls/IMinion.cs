using UnityEngine;

namespace Enemy.Controls
{
    public interface IMinion
    {
        void ExecuteAction(int actionIndex, Vector3 targetDirection);
    }
}