using UnityEngine;
using TMPro;
public class LowestFloorLoader : MonoBehaviour
{
    TMP_Text text;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        text = GetComponent<TMP_Text>();
        text.text = ("Lowest Floor: " + LoadHighScore());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public int LoadHighScore()
    {
        return PlayerPrefs.GetInt("HighScore", 0); // Default to 0
    }
}
