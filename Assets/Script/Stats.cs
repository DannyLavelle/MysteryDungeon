using UnityEngine;
public enum PersonalityType
{
    None,
    Aggressive,
    Shy,
    Player

}
public class Stats : MonoBehaviour
{
    public int level = 1;
    public int currentHealth = 20;
    public int energy = 50;
    public int maxHealth;
    public PersonalityType personality;//personality type 
    public int damage = 3;
    public int detectionRange;
    public int allyThreshold;
    public Stats(int level = 0, int energy = 0, int maxHealth = 0, PersonalityType personality = PersonalityType.None, int damage = 0, int detectionRange = 0)
    {
        this.level = level;
     
        this.energy = energy;
        this.maxHealth = maxHealth;
        this.personality = personality;
        this.damage = damage;
        this.detectionRange = detectionRange;
        
    }

    private void Start()
    {
        currentHealth = maxHealth;
        
    }
}
