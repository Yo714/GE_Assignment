using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemShow : MonoBehaviour
{
    public float RotateSpeed = 100;

    private bool m_bInSight = false;
    // Start is called before the first frame update
    void Start()
    {
    }

    public void OnOutSight(object obj)
    {
        if (gameObject == null) return;

        if ((GameObject)obj == gameObject)
        {
            m_bInSight = false;
        }
    }

    public void OnInSight(object obj)
    {
        m_bInSight = ((GameObject)obj == gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (m_bInSight)
        {
            return;
        }
        else
        {
            transform.Rotate(Vector3.up, Time.deltaTime * RotateSpeed);
        }
    }
}