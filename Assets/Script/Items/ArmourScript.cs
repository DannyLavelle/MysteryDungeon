using UnityEngine;

public class ArmourScript : ItemBase
{
  [Header("Potion Settings")]
    [SerializeField] private string potionName = "Armour Potion";
    [SerializeField] private int effectAmount = 10;


    public override void Consume(Stats stats)
    {
        stats.AddArmour(effectAmount);
        //Debug.Log($"Potion consumed: healed {effectAmount} Energy");
    }
}
