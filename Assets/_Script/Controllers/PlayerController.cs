using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class PlayerController : MonoBehaviour
{
    [HideInInspector]
    public int health;
    public int healthPowerAmmount = 10;
    public int currentHealth { get; private set; }
    public bool isDie = false;
    public bool isDamaged = false;
    public HealthBar healthBar;
    public GameObject diePanel;
    public TextMeshProUGUI keyText;
    public GameObject hitEffect;
    public GameObject healthPowerUpEffect;
    public AudioClip death;

    private Animator animator;
    private AudioSource audioSource;

    #region Singleton 
    public static PlayerController instance;
    private void Awake()
    {
        instance = this;
        health = Upgrade.instance.GetGlobalHealth();
        currentHealth = health;
        healthBar.SetMaxHealth(health);
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        //healthBar = transform.GetChild(5).GetChild(0).GetComponent<HealthBar>();
    }

    public void Running(float runningValue)
    {
        animator.SetFloat("Running", runningValue);
    }
    public void Attcking(int noOfClicks)
    {
        if (noOfClicks == 1)
        {
            animator.SetBool("Attack 1", true);
        }
    }
    public void PlayAudio(AudioClip audioClip)
    {
        audioSource.clip = audioClip;
        if (!audioSource.isPlaying)
            audioSource.Play();
        audioSource.loop = true;
    }
    public void StopAudio()
    {
        audioSource.Stop();
        audioSource.loop = false;
        audioSource.clip = null;
    }
    public void HealthPowerUp()
    {
        if(currentHealth < health)
        {
            currentHealth += healthPowerAmmount;
            healthBar.SetHealth(currentHealth);
            GameObject insEffect=  Instantiate(healthPowerUpEffect, transform.position, Quaternion.identity);
            Destroy(insEffect, 0.5f);
            if (currentHealth >= health)
            {
                currentHealth = health;
                healthBar.SetHealth(currentHealth);
            }
        }
    }
    public void TakeDamage(int damage)
    {
        isDamaged = true;
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
        animator.SetTrigger("Damaged");
        GameObject theEffect = Instantiate(hitEffect, transform.position, Quaternion.identity);
        Debug.Log(transform.name + " takes " + damage + " Damage.");
        Destroy(theEffect, 0.5f);
        if (currentHealth <= 0)
        {
            StartCoroutine(Die());
        }
    }
    public IEnumerator Die()
    {
        animator.SetBool("Die", true);
        Debug.Log(transform.name + " Died.");
        isDie = true;
        PlayAudio(death);
        yield return new WaitForSeconds(2f);
        diePanel.SetActive(true);
        keyText.text = CoinSystem.instance.savedKeys.ToString();
    }
    public void RiviveWithKeys()
    {
        CoinSystem.instance.UseKeys(5);
        bool canUseCoin = CoinSystem.instance.haveKeys;
        if (canUseCoin)
        {
            currentHealth = health;
            healthBar.SetMaxHealth(health);
            if (isDie)
            {
                animator.SetBool("Die", false);
                animator.ResetTrigger("Damaged");
                if (Falling.instance.GetFalling())
                {
                    transform.position = SelectPlayer.instance.instantiatePoints[SelectPlayer.instance.instantiatePoint].transform.position;
                    Falling.instance.SetFalling(false);
                }
            }
            isDie = false;
            diePanel.SetActive(false);
        }
    }
    public void RiviveWithAds()
    {
        currentHealth = health;
        healthBar.SetMaxHealth(health);
        if (isDie)
        {
            animator.SetBool("Die", false);
            animator.ResetTrigger("Damaged");

            if (Falling.instance.GetFalling())
            {
                transform.position = SelectPlayer.instance.instantiatePoints[SelectPlayer.instance.instantiatePoint].transform.position;
                Falling.instance.SetFalling(false);
            }
        }
        isDie = false;
        diePanel.SetActive(false);
    }
    public void BackToMainMenu()
    {
        if (isDie)
        {
            SceneFader.instance.FadeTo("MainMenu");
            diePanel.SetActive(false);
        }
    }
    //public void LookAt(Transform target)
    //{
    //    transform.LookAt(target);
    //}
}
