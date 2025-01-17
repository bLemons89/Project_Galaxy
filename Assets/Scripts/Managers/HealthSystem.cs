using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;

public class HealthSystem : MonoBehaviour
{
    [Header("===== STATS =====")]
    [SerializeField] float maxHealth;
    float currentHealth;

    [Header("===== VISUAL =====")]
    [SerializeField] Image healthBarBack;
    [SerializeField] Image healthBarFill;
    [SerializeField] Gradient healthGradient;
    [SerializeField] float speed;
    
    float lerpSpeed;

    [Header("===== CRITICAL HEALTH =====")]
    [SerializeField] TextMeshProUGUI critWarningText;
    [SerializeField] float critHealth = 0.2f;
    [SerializeField] float flashSpeed = 0.5f;
    
    //[Header("===== STATUS EFFECT =====")]

    //[Header("===== DAMAGE TYPE =====")]

    // Flags //
    bool isMax;
    bool isCrit;
    bool isDead;
    //bool isInvincible;

    public bool IsDead { get {  return isDead; } }
    //public bool IsInvincible { get; set; }  

    void Start()
    {
        currentHealth = maxHealth;

        //set flags
        isMax = true;
        isCrit = false;
        isDead = false;
    }

    // Update is called once per frame
    void Update()
    {
        //make sure health cannot go above max
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
            isMax = true;
        }
        //most conditions
        else if (currentHealth < maxHealth)
        {
            isMax = false;
        }
        
        lerpSpeed = speed * Time.deltaTime;

        UpdateHealthBar();
        ColorChange();

        CheckForCriticalHealth();
    }

    // Damage/Heal //
    public void Damage(float damageAmt)
    {
        if (currentHealth > 0)
        {
            currentHealth -= damageAmt;
        }
        //check for death
        if (currentHealth <= 0 && !isDead)
        {
            isDead = true;
            //Handle Death - message, respawn
        }
    }
    public void Heal(float healAmt)
    {
        if(currentHealth < maxHealth)
        {
            currentHealth += healAmt;
        }
    }


    // Health Bar //
    void UpdateHealthBar()
    {
        healthBarFill.fillAmount = Mathf.Lerp(healthBarFill.fillAmount, currentHealth / maxHealth, lerpSpeed);
    }
    void ColorChange()
    {
        //try gradient
        healthBarFill.color = healthGradient.Evaluate(currentHealth / maxHealth);

        //Color healthColor = Color.Lerp(Color.red, Color.green, currentHealth/maxHealth);
        //healthBarFill.color = healthColor;
    }


    // Critical Health //
   void CheckForCriticalHealth()
    {
        if (currentHealth <= maxHealth * critHealth)
        {
            if(!isCrit)
            {
                isCrit = true;
                StartFlashing();
            }
        }
        else
        {
            if (isCrit)
            {
                isCrit = false;
                StopFlashing();
            }
        }
    }
    void StartFlashing()
    {
        CancelInvoke("FlashCritWarning"); // Ensure no duplicate calls
        InvokeRepeating("FlashCritWarning", 0f, flashSpeed); // Start flashing
    }
    void StopFlashing()
    {
        CancelInvoke("FlashCritWarning");
        critWarningText.gameObject.SetActive(false); // Hide the warning text
    }
    void FlashCritWarning()
    {
        critWarningText.gameObject.SetActive(!critWarningText.gameObject.activeSelf);
    }






    //void CheckForCriticalHealth()
    //{
    //    if (currentHealth <= maxHealth * critHealth)
    //    {   //make sure coroutine isn't already playing
    //        if(!isCrit)
    //        {
    //            //change flag
    //            isCrit = true;
    //            critWarningText.gameObject.SetActive(true); // Ensure it starts visible
    //            InvokeRepeating("FlashCritWarning", 0, flashSpeed);

    //        }
    //    }
    //    else
    //    {  
    //        //stop when leaves the threshhold
    //        if(isCrit)
    //        {
    //            //change flag
    //            isCrit = false;
    //            CancelInvoke("FlashCritWarning"); // Stop repeating

    //            //turn off
    //            critWarningText.gameObject.SetActive(false);
    //        }           
    //    }
    //}

    //private IEnumerator CritWarning()
    //{
    //    while(true)
    //    {
    //        //toggle text on/off
    //        critWarningText.gameObject.SetActive(!critWarningText.gameObject.activeSelf);
    //        yield return new WaitForSeconds(flashSpeed);
    //    }
    //}

}
