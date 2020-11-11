using System.Security.Cryptography;
using UnityEngine;

public class CharatersStats : MonoBehaviour
{
    public int health = 100;
    public int currentHealth { get; private set; }

    public Stat damage;

    private void Awake()
    {
        currentHealth = health;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log(transform.name + " takes " + damage + " Damage.");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        // Die in Some Way 
        // this meath is menat to be overwitern
        Debug.Log(transform.name + " Died.");
    }
}
