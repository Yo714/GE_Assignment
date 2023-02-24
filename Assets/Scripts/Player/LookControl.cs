using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookControl : MonoBehaviour
{
    public float MouseSenstivity = 300;
    public float MaxAngle = 80;
    public float MinAngle = -80;
    public Transform PlayerTransform;

    private float viewupdown = 0.0f;

    public float PickUpDistance = 2.0f;
    public LayerMask PickUpMack;

    private GameObject m_bLastTarget = null;

    public AnimationCurve RecoilCurve;
    private Vector2 Recoil;
    public float RecoilFadeOutTime = 0.3f;

    private float currentRecoilTime;
    private Vector2 currentRecoil;

    private UIManager UIMgr;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        Look_Update();
        LookItem_Update();

        PickUp_Update();
    }

    private void PickUp_Update()
    {
        if(Input.GetKeyDown(KeyCode.F) && m_bLastTarget != null)
        {
            if (m_bLastTarget.tag == "Gun")
            {
                EventCenter.GetInstance().Trigger("PickUpItem", m_bLastTarget, 0, 0);
            }
        }
    }

    private void LookItem_Update()
    {
        RaycastHit hit;
        //check item in the player view or not
        if (Physics.Raycast(transform.position, transform.forward, out hit, PickUpDistance, PickUpMack))
        {
            if(m_bLastTarget != hit.collider.gameObject)
            {
                m_bLastTarget = hit.collider.gameObject;
                EventCenter.GetInstance().Trigger("InSight", m_bLastTarget, 0, 0);
            }
        }
        else
        {
            if (m_bLastTarget != null)
            {
                EventCenter.GetInstance().Trigger("OutSight", m_bLastTarget, 0, 0);
                m_bLastTarget = null;
            }
        }
    }

    private void Look_Update()
    {
        float MouseX = Input.GetAxis("Mouse X") * Time.deltaTime * MouseSenstivity;
        float MouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * MouseSenstivity;

        viewupdown -= MouseY;

        viewupdown = Mathf.Clamp(viewupdown, MinAngle, MaxAngle);

        CalculateRecoilOffest();

        viewupdown -= currentRecoil.y;
        MouseX += currentRecoil.x;

        transform.localRotation = Quaternion.Euler(viewupdown, 0, 0);

        if (PlayerTransform != null)
        {
            PlayerTransform.Rotate(PlayerTransform.up, MouseX);
        }
    }

    private void CalculateRecoilOffest()
    {
        currentRecoilTime += Time.deltaTime;
        Recoil.x = Random.Range(-0.1f, 0.1f);
        Recoil.y = Random.Range(-0.1f, 0.2f);
        float tmp_RecoilFraction = currentRecoilTime / RecoilFadeOutTime;
        float tmp_RecoilValue = RecoilCurve.Evaluate(tmp_RecoilFraction);
        currentRecoil = Vector2.Lerp(Vector2.zero, currentRecoil, tmp_RecoilValue);
    }

    public void FiringForTest()
    {
        currentRecoil += Recoil;
        currentRecoilTime = 0;
    }
}
