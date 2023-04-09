using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeController : MonoBehaviour
{

    private Rigidbody2D rb;

    [SerializeField] private int throwPower = 50;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();

        if (CharacterController.isFacingRight)
        {
            rb.AddForce(new Vector2(throwPower, 0));
        }
        else
        {
            rb.AddForce(new Vector2(throwPower * -1, 0));
        }
        

        Destroy(gameObject, 5);
    }

}
