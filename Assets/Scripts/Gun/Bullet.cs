using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ImpactEffect
{
    public string tag;
    public GameObject Effect;
}

public class Bullet : MonoBehaviour
{
    public AudioClip[] ImpactAudios;

    public ImpactEffect[] EffectList;

    private float m_MaxDistance = 0.0f;

    private GameObject m_Owner = null;

    private Vector3 m_StartPos;

    private int Damage;

    public GameObject enemy;

    public void Init(GameObject owner, float maxdistance, int damage)
    {
        m_Owner = owner;
        m_MaxDistance = maxdistance;
        Damage = damage;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (ImpactAudios.Length > 0)
        {
            AudioSource.PlayClipAtPoint(ImpactAudios[Random.Range(0, ImpactAudios.Length)], transform.position);
        }

        string tag = collision.gameObject.tag;

        foreach (ImpactEffect ie in EffectList)
        {
            if (ie.tag == tag)
            {
                GameObject obj = GameObject.Instantiate(ie.Effect, transform.position,
                    Quaternion.LookRotation(collision.contacts[0].normal));

                Destroy(obj, 5.0f);
            }
        }
        gameObject.SetActive(false);

        if (collision.gameObject.tag.Equals("Enemy"))
        {
            Enemy enemyMgr = collision.gameObject.GetComponent<Enemy>();
            if (enemyMgr != null)
            {
                enemyMgr.TakeDamage(Damage);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        m_StartPos = transform.position;
        enemy = GameObject.FindGameObjectWithTag("Enemy");
    }

    // Update is called once per frame
    void Update()
    {
        if((transform.position - m_StartPos).magnitude > m_MaxDistance)
        {
            gameObject.SetActive(false);
        }
    }
}
