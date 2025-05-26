using System.Collections;
using UnityEngine;

public class AttackAction : GameAction
{
    private PlayerController player;
    private Vector2Int targetTile;

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
            Debug.Log("Attacking enemy at " + targetTile);
            Stats stats = enemy.GetComponent<Stats>();
            if (stats != null)
            {
                stats.currentHealth -= 5; // Replace with real damage logic
                Debug.Log("Enemy HP: " + stats.currentHealth);
            }
        }
        else
        {
            Debug.Log("No enemy to attack at " + targetTile);
        }

        yield return new WaitForSeconds(0.1f);
    }

    private Enemy GetEnemyAtPosition(Vector2Int pos)
    {
        foreach (Enemy enemy in GameObject.FindObjectsByType<Enemy>(FindObjectsSortMode.None))
        {
            Vector2Int enemyPos = GridUtility.WorldToGridPosition(enemy.transform.position);
            if (enemyPos == pos)
                return enemy;
        }
        return null;
    }
}
