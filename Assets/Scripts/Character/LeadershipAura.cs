using Enemy.Controls;
using UnityEngine;

namespace Character
{
    public class LeadershipAura : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("BUMBUMBUMMMM");
            if (!other.TryGetComponent<MinionController>(out var minion)) return;
            Debug.Log($"name : {other.transform.name}");
            minion.JoinSquad();
        }
    }
}