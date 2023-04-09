using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateLightning : MonoBehaviour
{
    [SerializeField] private GameObject Lightning;

    [SerializeField] private AudioClip[] LightningSounds;

    [SerializeField] private Transform characterTransform;

    private float lightningWaitTime = 0.0f;

    

    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(createOneLightning());

        //Instantiate(Lightning, new Vector3(0, 14, 0), Quaternion.Euler(new Vector3(0, 0, Random.Range(-110, -70))));

    }

    public void startLightnings()
    {
        StartCoroutine(createOneLightning());
    }


    IEnumerator createOneLightning()
    {
        while (true)
        {
            lightningWaitTime = Random.Range(0.15f, 0.3f);

            yield return new WaitForSeconds(lightningWaitTime);

            //Instantiate(Lightning, new Vector3(characterTransform.position.x + 4, 12, 0), Quaternion.Euler(new Vector3(0, 0, Random.Range(-110, -70))));
            //Instantiate(Lightning, new Vector3(characterTransform.position.x + Random.Range(1.5f, 7.0f), 12, 0), Quaternion.Euler(new Vector3(0, 0, Random.Range(-80, -70))));


            if (Random.Range(0,2) == 0)
            {
                Instantiate(Lightning, new Vector3(characterTransform.position.x + Random.Range(1.5f, 7.0f), 14, 0), Quaternion.Euler(new Vector3(0, 0, Random.Range(-90, -70))));
            }
            else
            {
                Instantiate(Lightning, new Vector3(characterTransform.position.x + Random.Range(1.5f, 7.0f), 14, 0), Quaternion.Euler(new Vector3(0, 0, Random.Range(-90, -70))));
                Instantiate(Lightning, new Vector3(characterTransform.position.x + Random.Range(1.5f, 7.0f), 14, 0), Quaternion.Euler(new Vector3(0, 0, Random.Range(-90, -70))));
            }


            AudioSource newAudioSource = gameObject.AddComponent<AudioSource>();
            newAudioSource.PlayOneShot(LightningSounds[Random.Range(0, LightningSounds.Length)]);
            Destroy(newAudioSource, 4.0f);

            //lightningWaitTime = Random.Range(5.0f, 15.0f);

            //lightningWaitTime = Random.Range(1.0f, 3.0f);


            yield return new WaitForSeconds(0.25f);
            //yield return new WaitForSeconds(Random.Range(0.2f, 0.5f));
        }
        
    }
}
