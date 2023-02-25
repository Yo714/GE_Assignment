using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explode : MonoBehaviour
{
    public AudioClip[] ExplodeAudios = null;
    public GameObject ExplodeEffect = null;

    public float ExplodeAreaRadio = 5.0f;
    public float ExplodeForce = 500.0f;

    public void Explosion()
    {
        Vector3 pos = transform.position;
        if(ExplodeAudios.Length > 0)
        {
            AudioSource.PlayClipAtPoint(ExplodeAudios[Random.Range(0, ExplodeAudios.Length)], pos);
        }

        if(ExplodeEffect != null)
        {
            GameObject obj = GameObject.Instantiate(ExplodeEffect, pos, transform.localRotation);

            Destroy(obj, 5.0f);
        }
    }
}
