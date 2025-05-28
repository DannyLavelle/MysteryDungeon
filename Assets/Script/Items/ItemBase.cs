using UnityEngine;

public abstract class ItemBase :MonoBehaviour
{

    //[SerializeField] public string itemName = "Potion";
    //[SerializeField] public int effectAmount = 10;


    public  abstract void Consume(Stats stats);
    
}
