using UnityEngine;

public interface ItemBase 
{

    //[SerializeField] public string itemName = "Potion";
    //[SerializeField] public int effectAmount = 10;


    public  void Consume(Stats stats);
    
}
