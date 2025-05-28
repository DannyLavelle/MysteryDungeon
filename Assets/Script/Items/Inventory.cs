using UnityEngine;


public class Inventory : MonoBehaviour
{
   public GameObject[] inventory = new GameObject[3];
    public bool debug;
    public int inventorycount;
    public bool pickup(GameObject item, GameObject itemObject)
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
            inventory[index].SetActive(true);
            ItemBase bs =inventory[index].GetComponent<ItemBase>();
            Debug.Log(bs);
            bs.Consume(gameObject.GetComponent<Stats>());
            Debug.Log("Use Item: " + inventory[index]);
            Destroy(inventory[index]);
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
               ;
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
