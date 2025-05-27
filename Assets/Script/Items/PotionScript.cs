using UnityEngine;

public class PotionScript : MonoBehaviour,IItem
{

    [SerializeField] private string itemName = "Potion";
    [SerializeField] private int effectAmount = 10;

    public string ItemName => itemName;

    public int EffectAmount => effectAmount;

    public void Consume(Stats stats)
    {
        stats.Heal(EffectAmount);
    }

}
