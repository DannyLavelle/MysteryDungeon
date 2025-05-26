using UnityEngine;

public class EnemyBasicAttack : EnemyAttackBase
{


    public override void Attack(Stats playerStats,Stats enemyStats)
    {
       playerStats.TakeDamage(enemyStats.damage);
        Debug.Log("Hit player");
    }

   
}
