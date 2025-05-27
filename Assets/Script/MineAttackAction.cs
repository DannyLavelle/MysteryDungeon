using System.Collections;
using UnityEngine;

public class MineAttackAction : AttackAction
{
    // Assign this in code before using, or expose it another way
    public  GameObject spawnPrefab = Resources.Load<GameObject>("Mine");
    public string spawnMessage = "mine";
    public MineAttackAction(PlayerController player, Vector2Int targetTile)
        : base(player, targetTile)
    {
        // You can override cost/damage multiplier here if you like
        this.energyCost = 10;
        this.damageMultiplier = 3.5f;
    }

    public override IEnumerator Execute()
    {
        // 1) If there's an enemy under the tile, do a normal attack
        Enemy enemy = GetEnemyAtPosition(targetTile);
        if (enemy != null)
        {

            yield break;
        }

        // 2) Otherwise, place a mine
        if (spawnPrefab == null)
        {
            Debug.LogError("MineAttackAction.minePrefab is not assigned!");
        }
        else
        {
            Vector3 worldPos = GridUtility.GridToWorldPosition(targetTile);
            // Instantiate the mine at ground level; assumes the prefab has a trigger collider
            GameObject mine = GameObject.Instantiate(spawnPrefab, worldPos, Quaternion.identity);
            Stats mineStats = mine.GetComponent<Stats>();
            Debug.Log($"Placing{spawnMessage}");
            mineStats.damage = player.GetComponent<Stats>().damage * damageMultiplier;
           
        }

        // 3) Spend energy / play animation if needed
        TakeCost(energyCost);

        // 4) Now hand off to enemies
        yield return TurnManager.Instance.EnemyTurn();
    }
}
