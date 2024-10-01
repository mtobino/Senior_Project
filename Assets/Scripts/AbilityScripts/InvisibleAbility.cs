using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleAbility : Ability
{

    [SerializeField] EnemyAI enemyAI;
    // Constructor
    private void Awake()
    {
        cooldownLength = 30;
        abilityLength = 5;
    }

    private void Start()
    {
        enemyAI = FindObjectOfType<EnemyAI>();
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
        float defaultSightRange = enemyAI.sightRange;
        float defaultAttackRange = enemyAI.attackRange;
        enemyAI.sightRange = 0;
        enemyAI.attackRange = 0;
        Debug.Log("Player Activated Invisibility Ability");
        int counter = abilityLength;
        StartCoroutine(startCooldown());
        while (counter > 0)
        {
            yield return new WaitForSeconds(1);
            
            Debug.Log("Healed 10 HP");
            counter--;
        }
        Debug.Log("Ability Ended");
        enemyAI.sightRange = defaultSightRange;
        enemyAI.attackRange = defaultAttackRange;
    }


}
