using UnityEngine;

public class PotionScript : ItemBase
{
    [Header("Potion Settings")]
    [SerializeField] private string potionName = "Potion";
    [SerializeField] private int effectAmount = 10;


    public  void Consume(Stats stats)
    {
        stats.Heal(effectAmount);
        Debug.Log($"Potion consumed: healed {effectAmount} HP");
    }
}
