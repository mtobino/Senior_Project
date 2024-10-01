using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour, IHear
{
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;

    private bool IS_MOVING = true;
    private bool IS_SPRINTING;

    //enemy health
    public float health;

    //player health
    private HealthManager healthManager;

    // Patrolling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange = 50;

    // Attacking
    public float timeBetweenAttacks = 5;
    bool alreadyAttacked = false;
    public GameObject projectile;

    // States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    //hearing

    //getting player vars
    public Player playerClass;

    // Enemy Sounds
    [SerializeField] private AudioSource roar;
    [SerializeField] private AudioSource patroll;
    [SerializeField] private AudioSource chasing;
    private HashSet<AudioSource> sounds = new HashSet<AudioSource>();

    private enum SoundState 
    {
        CHASING, PATROLLING, ROARING
    }

    private SoundState soundState;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        healthManager = FindObjectOfType<HealthManager>();
        agent = GetComponent<NavMeshAgent>();
        soundState = SoundState.PATROLLING;
        sounds.Add(roar);
        sounds.Add(patroll);
        sounds.Add(chasing);
        if(!TryGetComponent(out agent))
        {
            Debug.LogWarning("EnemyAI does not have a NavMesh");
        }
    }

    private void Update()
    {
        // Checking if the player is in sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange) patrolling();
        if (playerInSightRange && !playerInAttackRange) chasePlayer();
        if (playerInSightRange && playerInAttackRange) attackPlayer();
        PlayAppropiateSound();
        
    }
    private void PlayAppropiateSound()
    {
        switch(soundState)
        {
            case SoundState.PATROLLING:
                PlaySoundHelper(patroll);
                break;
            case SoundState.CHASING:
                PlaySoundHelper(chasing);
                break;
            case SoundState.ROARING:
                PlaySoundHelper(roar);
                break;
            default:
                Debug.Log("Should not get hear :P");
                break;

        }
    }
    private void PlaySoundHelper(AudioSource audio)
    {
        if (audio.isPlaying)
        {
            return;
        }
        StopSoundsOtherThan(audio);
        audio.Play();
    }
    private void StopSoundsOtherThan(AudioSource audio)
    {
        foreach(AudioSource auidoSource in sounds)
        {
            if(auidoSource != audio)
            {
                auidoSource.Stop();
            }
        }
    }

    private void UpdateSoundState(SoundState soundState)
    {
        this.soundState= soundState;
    }

    public bool isMoving()
    {
        return IS_MOVING;
    }

    public bool isSprinting()
    {
        return IS_SPRINTING;
    }

    private void patrolling()
    {
        if (!walkPointSet)
        {
            searchWalkPoint();
        }
        if(walkPointSet)
        {
            agent.SetDestination(walkPoint);
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        walkPointSet = true;
        IS_MOVING = true;

        // If walkpoint reached...create new walkpoint
        if(distanceToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false;
        }
        UpdateSoundState(SoundState.PATROLLING);
    }

    private void searchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        float newZ = transform.position.z + randomZ;
        float newX = transform.position.x + randomX;

        // Clamping values to desired range to prevent walkPoint from forcing enemy over the edge
        newZ = Mathf.Clamp(newZ, 5f, 495f);
        newX = Mathf.Clamp(newX, 5f, 495f);

        walkPoint = new Vector3(newX, transform.position.y, newZ);

        //Debug.Log(walkPoint);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
        {
            walkPointSet = true;
        }
    }

    private void chasePlayer()
    {
        IS_MOVING = true;
        agent.SetDestination(player.position);
        UpdateSoundState(SoundState.CHASING);
    }

    private void attackPlayer()
    {
        // Enemy won't move
        IS_MOVING = false;
        agent.SetDestination(transform.position);
        agent.speed = 10;
        //Debug.Log("Enemy arrived at sound, new speed: " + agent.speed);

        transform.LookAt(player);

        if(!alreadyAttacked)
        {
            // ENEMY'S ATTACKING CODE GOES HERE (projectile for now)
            // If the random number is 0 or 1, the enemy will throw rocks for 5 seconds.
            // If the random number is between 2-9, the enemy will do "bonK" the player until they are out of health or until they are out of range
            int chooseAttack = UnityEngine.Random.Range(0, 10);

            if(chooseAttack >= 0 && chooseAttack <= 1)
            {
                Debug.Log("Enemy is doing rock projectile attack");
                Vector3 enemyPos = transform.position;
                enemyPos.x += 2;
                //enemyPos.z += 3;
                enemyPos.y += 2;

                Rigidbody rb = Instantiate(projectile, enemyPos, Quaternion.identity).GetComponent<Rigidbody>();
                rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
                rb.AddForce(transform.up * 2f, ForceMode.Impulse);

                alreadyAttacked = true;
                Invoke(nameof(resetAttack), 5);
            }
            else
            {
                Debug.Log("Enemy is doing close range attack");
                agent.destination = player.position; //get on top of the player's location
                healthManager.takeDamage(25);
                alreadyAttacked = true;
                Debug.Log("alreadyAttacked: " + alreadyAttacked);
                Invoke(nameof(resetAttack), 5);
            }
        }
        UpdateSoundState(SoundState.ROARING);
    }

    private void resetAttack()
    {
        Debug.Log("Rest Attack ran");
        alreadyAttacked = false;

    }

    private void takeDamage(int damage)
    {
        health -= damage;

        if (health <= 0) Invoke(nameof(destroyEnemy), 0.5f);
    }

    private void destroyEnemy()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange); //Attack range
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange); //Sight range
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, playerClass.soundRange); //Hearing range
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, walkPointRange); // possible walking range
    }

    //enemy listening method
    public void respondToSound(Sound sound)
    {

        print(sound.soundType);
        if(sound.soundType == Sound.SoundType.PlayerRunning)
        {
            moveTo(sound.pos);
            Debug.Log("Moving towards the running sound");
        }
        
        print(name + " responding to sound at " + sound.pos);
    }

    private void moveTo(Vector3 pos)
    {
        IS_MOVING = true;
        agent.speed = 20;
        Debug.Log("Enemy heard a sound, speeding up to: " + agent.speed);
        if (agent.hasPath)
        {
            // Disregard the current destination
            Debug.Log("Resetting enemy's destination to: " + pos);
            agent.ResetPath();
        }
        agent.SetDestination(pos);
        agent.isStopped = false;
    }
}
