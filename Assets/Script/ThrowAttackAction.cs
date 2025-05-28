using UnityEngine;
using System.Collections;

public class ThrowAttackAction : AttackAction
{
    public GameObject spawnPrefab = Resources.Load<GameObject>("Throwing");
    public ThrowAttackAction(PlayerController player, Vector2Int targetTile)
         : base(player, targetTile)
    {
        // You can override cost/damage multiplier here if you like
        this.energyCost = 10;
        this.damageMultiplier = .5f;
    }
    public override IEnumerator Execute()
    {

        if (spawnPrefab == null)
        {
            Debug.LogError("throwing prefab not assigned is not assigned!");
        }
        else
        {
            Vector3 worldPos = GridUtility.GridToWorldPosition(targetTile);
            
            // Instantiate the mine at ground level; assumes the prefab has a trigger collider
            GameObject thrownWeapon = GameObject.Instantiate(spawnPrefab, player.transform.position, Quaternion.identity);
            Stats thrownStats = thrownWeapon.GetComponent<Stats>();
            Debug.Log($"Placing throwing weapon");
            thrownStats.damage = player.GetComponent<Stats>().damage * damageMultiplier;
            var playerpos = GridUtility.WorldToGridPosition(player.transform.position);
            ThrownWeaponScript thrownWeaponScript = thrownWeapon.GetComponent<ThrownWeaponScript>();
            
            thrownWeaponScript.Direction = GridUtility.Get8Direction(playerpos, targetTile);

            base.TakeCost(energyCost);


        }


       
        yield return TurnManager.Instance.EnemyTurn();
    }

}
