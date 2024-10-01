using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 
 * This abstract class will be the baseline for each new ability that is created.
 * Each ability will have its own definition of "useAbility"
 * Each ability will be able to utilize the cooldown method by simply
 * assigning the cooldownAmount its own unique value and calling the startCooldown method from here
 * Using this abstract class will make it much easier to add additional "abilities" in the future. 
 * 
 */
public abstract class Ability : MonoBehaviour
{    
    // Eeach ability will have a different length
    public int abilityLength;
    // Each ability will have a different cooldown length (depending how useful it is)
    public int cooldownLength; //duration
    public bool cooldownActive = false;

    public bool abilityInUse = false;

    private GameInput gameInput;

    private void Start()
    {
        gameInput = Player.instance.GetGameInput();
        gameInput.OnAbilityUse += GameInput_OnAbilityUse;
    }

    private void GameInput_OnAbilityUse(object sender, System.EventArgs e)
    {
        StartCoroutine(useAbility());
    }

    // Function defined here since it's pretty much a univseral method, each ability just needs to define their cooldown length.
    public IEnumerator startCooldown()
    {
        cooldownActive = true;
        int counter = cooldownLength;
        while (counter > 0)
        {
            yield return new WaitForSecondsRealtime(1);
            counter--;
            Debug.Log("Ability cooldown: " + counter);
        }
        cooldownActive = false;
    }

    // Similar function to startCooldown. However, this is used for the length of the individual ability, not the length of the cooldown.
    // Each ability will have their own unique definition of useAbility
    public abstract IEnumerator useAbility();

 

}
