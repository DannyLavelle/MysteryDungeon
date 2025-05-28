using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Inventory : MonoBehaviour
{
   public GameObject[] inventory = new GameObject[3];
    public bool debug;
    public int inventorycount;
    public TMP_Text[] inventoryText = new TMP_Text[3];
    public bool pickup(GameObject item, GameObject itemObject)
    {
        if(item.CompareTag("Trap") || item.CompareTag("Stairs"))
        {
            UseItem(item, false);
            Debug.Log("Using Trap");
            return false;
        }

        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i] == null)
            {
                inventory[i] = item;
                Debug.Log("Picked up item");
                DisplayInventory(i);
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
            //Debug.Log(bs);
            bs.Consume(gameObject.GetComponent<Stats>());
            Debug.Log("Use Item: " + inventory[index]);
            Destroy(inventory[index]);
            inventory[index] = null;
            DisplayInventory(index);



        }
    }
    public void UseItem(GameObject obj,bool toDestroy)
    {
        ItemBase bs = obj.GetComponent<ItemBase>();
        bs.Consume(gameObject.GetComponent<Stats>());
        if (obj == null) return;
        obj.SetActive(true);
        if (toDestroy) Destroy(obj);
       
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
            //DisplayInventory();

        }
    }
    public void DisplayInventory(int index)
    {
        if( inventory[index] != null)
        {
            inventoryText[index].text = inventory[index].name;
        }
        else
        {
            inventoryText[index].text = "";
        }
          
    }
}
