using System.Collections;

using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public float lookRadius = 5f;
    public int health = 100;
    public int currentHealth { get; private set; }
    public HealthBar healthBar;
    [HideInInspector]
    public int damage = 1;
    public float attackspeed = 1f;
    public bool isDied = false;
    public GameObject dieEffect;
    public GameObject coinPrefab;
    public float timeForNewPath;
    public float walkspeed = 5.0f;

    private bool inCoroutine;
    private Vector3 targetWalk;
    private NavMeshPath path;
    private bool validPath;

    private float attackcooldown = 0f;
    private Transform target;
    private NavMeshAgent agent;
    private Animator animator;
    private bool isAttacking;
    private Rigidbody rigidbody;

    public bool looking;
    public Transform lookTarget;

    public void SetLooking(bool seting)
    {
        looking = seting;
    }

    public bool GetLooking()
    {
        return looking;
    }

    #region Singleton
    public static EnemyController instance;
    private void Awake()
    {

        instance = this;
        currentHealth = health;
        healthBar.SetMaxHealth(health);
        agent = GetComponent<NavMeshAgent>();
        path = new NavMeshPath();
    }
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        target = SelectPlayer.instance.player.transform;
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();
        isAttacking = false;
        looking = false;
        if (lookTarget == null)
            lookTarget = transform.GetChild(0).transform;
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(target.position, transform.position);
        attackcooldown -= Time.deltaTime;
        if (distance <= lookRadius)
        {
            agent.SetDestination(target.position);
            animator.SetBool("Walk", true);
            if (distance <= agent.stoppingDistance)
            {
             
                // Attack the target
                if (!isDied && !PlayerController.instance.isDie)
                {
                    isAttacking = true;
                    Attack();
                    // face the target
                    animator.SetBool("Attack", true);
                    if (!looking)
                    {
                        //target.GetComponent<PlayerController>().LookAt(transform);
                        EnemyController.instance.SetLooking(true);
                    }
                }
                else
                {
                    animator.SetBool("Attack", false);
                }
                FaceTarget();
            }
        }
        else
        {
            EnemyController.instance.SetLooking(false);
            animator.SetBool("Attack", false);
            isAttacking = false;
            if (!isAttacking && transform.tag == "Enemy")
            {
                if (!inCoroutine)
                {
                    StartCoroutine(DoSomething());
                    animator.SetBool("Walk", false);
                    //Debug.LogWarning("Walking " + transform.name);
                }
                else
                {
                    animator.SetBool("Walk", true);
                }
            }
        }
    }
    public void PowerUpEneimes()
    {
        damage++;
    }

    private void Attack()
    {
        if (attackcooldown <= 0)
        {
            target.GetComponent<PlayerController>().TakeDamage(damage);
            attackcooldown = 1f / attackspeed;
        }
    }
    void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        animator.SetTrigger("Damaged");
        healthBar.SetHealth(currentHealth);
        Debug.Log(transform.name + " takes " + damage + " Damage.");

        if (currentHealth < 0)
        {
            isDied = true;
            SpwanSystem.instance.SetCountDeadEnemies(1);
            StartCoroutine(Die());
            animator.SetBool("Die", true);
        }
    }
    public IEnumerator Die()
    {
        EnemyController.instance.SetLooking(false);
        GameObject insEffect = Instantiate(dieEffect, transform.position, Quaternion.identity);
        Destroy(insEffect, 0.5f);
        Instantiate(coinPrefab, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
        Debug.Log(transform.name + " Died");
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }

    IEnumerator DoSomething()
    {
        inCoroutine = true;

        yield return new WaitForSeconds(timeForNewPath);
        GetNewPath();
        if (!isDied)
            validPath = agent.CalculatePath(targetWalk, path);
        if (!validPath)
            Debug.Log("Found an invaild Path");

        while (!validPath)
        {
            yield return new WaitForSeconds(0.01f);
            GetNewPath();
            if (!isDied)
                validPath = agent.CalculatePath(targetWalk, path);
        }

        inCoroutine = false;
    }
    void GetNewPath()
    {
        //targetWalk = GetRandomPostion();
        targetWalk = SpwanSystem.instance.Levels[SpwanSystem.instance.levelCount].SpwanPoints[Random.Range(0, SpwanSystem.instance.Levels[SpwanSystem.instance.levelCount].SpwanPoints.Count)].transform.position;
        if (!isDied)
            agent.SetDestination(targetWalk);
    }
    private void OnCollisionEnter(Collision collision)
    {
        //print(collision.collider.name + gameObject.name);
        if (!isAttacking)
        {
            animator.SetBool("Walk", false);
            GetNewPath();
            if (!isDied)
                validPath = agent.CalculatePath(targetWalk, path);
        }
    }
}
