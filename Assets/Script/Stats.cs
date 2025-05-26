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
    public float currentHealth = 20;
    public int energy = 50;
    public float maxHealth;
    public PersonalityType personality;//personality type 
    public int damage = 3;
    public int detectionRange;
    public int allyThreshold;
    public float armour;
    public float maxArmour = 75;
    public float xp;
    public float xpToNextLevel = 10 ;
    public int xpOnDeath = 1 ;
    public int xpIncrease;
    bool multiplyXP;
    public int attackRange = 1;
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
    
    public void TakeDamage(int amount)
    {
        currentHealth -= amount * ((100-armour) /100);
        if(currentHealth <= 0)
        {
            Die();
        }
    }
    public void Die()
    {
        if(personality == PersonalityType.Player)
        {
            
        }
        else
        {
            Stats playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<Stats>();
            playerStats.AddXP(xpOnDeath);
            Destroy(gameObject);
        }

    }
    public void AddXP(int amount)
    {
        xp += amount;
        if(xp >= xpToNextLevel)
        {
            //level up
            xp -= xpToNextLevel;
            xpToNextLevel = 0;
            IncreaseXP();
        }
    }
    public void IncreaseXP()
    {
        if(multiplyXP)
        {
            xpToNextLevel *= xpIncrease;

        }
        else
        {
            xpToNextLevel += xpIncrease;
        }
    }
}
