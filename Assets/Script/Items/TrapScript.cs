using UnityEngine;

public class TrapScript : ItemBase
{
    [Header("Trap Settings")]
    [SerializeField] private string trapNamen = "Spike Trap";
    [SerializeField] private int effectAmount = 10;
    public GameObject trapObject;


    public override void Consume(Stats stats)
    {
        trapObject.SetActive(true);
        stats.TakeDamage(effectAmount);
        //Debug.Log($"Potion consumed: healed {effectAmount} Energy");
    }
}

