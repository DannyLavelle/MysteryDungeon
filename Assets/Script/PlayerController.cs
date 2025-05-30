using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    public Vector2Int GridPosition;
    public Vector2Int LastMoveDirection { get; private set; } = Vector2Int.zero;
    public DungeonContainer dungeonGridContainer;


    private void Start()
    {
        
        dungeonGridContainer = FindAnyObjectByType<DungeonContainer>();
    }
    public IEnumerator StepTo(Vector2Int target)
    {
        
        LastMoveDirection = target - GridPosition;

      
        GridPosition = target;
        Vector3 worldPos = new Vector3(target.x, 0, target.y);
        transform.position = worldPos;
        CheckForItemOnCurrentTile();
        yield return new WaitForSeconds(0.05f);
    }
    private void CheckForItemOnCurrentTile()
    {
        // Get your current grid position
        Vector2Int myPos = GridPosition;

        // Find all MonoBehaviours that implement IItem
        ItemReference[] itemList= FindObjectsByType<ItemReference>(FindObjectsSortMode.None)/*.OfType<IItem>().ToList()*/;



        foreach (ItemReference item in itemList)
        {
            
           GameObject itemObject = item.gameObject;
            Vector2Int itemPos = GridUtility.WorldToGridPosition(itemObject.transform.position);

            if (itemPos == myPos)
            {
                
                Inventory inventory = GetComponent<Inventory>();
                if(inventory.pickup(itemObject, itemObject))
                {
                    itemObject.SetActive(false);
                }    
                break;
            }
        }
    }
}
