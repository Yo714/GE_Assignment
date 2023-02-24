using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemShow: MonoBehaviour
{
    public float RotateSpeed = 100;

    private bool m_bInSight = false;
    // Start is called before the first frame update
    void Start()
    {
        EventCenter.GetInstance().Regist("InSight", OnInSight);
        EventCenter.GetInstance().Regist("OutSight", OnOutSight);
    }

    void OnOutSight(object obj, int param1, int param2)
    {
        if((GameObject)obj == gameObject){
            m_bInSight = false;
        }
    }

    void OnInSight(object obj, int param1, int param2)
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
