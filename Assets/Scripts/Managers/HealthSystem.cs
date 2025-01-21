using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;
using DG.Tweening;
using UnityEngine.Events;

public class HealthSystem : MonoBehaviour
{
    [Header("===== STATS =====")]
    [SerializeField] float maxHealth;
    float currentHealth;
    float previousHealth;

    [Header("===== VISUAL =====")]
    [SerializeField] Image healthBarBack;
    [SerializeField] Image healthBarFill;
    [SerializeField] Gradient healthGradient;
    [SerializeField] float topFillSpeed;
    [SerializeField] float bottomFillSpeed;
    [SerializeField] Image easeBar;
    [SerializeField] float dmgFlashDuration;

    [Header("===== CRITICAL HEALTH =====")]
    [SerializeField] TextMeshProUGUI critWarningText;
    [SerializeField] float critHealth = 0.2f;
    [SerializeField] float flashSpeed = 0.5f;
    
    //[Header("===== STATUS EFFECT =====")]

    //[Header("===== DAMAGE TYPE =====")]

    // Flags //
    bool isHeal;
    bool isCrit;
    bool isDead;
    //bool isInvincible;

    public bool IsDead { get {  return isDead; } }
    //public bool IsInvincible { get; set; }  

    //===== EVENTS =====
    public UnityEvent OnDeath;

    void Start()
    {
        currentHealth = maxHealth;

        //set flags
        isHeal = false;
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
        }
        
        UpdateHealthBar();

        CheckForCriticalHealth();
    }

    // Damage/Heal //
    public void Damage(float damageAmt)
    {
        if (currentHealth > 0)
        {
            currentHealth -= damageAmt;
            isHeal = false;

            if(CompareTag("Player"))
            {
                Invoke("FlashDamageScreen", 0f);
                AudioManager2.PlaySound(AudioManager2.Sound.PlayerDamage);
            }
        }
        //check for death
        if (currentHealth <= 0)
        {
            //isDead = true;
            //Handle Death - message, respawn
            if (this.CompareTag("Player"))
            {
                OnDeath?.Invoke();

                currentHealth = maxHealth;
            }
            else
                Destroy(this.gameObject);
        }
    }
    public void Heal(float healAmt)
    {
        if(currentHealth < maxHealth)
        {
            currentHealth += healAmt;
            isHeal = true;
        }
    }

    // Health Bar //
    void UpdateHealthBar()
    {
        float fillAmount = currentHealth / maxHealth;

        DOTween.Kill(healthBarFill);
        DOTween.Kill(easeBar);

        if (isHeal)
        {
            easeBar.color = Color.green;
            healthBarFill.DOFillAmount(fillAmount, topFillSpeed);
            easeBar.DOFillAmount(fillAmount, bottomFillSpeed);
        }
        else if (easeBar != null)
        {
            easeBar.color = Color.red;
            healthBarFill.DOFillAmount(fillAmount, bottomFillSpeed);
            easeBar.DOFillAmount(fillAmount, topFillSpeed);
        }

        if(healthBarFill != null)
            healthBarFill.color = healthGradient.Evaluate(currentHealth / maxHealth);        
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
        // Ensure no duplicate calls
        CancelInvoke("FlashCritWarning"); 
        // Start flashing
        InvokeRepeating("FlashCritWarning", 0f, flashSpeed);
    }
    void StopFlashing()
    {
        CancelInvoke("FlashCritWarning");

        if(critWarningText != null)
        { 
            // Hide the warning text
            critWarningText.gameObject.SetActive(false);
        }
    }
    void FlashCritWarning()
    {
        if(critWarningText != null) 
        { 
            critWarningText.gameObject.SetActive(!critWarningText.gameObject.activeSelf);
        }
    }
    
    // Player Damage Screen //
    private void FlashDamageScreen()
    {
        GameManager.instance.PlayerDamageScreen.SetActive(true);
        Invoke("HideDamageScreen", dmgFlashDuration);
    }
    private void HideDamageScreen()
    {
        GameManager.instance.PlayerDamageScreen.SetActive(false);
    }
}
