using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MentalHealthBar : MonoBehaviour
{
    public float startingHealth = 100f;
    public float currentHealth;
    public Image brainImage;
    public Text healthText;

    [System.Serializable]
    public class TagDecreaseRate
    {
        public string tag;
        public float decreaseRate;
    }

    public TagDecreaseRate[] tagDecreaseRates;

    private Dictionary<string, float> decreaseRateDict = new Dictionary<string, float>();

    void Start()
    {
        currentHealth = startingHealth;
        UpdateHealthUI();

        // Create dictionary of decrease rates for each tag
        foreach (TagDecreaseRate rate in tagDecreaseRates)
        {
            decreaseRateDict.Add(rate.tag, rate.decreaseRate);
        }
    }

    void Update()
    {
        // Decrease health over time based on decrease rate of active objects with specified tags
        float decreaseRate = 0f;

        foreach (KeyValuePair<string, float> entry in decreaseRateDict)
        {
            GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag(entry.Key);

            foreach (GameObject obj in objectsWithTag)
            {
                if (obj.activeInHierarchy)
                {
                    decreaseRate += entry.Value;
                    break;
                }
            }
        }

        currentHealth -= decreaseRate * Time.deltaTime;
        currentHealth = Mathf.Clamp(currentHealth, 0f, startingHealth);

        UpdateHealthUI();
    }

    void UpdateHealthUI()
    {
        float fillAmount = currentHealth / startingHealth;
        brainImage.fillAmount = fillAmount;

        // Calculate the color based on the fill amount
        Color color = Color.Lerp(Color.red, Color.green, fillAmount);
        brainImage.color = color;

        healthText.text = currentHealth.ToString("0");
    }

}
