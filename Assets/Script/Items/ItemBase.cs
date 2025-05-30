using UnityEngine;

public abstract class ItemBase :MonoBehaviour
{

    public  abstract void Consume(Stats stats);
    
    public virtual Sprite Sprite()
    {
        SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();

        if (sr != null) return sr.sprite;

        Debug.LogWarning($"No SpriteRenderer found in {name} or its children.");
        return null;


    }
}
