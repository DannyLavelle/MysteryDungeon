using UnityEngine;
using UnityEngine.UI;
public class HealthBar : MonoBehaviour
{
    public Image fill;
    public float changeSpeed;

    public float fillAmount;

    private void Update()
    {
        fill.fillAmount = Mathf.Lerp(fill.fillAmount,fillAmount,changeSpeed + Time.deltaTime);
    }
}
