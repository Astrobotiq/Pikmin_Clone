using Enemy.Command;
using Enemy.Controls;
using UnityEngine;

/// <summary>
/// Belirli bir hedefe saldırma mantığını yöneten komut.
/// </summary>
public class AttackCommand : BaseSmartCommand
{
    private float _attackCooldown;
    private float _lastAttackTime;

    public AttackCommand(Mover mover, IInteractable target, float range, float cooldown) 
        : base(mover, target, range)
    {
        _attackCooldown = cooldown;
    }

    protected override void PerformInteraction()
    {
        if (Time.time >= _lastAttackTime + _attackCooldown)
        {
            // Burada animasyon tetiklenebilir.
            _target.Interact(); // Hasar ver.
            _lastAttackTime = Time.time;
            
            // Saldırı bitince komut bitsin istiyorsan:
            // _isFinished = true; 
            // Ama genelde düşman ölene kadar sürer, o yüzden logic buraya eklenebilir.
        }
    }
}