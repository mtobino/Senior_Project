using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedAbility : Ability
{
    // fastSpeed is the speed at which the player will move when their ability is activated
    [SerializeField] private float fastSpeed = 5f;

    public float defaultSpeed;

    // Constructor 
    private void Awake()
    {
        cooldownLength = 30;
        abilityLength = 5; //duration
    }

    private void Start()
    {
        defaultSpeed = Player.instance.moveSpeed;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q) && !cooldownActive)
        {
            StartCoroutine(useAbility());
        }
    }

    /*
     * Overrides the startAbilityLength method to control the duration of an ability.
     * This method adjusts the player's movement speed temporarily while the ability is active.
     *
     * @return An IEnumerator coroutine used to control the duration of the ability.
     */
    public override IEnumerator useAbility()
    {
        Debug.Log("Player Activated Speed Ability");
        Player.instance.moveSpeed = fastSpeed;
        int counter = abilityLength;
        StartCoroutine(startCooldown());
        while (counter > 0)
        {
            yield return new WaitForSeconds(1);
            counter--;
        }
        Player.instance.moveSpeed = defaultSpeed;
        Debug.Log("Speed Ability Ended");
    }
    

}
