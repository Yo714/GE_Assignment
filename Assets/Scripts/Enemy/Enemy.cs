using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int startingHealth = 10;
    private int currentHealth;

    public int Damage;

    public GameObject healingPrefab;
    private EnemySpawner enemySpawner;
    private ScoreManager scoreMgr;

    public float dropRate = 0.1f; // 10% chance of dropping a healing item

    public Transform target;
    public float speed = 4f;
    Rigidbody rig;

    [SerializeField] float jumpHeight;

    Vector3 atkLoc;

    float attackState, atkTime, atkTimeMax, atkRecover = 0.5f, atkDelay = 0.5f;

    bool attacking = false, atkMode = false, jumpBack = false, jumpFlag = false, jumping = false;

    Transform[] paths;
    Transform highest;
    [SerializeField] Transform node, lastNode;

    [SerializeField] bool pathing = false;
    public float move_speed = 10f;
    public float rot_speed = 2f;
    GameObject pathParent;

    [SerializeField] bool nodeFlag = false;
    bool pathFlag = false, pathJumping = false;



    void Start()
    {
        currentHealth = startingHealth;
        enemySpawner = FindObjectOfType<EnemySpawner>();
        scoreMgr = FindObjectOfType<ScoreManager>();

        rig = GetComponent<Rigidbody>();


        pathParent = GameObject.Find("Paths");
        
        
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log(currentHealth);
        if (currentHealth <= 0)
        {
            // Enemy is dead, let the spawner know
            enemySpawner.OnEnemyDeath();

            // Enemy is dead, add score
            scoreMgr.AddScore();

            // Check if we should spawn a healing item
            if (Random.value < dropRate)
            {
                // Spawn healing item
                Instantiate(healingPrefab, transform.position, transform.rotation);
            }

            Destroy(gameObject);
        }
    }

    void Update()
    {
        //Debug.Log("attacking " + attacking);
        if(target == null)
        {
            target = GameObject.FindWithTag("Player").transform;
        }

        

        if(Vector3.Distance(target.position, transform.position) < 1 && jumpBack && !jumping)
        {
            jumping = true;
            jumpBack = false;
            Vector3 pos = Vector3.MoveTowards(transform.position, target.position, speed * Time.fixedDeltaTime);
            rig.MovePosition(pos);
            Vector3 actface = new Vector3(target.transform.position.x - transform.position.x, 0, target.transform.position.z - transform.position.z);
            transform.rotation = Quaternion.LookRotation(actface);


            Vector3 jumpLoc = -transform.forward * 10f;

            rig.AddForce(jumpLoc + new Vector3(0, jumpHeight, 0), ForceMode.Impulse);

            Invoke("CloseJump", 1.2f);
        }
        else
        {
            jumpBack = false;
        }

        if (atkMode && !jumping)
        {
            Physics.IgnoreLayerCollision(3, 7);
            atkTime -= Time.deltaTime;
            attackState = (atkTime / atkTimeMax) * attackState;
            rig.velocity = transform.forward * attackState;
            if (atkTime <= 1 && !jumpFlag)
            {
                jumpFlag = true;
                jumpBack = true;
            }
            if (atkTime <= atkRecover)
            {
                if(jumping == true)
                {
                    Invoke("JumpImmune", 1.5f);
                }
                else
                {
                    Physics.IgnoreLayerCollision(3, 7, false);
                }
                //rig.detectCollisions = true;
                attacking = false;
                atkMode = false;
                jumpFlag = false;
                //Physics.IgnoreCollision(target.GetComponent<Collider>(), GetComponent<Collider>(), false);
            }
        }
        if (target.position.y - transform.position.y > 6f || target.position.y - transform.position.y < -6f && !pathing)
        {
            Debug.Log("pp");
            pathing = true;
            if(highest == null)
            {
                foreach (Transform pathings in pathParent.transform)
                {
                    Debug.Log("pathings: " + pathings);
                    if (pathings.name.Contains("Pathing"))
                    {
                        float high = 0;
                        Vector3 pathtempDis = pathings.transform.position;
                        //pathtempDis.y = 0;
                        Vector3 transtempDis = transform.position;
                        //transtempDis.y = 0;
                        if(pathtempDis.y - transtempDis.y < -6 || pathtempDis.y - transtempDis.y > 6)
                        {
                            continue;
                        }
                        float temp = Vector3.Distance(pathtempDis, transtempDis);
                        if (temp > high)
                        {
                            high = temp;
                            highest = pathings;
                        }
                    }
                }
            }
            
            RePath(highest);
            //return;
        }

        if (pathing)
        {
            if (Vector3.Distance(node.position, transform.position) < 2f)
            {
                lastNode = node;
                
                if(pathFlag == true)
                {
                    pathFlag = false;
                    

                    if (!pathJumping)
                    {
                        RePath(highest);
                        pathJumping = true;

                        Debug.Log("jumping to node: " + node);
                        Vector3 pathJump = (node.position - transform.position);

                        rig.AddForce(pathJump + new Vector3(0, 6, 0), ForceMode.Impulse);

                        Invoke("PathJumpReset", 1f);
                    }
                    

                }
                
            }
            if(node.name.Contains("Start"))
            {
                Vector3 dir = node.position - this.transform.position;
                Quaternion lookatWP = Quaternion.LookRotation(dir);
                Debug.DrawRay(this.transform.position, dir, Color.red);
                this.transform.rotation = Quaternion.Slerp(this.transform.rotation, lookatWP, rot_speed * Time.deltaTime);
                this.transform.Translate(0f, 0f, move_speed * Time.deltaTime, Space.Self);
                return;
            }
        }

        float distance = Vector3.Distance(target.position, transform.position);
        if (distance > 3 && !attacking && !jumping) 
        {
            
            Vector3 pos = Vector3.MoveTowards(transform.position, target.position, speed * Time.fixedDeltaTime);
            rig.MovePosition(pos);
            Vector3 actface = new Vector3(target.transform.position.x - transform.position.x, 0, target.transform.position.z - transform.position.z);

            //Debug.Log(actface);
            Vector3 direction = Vector3.RotateTowards(Vector3.forward, actface, 4f, 0f);
            transform.rotation = Quaternion.LookRotation(direction);
            return;
        }
        else if (!attacking && distance <= 3 && !jumping)
        {
            Vector3 actface = new Vector3(target.transform.position.x - transform.position.x, 0, target.transform.position.z - transform.position.z);
            Vector3 direction = Vector3.RotateTowards(Vector3.forward, actface, 4f, 0f);
            transform.rotation = Quaternion.LookRotation(direction);
            attacking = true;
            atkLoc = target.transform.position;
            Invoke("Attack", atkDelay);
            
        }
        

    }

    void Attack()
    {
        atkMode = true;
        atkTime = 2;
        atkTimeMax = 2;
        attackState = 25;
    }

    void CloseJump()
    {
        jumping = false;
    }


    void JumpImmune()
    {
        Physics.IgnoreLayerCollision(3, 7, false);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            PlayerMgr playerMgr = other.gameObject.GetComponent<PlayerMgr>();
            if (playerMgr != null)
            {
                playerMgr.TakeDamage(Damage);
            }
        }
    }

    void RePath(Transform pathNode)
    {
        Debug.Log("called");
        foreach(Transform pathNext in pathNode)
        {
            Debug.Log("pathnext " + pathNext);
            if(lastNode== null)
            {
                lastNode = pathNext;
                pathFlag = true;
            }
            if (nodeFlag)
            {
                node = pathNext;
                Debug.Log("changela");
                pathFlag = true;
                nodeFlag = false;
                if (node.name.Contains("End"))
                {
                    Invoke("ResetLastNode", 2f);
                    pathing = false;
                    highest = null;
                }
                break;
            }
            if (lastNode == pathNext )
            {
                nodeFlag = true;
            }
            
        }
    }

    void PathJumpReset()
    {
        node = null;
        pathJumping= false;
    }

    void ResetLastNode()
    {
        lastNode = null;
    }

}