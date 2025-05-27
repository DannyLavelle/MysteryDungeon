using System.Collections;
using UnityEngine;

public class AttackAction : GameAction
{
    public PlayerController player { get; private set; }
     public Vector2Int targetTile {  get; private set; }
    public float damageMultiplier = 1;
    public int energyCost = 0;
    public AttackAction(PlayerController player, Vector2Int targetTile)
    {
        this.player = player;
        this.targetTile = targetTile;
    }

    public override IEnumerator Execute()
    {
        Enemy enemy = GetEnemyAtPosition(targetTile);
        if (enemy != null)
        {
          AnimateAttack();
          DamageEnemy(enemy);
        }
        else
        {
            Debug.Log("No enemy to attack at " + targetTile);
        }

        yield return TurnManager.Instance.EnemyTurn();

    }

    public Enemy GetEnemyAtPosition(Vector2Int pos)
    {
        foreach (Enemy enemy in GameObject.FindObjectsByType<Enemy>(FindObjectsSortMode.None))
        {
            Vector2Int enemyPos = GridUtility.WorldToGridPosition(enemy.transform.position);
            if (enemyPos == pos)
                return enemy;
        }
        return null;
    }

    public void DamageEnemy(Enemy enemy)
    {
        Debug.Log("Attacking enemy at " + targetTile);
        Stats stats = enemy.GetComponent<Stats>();
        Stats playerStats = player.GetComponent<Stats>();
        if (stats != null)
        {
            //playerStats.damage
            stats.TakeDamage(playerStats.damage * damageMultiplier); // Replace with real damage logic
            Debug.Log("Enemy HP: " + stats.currentHealth);
        }
    }

    void AnimateAttack()
    {

    }
    public void TakeCost(int amount)
    {
        Stats playerStats = player.GetComponent<Stats>();
        if(playerStats.energy < amount)//if player can't pay in energy they pay in health 
        {
            amount -= playerStats.energy;
            playerStats.TakeDamage(amount);

        }
        else
        {
            playerStats.energy -= amount;
        }
    }

}
