using UnityEngine;


public class Inventory : MonoBehaviour
{
   public ItemBase[] inventory = new ItemBase[3];
    public bool debug;
    public int inventorycount;
    public bool pickup(ItemBase item, GameObject itemObject)
    {
        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i] == null)
            {
                inventory[i] = item;
                Debug.Log("Picked up item");
                DisplayInventory();
                return true;

            }

            
        }
        return false;
    }
    public void UseItem(int index)//use Item
    {
        if(inventory[index] != null)
        {
            inventory[index].Consume(gameObject.GetComponent<Stats>());
            Debug.Log("Use Item: " + inventory[index]);
            inventory[index] = null;
           

        }
    }
    private void Update()
    {

        if (debug)
        {
            for(int i = 0; i < inventory.Length; i++)
            {
                UseItem(i); 
                Debug.Log("Using item: "+ i);
            }
            debug = false;
            DisplayInventory();

        }
    }
    public void DisplayInventory()
    {
        inventorycount = 0;
        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i] != null) inventorycount += 1;
            Debug.Log(inventory[i]);
        }
    }
}
