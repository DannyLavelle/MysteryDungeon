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
    [Header("Base Attributes")]
    public PersonalityType personality; // Personality type
    public int level = 1;
    public int energy = 50;

    [Header("Health and Defence")]
    public float currentHealth = 20;
    public float maxHealth;
    public float armour;
    public float maxArmour = 75;

    [Header("Combat")]
    public float damage = 3;
    public int attackRange = 1;

    [Header("AI & Behavior")]
    public int detectionRange;
    public int allyThreshold;
   [Range(0,10)] public int intelligence;


    [Header("Experience: Player")]
    public float xp;
    public float xpToNextLevel = 10;
    public int xpIncrease;

    [Header("Experience: Enemy")]
    public int xpOnDeath = 1;   
    public bool multiplyXP;



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
    
    public void TakeDamage(float amount)
    {
        currentHealth -= amount * ((100-armour) /100);
        if(currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal (float amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            maxHealth = (currentHealth - maxHealth)/2;
            currentHealth = maxHealth;
            Debug.Log("Increasing Max Health");
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
