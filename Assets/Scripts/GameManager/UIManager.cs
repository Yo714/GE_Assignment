using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text TipsMessage = null;

    private bool m_bShowTips = false;

    public float TwinklingSpeed = 0.5f;
    private float m_fTwinklingTime = 0.0f;

    public Image GunIcon = null;
    public Text GunName = null;
    public Text GunInfo = null;

    // Start is called before the first frame update
    void Start()
    {
        TipsMessage.enabled = false;
        EventCenter.GetInstance().Regist("InSight", OnInSight);
        EventCenter.GetInstance().Regist("OutSight", OnOutSight);
        EventCenter.GetInstance().Regist("EquipmentItem", OnEquipmentItem); ;
        EventCenter.GetInstance().Regist("SetBullet", OnSetBullet);
    }

    void OnSetBullet(object obj, int param1, int param2)
    {
        GunInfo.text = string.Format("{0}/{1}", param1, param2);
    }

    void OnEquipmentItem(object obj, int param1, int param2)
    {
        GunIcon.gameObject.SetActive(obj != null);
        GunData gd = ((GameObject)obj).GetComponent<Gun>().GunAttr;
        GunIcon.sprite = gd.ItemSprite;
        GunName.text = gd.ItemName;
    }

    void OnOutSight(object obj, int param1, int param2)
    {
        m_bShowTips = false;
        TipsMessage.enabled = false;
    }

    void OnInSight(object obj, int param1, int param2)
    {
        m_bShowTips = true;
        TipsMessage.enabled = true;
        if(((GameObject)obj).tag == "Gun")
        {
            TipsMessage.text = "Click F To Pick Up";
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(m_bShowTips)
        {
            m_fTwinklingTime += Time.deltaTime;
            if(m_fTwinklingTime > TwinklingSpeed)
            {
                TipsMessage.enabled = !TipsMessage.enabled;
                m_fTwinklingTime = 0;
            }
        }
    }
}
