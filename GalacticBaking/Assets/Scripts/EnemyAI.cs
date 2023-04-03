using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    private PlayerInventory inventory;
    [SerializeField] public int maxHealth = 3;
    [SerializeField] private int currentHealth;
    public GameObject player;
    PlayerController controller;


    // AI var
    public NavMeshAgent agent;
    public Transform playerTransform;
    public LayerMask whatIsGround;
    public LayerMask whatIsPlayer;

    // Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    // Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;

    // States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    void Awake()
    {
        GameObject gameManager = GameObject.Find("GameManager");
        inventory = gameManager.GetComponent<PlayerInventory>();
        playerTransform = GameObject.Find("Chef").transform;
        agent = GetComponent<NavMeshAgent>();
        controller = player.GetComponent<PlayerController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        // Check for sight
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);

        if(!playerInSightRange && !playerInAttackRange)
        {
            Patrol();
        }
       
        if(playerInSightRange && !playerInAttackRange)
        {
            ChasePlayer();
        }
    }

        private void Patrol()
    {
        if(!walkPointSet) SearchWalkPoint();

        if(walkPointSet)
        agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        // Walkpoint reached
        if(distanceToWalkPoint.magnitude< 1f)
        walkPointSet = false;
    }

    private void SearchWalkPoint()
    {
        // Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if(Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
        walkPointSet = true;
    }

    private void ChasePlayer()
    {
        agent.SetDestination(playerTransform.position);
        transform.LookAt(playerTransform);
    }

    public void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            controller.PlayerTakeDamage(20);
        }
    }

    public void EnemyTakeDamage(int damage)
    {
        currentHealth -= damage;

        if(currentHealth <= 0)
        {
            Destroy(gameObject);
            inventory.AddCoins(5);
            Debug.Log("Enemy Destroyed");
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
