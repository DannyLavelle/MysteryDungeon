using UnityEngine;

public class EnemyBasicAttack : MonoBehaviour,  IEnemyAttackBase
{


    public  void Attack(Stats playerStats,Stats enemyStats)
    {
       playerStats.TakeDamage(enemyStats.damage);
        Debug.Log("Hit player");
    }

   
}
