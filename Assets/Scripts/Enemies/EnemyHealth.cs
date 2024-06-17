using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    private int currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    public void Damaged(int damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0)
        {
            Die();
        }
    }

    public void Healed(int heal) 
    {
        currentHealth = (currentHealth + heal) > maxHealth? maxHealth : (currentHealth + heal);
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
