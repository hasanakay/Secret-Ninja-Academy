using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamuraiController : MonoBehaviour
{
    private Animator animator;
    private Transform transform;
    private SpriteRenderer spriteRenderer;

    private bool deadControl = false;

    private bool isFacingRight = false;

    private bool walkControl = true;
    private bool runControl = false;
    private bool atackControl = true;

    [SerializeField] private GameObject atackZone;

    private CharacterController characterController;



    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;

    [SerializeField] private float maxDetermineCharacterDistance = 30f;

    private int determineDirection = -1; // 1 yada -1

    [SerializeField] private float leftX;  //0
    [SerializeField] private float rightX; //7

    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        transform = gameObject.GetComponent<Transform>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();


        characterController = FindObjectOfType<CharacterController>();


        InvokeRepeating("determineCharacter",0, 0.033f);

    }




    private void FixedUpdate()
    {
        if (deadControl == false)
        {
            if (walkControl)
            {
                walk();

            }
            else if (runControl)
            {
                run();
            }
            //determineCharacter();
        }

        
    }

    private void walk()
    {
        animator.SetBool("RunControl", false);
        animator.SetBool("WalkControl", true);

        if (isFacingRight)
        {
            determineDirection = 1;
        }
        else
        {
            determineDirection = -1;
        }

        if(transform.position.x <= leftX)
        {
            isFacingRight = true;
            spriteRenderer.flipX = !spriteRenderer.flipX;
            //Saða dön
        }
        else if (transform.position.x >= rightX)
        {
            isFacingRight = false;
            spriteRenderer.flipX = !spriteRenderer.flipX;
            //Sola dön
        }

        transform.position = new Vector2(transform.position.x + walkSpeed * determineDirection * Time.deltaTime, transform.position.y);
    }

    private void run()
    {
        animator.SetBool("WalkControl", false);
        animator.SetBool("RunControl", true);

        transform.position = new Vector2(transform.position.x + runSpeed * determineDirection * Time.deltaTime, transform.position.y);
    }

    private void determineCharacter()
    {
        //RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right * -1, maxDetermineCharacterDistance);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right * determineDirection, maxDetermineCharacterDistance);

        if(hit.collider != null)
        {
            //Debug.Log(hit.collider.gameObject.name);
            if(hit.collider.gameObject.name == "Character")
            {
                walkControl = false;
                runControl = true;

                //Debug.Log(hit.distance);
                if(hit.distance <= 0.25f)
                {
                    runControl = false;
                    animator.SetBool("RunControl", false);
                    Debug.Log("Saldýr");
                    atack();

                    takeDamageControl();
                }
                Debug.DrawLine(transform.position, hit.point, Color.red);

            }
        }
        //Debug.DrawLine(transform.position, hit.point, Color.red);

    }

    private void takeDamageControl()
    {
        if(characterController.atackControl == false || characterController.strikeControl == false)
        {
            Invoke("dead", 0.07f);
            //dead();
        }
    }

    private void atack()
    {
        if (atackControl)
        {
            atackControl = false;

            int randomNumber = Random.Range(0, 3);

            if(randomNumber == 0)
            {
                animator.SetTrigger("Atack1");
            }
            else if(randomNumber == 1)
            {
                animator.SetTrigger("Atack2");
            }
            else
            {
                animator.SetTrigger("Atack3");
            }

            Invoke("applyAttackDamage", 0.25f);

            Invoke("enableAtackAgain", 1.5f);
            
        }
    }

    private void applyAttackDamage()
    {
        atackZone.SetActive(true);
        Invoke("removeAttackDamage", 0.1f);

        if (deadControl == false)
        {
            characterController.samuraiAtacked();
        }

    }

    private void removeAttackDamage()
    {
        atackZone.SetActive(false);
    }

    private void enableAtackAgain()
    {
        atackControl = true;

        
    }

    private void dead()
    {
        deadControl = true;
        animator.SetTrigger("Samurai Dead");
        gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
        gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
        Destroy(gameObject, 3);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("NormalAtack") && !deadControl)
        {
            Debug.Log("NormalAtack");
            dead();

        }

        if (collision.gameObject.CompareTag("StrikeAtack") && !deadControl)
        {
            Debug.Log("StrikeAtack");
            dead();
        }

        if (collision.gameObject.CompareTag("KnifeAtack") && !deadControl)
        {
            Debug.Log("KnifeAtack");
            Destroy(collision.gameObject);
            dead();
        }
    }
}
