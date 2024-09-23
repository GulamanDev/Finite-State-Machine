using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyTankController : AdvancedFSM
{
    // Movement variables
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private float rotateSpeed = 10.0f;
    // How "close" to the player is considered as chasing range
    [SerializeField] private float chaseDistance = 5.0f;
    [SerializeField] private float attackDistance = 3.0f;
    // How "close" from the target waypoint until we choose another waypoint
    [SerializeField] private float waypointDistance = 1.0f;
    // The array of transform points in the scene that the agent will move
    // towards to while patrolling
    [SerializeField] private Transform[] waypoints;
    // Reference to the player tank
    [SerializeField] private Transform player;
    //Enemy Tank Firing Rate
    [SerializeField] private float fireRate = 1.0f;
    [SerializeField] private Transform turret;

    private float nextFireTime = 0.0f;
    private Rigidbody rb;
    private Transform bulletSpawnPoint;   

    public GameObject bullet;
    public float MoveSpeed => moveSpeed;
    public float RotateSpeed => rotateSpeed;
    public float ChaseDistance => chaseDistance;
    public float AttackDistance => attackDistance;
    public float WaypointDistance => waypointDistance;
   
    
    protected override void Initialize()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody component is missing on the enemy tank.");
        }

        fsmStates = new();
        //Construct the FSM
        PatrolState patrol = new PatrolState(waypoints);
        patrol.AddTransition(TransitionID.SawPlayer, StateID.Chase);
        ChaseState chase = new ChaseState();
        chase.AddTransition(TransitionID.LostPlayer, StateID.Patrol);
        chase.AddTransition(TransitionID.ReachPlayer, StateID.Attack);
        AttackState attack = new AttackState();
        attack.AddTransition(TransitionID.LostPlayer, StateID.Patrol);
        attack.AddTransition(TransitionID.SawPlayer, StateID.Chase);


        // First element is the default
        AddState(patrol);
        AddState(chase);
        AddState(attack);
    }

    protected override void FSMUpdate()
    {
       CurrentState.RunState(player, this.transform);
       CurrentState.CheckTransitionRules(player, this.transform);
    }

    public void MoveToTarget(Transform currentTarget)
    {
        // Get the vector pointing towards the direction of the target
        Vector3 targetDirection = currentTarget.position - transform.position;
        // Get the roatation that faces the targetDirection
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        // Rotate the tank to face the targetRotation
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 
            Time.deltaTime * rotateSpeed);
        // Tank is already rotate, simply move forward
        transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed);
    }

    public void StopMovement()
    {
        if (rb != null)
        {
            moveSpeed = 0f;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
        else
        {
            Debug.LogError("Rigidbody component is missing on the enemy tank.");
        }
    }

    public void RotateTurret(Transform target)
    {
        if (turret != null)
        {
            Vector3 direction = target.position - turret.position;
            direction.y = 0; // Ignore y-axis to constrain rotation to z-axis

            Quaternion rotation = Quaternion.LookRotation(direction);
            Quaternion targetRotation = Quaternion.Euler(0, rotation.eulerAngles.y, 0); // Only rotate around the y-axis

            turret.rotation = Quaternion.Slerp(turret.rotation, targetRotation, Time.deltaTime * rotateSpeed);
        }
        else
        {
            Debug.LogError("Turret Transform is missing.");
        }
    }

    public void FireAtPlayer()
    {
        if (Time.time > nextFireTime)
        {
            //Get the turret of the tank
            turret = gameObject.transform.GetChild(0).transform;
            bulletSpawnPoint = turret.GetChild(0).transform;

            // Calculate the direction to the player
            Vector3 directionToPlayer = (player.position - turret.position).normalized;

            // Check if the turret is facing the player
            float dotProduct = Vector3.Dot(turret.forward, directionToPlayer);
    
            
            if (dotProduct > 0.95f) // Adjust the threshold as needed
            {
                Debug.Log("Firing at player!");
                Instantiate(bullet, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            }
            
            nextFireTime = Time.time + fireRate;
        }
    }

    public void ResumeMovement()
    {
        if (rb != null)
        {
            moveSpeed = 5f; // Reset to default move speed or any desired value
        }
        else
        {
            Debug.LogError("Rigidbody component is missing on the enemy tank.");
        }
    }

}
