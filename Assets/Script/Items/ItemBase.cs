using UnityEngine;

public abstract class ItemBase :MonoBehaviour
{

    //[SerializeField] public string itemName = "Potion";
    //[SerializeField] public int effectAmount = 10;


    public  abstract void Consume(Stats stats);
    
    public virtual Sprite Sprite()
    {
        SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();

        if (sr != null) return sr.sprite;

        Debug.LogWarning($"No SpriteRenderer found in {name} or its children.");
        return null;


    }
}
