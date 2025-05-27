using UnityEngine;

public interface IItem
{
    string ItemName { get; }
    int EffectAmount { get; }
    public void Consume(Stats stats);
  
}
