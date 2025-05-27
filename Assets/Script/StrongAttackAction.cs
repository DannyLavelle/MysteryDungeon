using System.Collections;
using UnityEngine;

public class StrongAttackAction : AttackAction
{
    private float strongMultiplier = 2f; // Example: Double the damage
    private int newEnergyCost = 5;
    public StrongAttackAction(PlayerController player, Vector2Int targetTile)
        : base(player, targetTile)
    {
        this.damageMultiplier = strongMultiplier;
        this.energyCost = newEnergyCost;
    }
}