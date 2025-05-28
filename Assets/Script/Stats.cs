using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
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
    int maxEnergy;
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


    public string currentAction;
    public Image healthBar;
    public Image energyBar;
    public TMP_Text levelText;
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
        maxEnergy = energy;
        currentHealth = maxHealth;
        IncreaseForFloor();
        UpdateHealthBar();
        
    }
    private void Update()
    {
       if(personality == PersonalityType.Player) UpdateEnergyBar();
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount * ((100-armour) /100);
        UpdateHealthBar();
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
            maxHealth += (currentHealth - maxHealth)/2;
            currentHealth = maxHealth;
            Debug.Log("Increasing Max Health");
        }
    }
    public void Die()
    {
        maxHealth = 0;
        if(personality == PersonalityType.Player)
        {
            SaveHighScore(TurnManager.Instance.FLoorNumber);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
            level++;
            maxHealth += 5;
            damage += 1;
            levelText.text = ("Level " + level);

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

    public void AddArmour(int amount)
    {
        armour += amount;
        if(armour >= maxArmour)
        {
            armour = maxArmour;
        }

    }

    public void IncreaseForFloor()
    {
        if (personality == PersonalityType.Player)return;
            
        int floor = TurnManager.Instance.FLoorNumber;

        maxHealth = maxHealth + (5* floor);
        currentHealth = maxHealth;

        damage = damage + floor;

        detectionRange += floor;

        xpOnDeath += floor;

    }
    private void UpdateHealthBar()
    {
        float healthPercent = currentHealth / maxHealth;

        if (healthBar != null)
        {
           
            healthBar.fillAmount = healthPercent;
        }
 
    }
    private void UpdateEnergyBar()
    {
        float e = energy;
        float me = maxEnergy;
        float Percent = e/me;
        if (Percent > 1) Percent = 1;
        if (energyBar != null)
        {
            Debug.Log("Energy: "+ energy + " max: " + maxEnergy + " percent: " + Percent);
            energyBar.fillAmount = Percent;
        }

    }

    public void SaveHighScore(int currentScore)
    {
        int highScore = PlayerPrefs.GetInt("HighScore", 0); // Default to 0 if not set
        if (currentScore > highScore)
        {
            PlayerPrefs.SetInt("HighScore", currentScore);
            PlayerPrefs.Save(); // Write to disk
            Debug.Log("New High Score: " + currentScore);
        }
    }
    
}
