using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer backgroundImage;
    [SerializeField] private SpriteRenderer backgroundSnowImage;

    [SerializeField] private CreateLightning createLightning;

    [SerializeField] private HearthBar healthBar;
    [SerializeField] private HearthBar powerBar;


    [SerializeField] private BoxCollider2D normalAtackCollider;
    [SerializeField] private BoxCollider2D strikeAtackCollider;

    [SerializeField] private GameObject knife;

    private float normalAtackPowerCount = 25.0f;
    private float strikeAtackPowerCount = 45.0f;
    private float knifeAtackPowerCount = 15.0f;

    private float horizontal;
    [SerializeField] private float speed = 8f;
    [SerializeField] private float jumpingPower = 16f;
    public static bool isFacingRight = true;

    private float atacksAgainEnableTime = 0.5f;
    private float atacksColliderDisableTime = 0.05f;
    private float atacksColliderEnableTime = 0.35f;

    private float maxHealthCount = 100.0f;
    private float maxPowerCount = 100.0f;
    private float healthCount;
    private float powerCount;
    

    [SerializeField] private bool GroundControl = true;
    [SerializeField] private bool JumpControl = false;
    [SerializeField] public bool atackControl = true;
    [SerializeField] public bool strikeControl = true;
    [SerializeField] private bool throwKnifeControl = true;

    private void Start()
    {
        normalAtackCollider.gameObject.SetActive(false);
        strikeAtackCollider.gameObject.SetActive(false);

        powerBar.SetMaxHealth(maxPowerCount);
        healthBar.SetMaxHealth(maxHealthCount);

        healthCount = maxHealthCount;
        powerCount = maxPowerCount;

        //decreaseHealth(60);
        //decreasePower(90);

        //InvokeRepeating("increaseHealth", 3, 0.1f);
        InvokeRepeating("increasePower", 0, 0.1f);

        //CancelInvoke("increasePowerandHealth");
    }

    void Update()
    {
        
    }


    private void increaseHealth()
    {
        if (healthCount < maxHealthCount)
        {
            healthCount++;
            healthBar.SetHealth(healthCount);
        }

        if (healthCount >= 100)
        {
            CancelInvoke("increaseHealth");
        }
    }

    private void increasePower()
    {

        if (powerCount < maxPowerCount)
        {
            powerCount++;
            powerBar.SetHealth(powerCount);
        }

        if (powerCount >= 100)
        {
            //CancelInvoke("increasePower");
        }
    }

    private void decreaseHealth(float decreaseHealthCount)
    {
        
        healthBar.SetHealth(healthCount - decreaseHealthCount);
        healthCount -= decreaseHealthCount;

        if(healthCount<= 0)
        {
            Debug.Log("Game Over");
        }
    }

    private void decreasePower(float decreasePowerCount)
    {
        
        powerBar.SetHealth(powerCount - decreasePowerCount);
        powerCount -= decreasePowerCount;

        if (powerCount <= 0)
        {
            powerCount = 0;
            Debug.Log("Saldýrý Yapamazsýn Gücün Bitti");
        }
    }


    private void FixedUpdate()
    {
        horizontal = Input.GetAxisRaw("Horizontal");


        run();
        jump();

        Flip();

        atack();
        strike();

        throwKnife();

        
    }

    public void samuraiAtacked()
    {
        CancelInvoke("increaseHealth");
        decreaseHealth(40.0f);
        InvokeRepeating("increaseHealth", 3, 0.1f);
    }

    private void run()
    {
        if (GroundControl)
        {
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(horizontal * speed * 0.5f, rb.velocity.y);
        }

        if (Mathf.Abs(horizontal * speed) > 0)
        {
            animator.SetBool("MovingControl", true);
        }
        else
        {
            animator.SetBool("MovingControl", false);
        }
    }



    private void jump()
    {
        if (Input.GetButton("Jump") && GroundControl)
        {
            rb.AddForce(new Vector2(0f, jumpingPower), ForceMode2D.Impulse);

            JumpControl = true;

            animator.SetBool("GroundedControl", GroundControl);
            animator.SetBool("JumpControl", JumpControl);


            GroundControl = false;

            Debug.Log("Boþluk Tuþuna basýldý");

        }

        if (JumpControl && GroundControl)
        {
            JumpControl = false;
            animator.SetBool("JumpControl", JumpControl);
        }
    }


    private void atack()
    {
        if (Input.GetButton("Fire1") && atackControl && throwKnifeControl && strikeControl)
        {
            Debug.Log("Fire1");

            animator.SetTrigger("AtackTriger");

            //normalAtackCollider.gameObject.SetActive(true);
            Invoke("normalAtacksolliderEnable", atacksColliderEnableTime);

            atackControl = false;
            //Invoke("normlaAtacksolliderDisable", atacksColliderDisableTime);
            Invoke("atackEnable", atacksAgainEnableTime);
            
        }
    }

    private void atackEnable()
    {
        atackControl = true;
    }

    private void normalAtacksolliderEnable()
    {
        normalAtackCollider.gameObject.SetActive(true);
        decreasePower(normalAtackPowerCount);
        Invoke("normalAtacksolliderDisable", atacksColliderDisableTime);
    }

    private void normalAtacksolliderDisable()
    {
        normalAtackCollider.gameObject.SetActive(false);
    }

    private void strike()
    {
        if (Input.GetButton("Fire2") && strikeControl && throwKnifeControl && atackControl)
        {
            Debug.Log("Fire2");

            animator.SetTrigger("StrikeTriger");

            //strikeAtackCollider.gameObject.SetActive(true);

            Invoke("strikeAtacksolliderEnable", atacksColliderEnableTime);

            strikeControl = false;
            //Invoke("strikeAtacksolliderDisable", atacksColliderDisableTime);
            Invoke("strikeEnable", atacksAgainEnableTime);
        }
    }

    private void strikeAtacksolliderEnable()
    {
        strikeAtackCollider.gameObject.SetActive(true);
        decreasePower(strikeAtackPowerCount);
        Invoke("strikeAtacksolliderDisable", atacksColliderDisableTime);
    }

    private void strikeAtacksolliderDisable()
    {
        strikeAtackCollider.gameObject.SetActive(false);
    }

    private void strikeEnable()
    {
        strikeControl = true;
    }

    private void throwKnife()
    {
        if (Input.GetButton("Fire3") && throwKnifeControl && strikeControl && atackControl)
        {
            Debug.Log("Fire3");

            if (isFacingRight)
            {
                Instantiate(knife, new Vector3(transform.position.x + 0.6f, transform.position.y - 0.25f, transform.position.z), Quaternion.Euler(0, 0, -90));
            }
            else
            {
                Instantiate(knife, new Vector3(transform.position.x - 0.6f, transform.position.y - 0.25f, transform.position.z), Quaternion.Euler(0, 0, 90));
            }

            decreasePower(knifeAtackPowerCount);

            throwKnifeControl = false;
            Invoke("throwKnifeEnable", atacksAgainEnableTime);

        }
    }

    private void throwKnifeEnable()
    {
        throwKnifeControl = true;
    }


    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;

            backgroundImage.flipX = !backgroundImage.flipX;
            backgroundSnowImage.flipX = !backgroundSnowImage.flipX;
        }
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject.tag);

        if (collision.gameObject.CompareTag("Ground"))
        {
            GroundControl = true;
            //Debug.Log("Zeminde");
        }

        GroundControl = true;


        
    }


    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            //GroundControl = true;
        }
    }


    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            //GroundControl = false;
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.gameObject.tag);

        if (other.gameObject.CompareTag("EnemyAtack"))
        {
            Debug.Log("Düþman Saldýrdý");
        }

        if (other.gameObject.CompareTag("Lighting"))
        {
            Debug.Log("Yýldýrýmlar Düþmeye Baþlýyor");
            createLightning.startLightnings();
        }
    }




}
