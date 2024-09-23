using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTank : MonoBehaviour
{
    //Initial Health
    [SerializeField] private float health;
    //Max Health
    [SerializeField] private float maxHealth;
    //Floating Health Bar Object
    [SerializeField] FloatingHealthBar healthBar;
    //Bullet Script
    [SerializeField] Bullet damage;
    //Explosion Effect
    public GameObject Explosion;



    public float Health => health;
    public float MaxHealth => maxHealth;
    Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        healthBar = GetComponentInChildren<FloatingHealthBar>();
    }

    private void Start()
    {
        health = maxHealth;
        healthBar.UpdateHealthBar(health , maxHealth);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            if (bullet != null)
            {
                TakeDamage(bullet.damage);
                Destroy(collision.gameObject); // Destroy the bullet after it hits the enemy
            }
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        healthBar.UpdateHealthBar(health , maxHealth);
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Instantiate(Explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
