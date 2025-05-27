using UnityEngine;

public class Inventory : MonoBehaviour
{
   public IItem[] inventory = new IItem[3];
    public void pickup(IItem item, GameObject itemObject)
    {
        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i] == null)inventory[i] = item;
        }
    }
    public void UseItem(int index)//use Item
    {
        if(inventory[index] != null)
        {
            inventory[index].Consume(gameObject.GetComponent<Stats>());
            inventory[index] = null;
        }
    }
}
