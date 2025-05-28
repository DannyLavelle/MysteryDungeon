using UnityEngine;

public class StairsScript : ItemBase
{
   
        [Header("Stair Settings")]
        [SerializeField] private string trapName = "Stairs";
        [SerializeField] private int effectAmount = 10;
        public GameObject trapObject;


        public override void Consume(Stats stats)
        {
            DungeonGenerator dungeon = FindAnyObjectByType<DungeonGenerator>();
           if(dungeon != null)
            {
                TurnManager.Instance.FLoorNumber++;
                Debug.Log("Increased turn number");
                dungeon.GenerateDungeon();
                Destroy(gameObject);
            }
        }
    
}
