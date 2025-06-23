using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;
using DG.Tweening;
using UnityEngine.Events;
using Unity.VisualScripting;

public class HealthSystem : MonoBehaviour
{
    [Header("===== PLAYER STATS =====")]
    [SerializeField] float maxHealth;
    float currentHealth;
    //float previousHealth;                 //unused

    [Header("===== VISUAL =====")]
    [Header("NOTE: Enemies only need Health Bar Fill")]
    [SerializeField] Image healthBarBack;
    [SerializeField] Image healthBarFill;
    [SerializeField] Image easeBar;
    [SerializeField] Gradient healthGradient;
    [SerializeField] float topFillSpeed;
    [SerializeField] float bottomFillSpeed;
    [SerializeField] float dmgFlashDuration;

    [Header("===== CRITICAL HEALTH =====")]
    [SerializeField] GameObject critWarning;
    //[SerializeField] TextMeshProUGUI critWarningText;
    [SerializeField] float critHealth = 0.2f;
    [SerializeField] float flashSpeed = 0.5f;

    private playerScript PlayerScript;
    //[Header("===== STATUS EFFECT =====")]

    //[Header("===== DAMAGE TYPE =====")]

    // Flags //
    bool isHeal;
    bool isCrit;
    bool isDead;
    //bool isInvincible;

    //getters
    public bool IsDead { get {  return isDead; } }
    public float MaxHealth { get; set; }
    public float CurrentHealth { get; set; }
    //public bool IsInvincible { get; set; }  

    //===== EVENTS =====
    [Header("If attached to player: Call Respawn() in playerScript")]
    public UnityEvent OnDeath;

    void Start()
    {
        currentHealth = maxHealth;
        PlayerScript = FindObjectOfType<playerScript>();

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
            currentHealth = maxHealth;

        UpdateHealthBar();

        if (this.CompareTag("Player"))       //applies to player only
            CheckForCriticalHealth();

    }

    // Damage/Heal //
    public void Damage(float damageAmt)
    {
        if (currentHealth > 0)
        {
            currentHealth -= damageAmt;
            isHeal = false;

            if(this.CompareTag("Player"))
            {
                Invoke("FlashDamageScreen", 0f);

                //Audio comes in here for damage sound
                AudioManager.instance.PlaySFX(AudioManager.instance.PlayerDMG[Random.Range(0, 
                    AudioManager.instance.PlayerDMG.Length)]);
            }
        }
        //check for death
        if (currentHealth <= 0)
        {
            //isDead = true;
            //Handle Death - message, respawn
            if (this.CompareTag("Player"))
            {
                Time.timeScale = 0;
                GameManager.instance.GetComponent<ButtonFunctions>().WinScreen.SetActive(true);
                //OnDeath?.Invoke();

                currentHealth = maxHealth;                      //Change where this is done i.e. Respawn?
            }
            else if (this.GetComponent<EnemyBase>() != null)
            {
                this.GetComponent<EnemyBase>().TakeDamage(damageAmt);
                AudioManager.instance.PlaySFX(AudioManager.instance.EnemyDMG[0]);
            }
            else
            {
                AudioManager.instance.PlaySFX(AudioManager.instance.EnemyDTH[0]);
                Destroy(this.gameObject);

                if (this.CompareTag("Boss"))
                {
                    OnDeath?.Invoke();

                    GameObject.FindWithTag("BossBar").SetActive(false);
                }
            }
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
        float fillAmount = (float)currentHealth / maxHealth;
        if (healthBarFill != null)
        {
            if (isHeal)
            {
                healthBarFill.fillAmount = Mathf.Lerp(healthBarFill.fillAmount, fillAmount, topFillSpeed * Time.deltaTime);
                //healthBarFill.DOFillAmount(fillAmount, topFillSpeed);

                //leave ease bar empty for enemies
                if (easeBar != null)                                           
                {
                    easeBar.color = Color.green;
                    easeBar.fillAmount = Mathf.Lerp(easeBar.fillAmount, fillAmount, bottomFillSpeed * Time.deltaTime);
                    //easeBar.DOFillAmount(fillAmount, bottomFillSpeed);
                }

            }
            else
            {
             healthBarFill.fillAmount = Mathf.Lerp(healthBarFill.fillAmount, fillAmount, bottomFillSpeed * Time.deltaTime);
             //healthBarFill.DOFillAmount(fillAmount, bottomFillSpeed);

                if (easeBar != null)
                {
                    easeBar.color = Color.red;
                    easeBar.fillAmount = Mathf.Lerp(easeBar.fillAmount, fillAmount, topFillSpeed * Time.deltaTime);
                    //easeBar.DOFillAmount(fillAmount, topFillSpeed);
                }
            }
            healthBarFill.color = healthGradient.Evaluate((float)currentHealth / maxHealth); 
        }
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

        if(critWarning != null)
        { 
            // Hide the warning text
            critWarning.gameObject.SetActive(false);
        }
    }
    void FlashCritWarning()
    {
        if(critWarning != null) 
        { 
            critWarning.gameObject.SetActive(!critWarning.gameObject.activeSelf);
        }
    }
    
    // Player Damage Screen //
    private void FlashDamageScreen()
    {
        PlayerScript.PlayerDamageScreen.GetComponent<Image>().enabled = true;
        Invoke("HideDamageScreen", dmgFlashDuration);
    }
    private void HideDamageScreen()
    {
       PlayerScript.PlayerDamageScreen.GetComponent<Image>().enabled = false; ;
    }
}
