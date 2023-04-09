using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyLightning : MonoBehaviour
{

    void Start()
    {
        Destroy(gameObject, Random.Range(0.5f, 1.0f));
    }
}
