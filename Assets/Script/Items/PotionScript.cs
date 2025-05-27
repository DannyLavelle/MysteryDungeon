using UnityEngine;

public class PotionScript : MonoBehaviour,IItem
{

    public int effectAmount = 10;
    public void Consume(Stats stats)
    {
        stats.Heal(effectAmount);
    }

}
