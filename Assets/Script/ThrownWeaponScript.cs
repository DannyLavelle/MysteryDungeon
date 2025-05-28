using System.Collections;
using UnityEngine;

public class ThrownWeaponScript : MonoBehaviour
{
   public  Vector2 Direction;
   public int range = 10;
   public float stepDelay;
   private Stats weaponStats ;
    Enemy[] enemies;
    private void Start()
    {
        weaponStats = GetComponent<Stats>();
         enemies = GameObject.FindObjectsByType<Enemy>(FindObjectsSortMode.None);

        StartCoroutine(TravelAndCheck());

    }
    private IEnumerator TravelAndCheck()
    {
       
        Vector2Int dir = new Vector2Int(
            Mathf.Clamp(Mathf.RoundToInt(Direction.x), -1, 1),
            Mathf.Clamp(Mathf.RoundToInt(Direction.y), -1, 1)
        );

       
        Vector2Int currentGrid = GridUtility.WorldToGridPosition(transform.position);

        
        for (int i = 0; i < range; i++)
        {
            currentGrid += dir;
            Vector3 worldPos = GridUtility.GridToWorldPosition(currentGrid);
            transform.position = worldPos;

           
            foreach (var e in enemies)
            {
                Vector2Int enemyPos = GridUtility.WorldToGridPosition(e.transform.position);
                if (enemyPos == currentGrid)
                {
                    
                    Stats stats = e.GetComponent<Stats>();
                    if (stats != null)
                        stats.TakeDamage(weaponStats.damage);
                    enemies = GameObject.FindObjectsByType<Enemy>(FindObjectsSortMode.None);//Recalcuate position after a hit to avoid atempted references


                    //Destroy(gameObject);
                    //yield break;
                }
            }

           
            if (stepDelay > 0f)
                yield return new WaitForSeconds(stepDelay);
            else
                yield return null;
        }

       
        Destroy(gameObject);
    }

    void CheckForEnemy()
    {

    }

}
