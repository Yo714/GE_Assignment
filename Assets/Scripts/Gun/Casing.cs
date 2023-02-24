using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Casing : MonoBehaviour
{
    public AudioClip[] ImpactAudios;

    private void OnCollisionEnter(Collision collision)
    {
        if (ImpactAudios.Length > 0)
        {
            AudioSource.PlayClipAtPoint(ImpactAudios[Random.Range(0, ImpactAudios.Length)], transform.position);
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
