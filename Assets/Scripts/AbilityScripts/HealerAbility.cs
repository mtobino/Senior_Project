using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealerAbility : Ability   
{
    [SerializeField] HealthManager healthManager;

    // Constructor 
    private void Awake()
    {
        cooldownLength = 30;
        abilityLength = 5;
    }

    private void Start()
    {
        healthManager = FindObjectOfType<HealthManager>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && !cooldownActive)
        {
            StartCoroutine(useAbility());
        }
    }

    public override IEnumerator useAbility()
    {
        Debug.Log("Player Used Healer Ability");
        int counter = abilityLength;
        StartCoroutine(startCooldown());
        while (counter > 0)
        {
            yield return new WaitForSeconds(1);
            healthManager.heal(5);
            Debug.Log("Healed 10 HP");
            counter--;
        }
    }

}
