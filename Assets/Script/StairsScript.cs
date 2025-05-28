using UnityEngine;

public class StairsScript : MonoBehaviour
{
    public class TrapScript : ItemBase
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
                dungeon.GenerateDungeon();
                Destroy(gameObject);
            }
        }
    }
}
