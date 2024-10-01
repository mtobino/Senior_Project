using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements.Experimental;

public class Level2EnemyAI : MonoBehaviour
{
    //Get player's health
    private HealthManager healthManager;

    public Transform playerLocation;
    public NavMeshAgent agent;

    public float randomPatrolPointRange = 2;
    public Transform centerPoint;

    public float sightRange = 10;
    public float attackRange = 5;

    public LayerMask whatIsPlayer;
    public bool playerInSightRange, playerInAttackRange;

    bool alreadyAttacked = false;

    public GameObject projectile;
    private enum SoundState
    {
        CHASING, PATROLLING, ROARING
    }

    private SoundState soundState;
    // Enemy Sounds
    [SerializeField] private AudioSource roar;
    [SerializeField] private AudioSource patroll;
    [SerializeField] private AudioSource chasing;
    private HashSet<AudioSource> sounds = new HashSet<AudioSource>();

    public bool isMoving { get; private set; }
    private void Start()
    {
        soundState = SoundState.PATROLLING;
        agent = GetComponent<NavMeshAgent>();
        //playerLocation = FindObjectOfType<Player>().transform;
        centerPoint = agent.transform;
        healthManager = FindObjectOfType<HealthManager>();
    }

    private void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        if(playerInSightRange)
        {
            chasePlayer();
            UpdateSoundState(SoundState.CHASING);
            isMoving= true;
        }
        if (playerInSightRange && playerInAttackRange)
        {
            attackPlayer();
            UpdateSoundState(SoundState.ROARING);
            isMoving= false;
        }

        if (agent.remainingDistance <= agent.stoppingDistance) //done w/ path
        {
            Vector3 point;
            if(randomPoint(centerPoint.position, randomPatrolPointRange, out point)) //pass in center pt and radius
            {
                Debug.DrawRay(point, Vector3.up, Color.red, 1.0f); // you can see the point he travels to
                agent.SetDestination(point);
                isMoving = true;
                UpdateSoundState(SoundState.PATROLLING);
            }
        }
        PlayAppropiateSound();
    }

    private void chasePlayer()
    {
        Vector3 targetLocation;
        targetLocation.x = playerLocation.position.x;
        targetLocation.y = playerLocation.position.y;
        targetLocation.z = playerLocation.position.z;

        targetLocation.x += 1;

        agent.destination = targetLocation;
    }

    bool randomPoint(Vector3 center, float range, out Vector3 result)
    {
        Vector3 randomPoint = center + Random.insideUnitSphere * range; //random point in a sphere 
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas)) 
        {
            //the 1.0f is the max distance from the random point to a point on the navmesh, might want to increase if range is big
            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        return false;
    }

    private void attackPlayer()
    {
        agent.SetDestination(transform.position);

        transform.LookAt(playerLocation);

        if (!alreadyAttacked)
        {
            // ENEMY'S ATTACKING CODE GOES HERE (projectile for now)

            //Debug.Log("Enemy is attacking you with rocks");
            Vector3 enemyPos = transform.position;
            //enemyPos.x += 2;
            //enemyPos.z += 3;
            //enemyPos.y = 2;

            Rigidbody rb = Instantiate(projectile, enemyPos, Quaternion.identity).GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
            rb.AddForce(transform.up * 2f, ForceMode.Impulse);

            alreadyAttacked = true;
            Invoke(nameof(resetAttack), 1);
        }
    }

    private void resetAttack()
    {
        alreadyAttacked = false;

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange); //Attack range
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange); //Sight range

    }

    private IEnumerator chaseTimer()
    {

        yield return new WaitForSeconds(5);
    }
    private void PlayAppropiateSound()
    {
        switch (soundState)
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
        foreach (AudioSource auidoSource in sounds)
        {
            if (auidoSource != audio)
            {
                auidoSource.Stop();
            }
        }
    }

    private void UpdateSoundState(SoundState soundState)
    {
        this.soundState = soundState;
    }
}
