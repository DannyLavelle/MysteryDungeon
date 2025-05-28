using UnityEngine;

public class PotionScript : ItemBase
{
    [Header("Potion Settings")]
    [SerializeField] private string potionName = "Health Potion";
    [SerializeField] private int effectAmount = 10;


    public override void Consume(Stats stats)
    {
        stats.Heal(effectAmount);
        Debug.Log($"Potion consumed: healed {effectAmount} HP");
    }
}
