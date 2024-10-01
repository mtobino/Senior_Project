using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    [SerializeField] Image healthBar;
    [SerializeField] public float healthAmount = 100f;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            takeDamage(5);
        }
        if(Input.GetKeyDown(KeyCode.L))
        {
            heal(5);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Projectile")
        {
            Debug.Log("Collided with: " + collision.gameObject);
            takeDamage(25);
        }
    }

    public void takeDamage(float damage)
    {
        healthAmount -= damage;
        healthBar.fillAmount = healthAmount / 100f;
    }

    public void heal(float healAmount)
    {
        healthAmount += healAmount;
        healthAmount = Mathf.Clamp(healthAmount, 0, 100);

        healthBar.fillAmount = healthAmount / 100f;
    }
}
