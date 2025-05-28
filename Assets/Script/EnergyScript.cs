using UnityEngine;

public class EnergyScript : ItemBase
{
    [Header("Potion Settings")]
    [SerializeField] private string potionName = "Energy Potion";
    [SerializeField] private int effectAmount = 10;


    public override void Consume(Stats stats)
    {
        stats.energy+= effectAmount;
        Debug.Log($"Potion consumed: healed {effectAmount} Energy");
    }
}
