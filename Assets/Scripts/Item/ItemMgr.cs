using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMgr : MonoBehaviour
{
    public Transform ArmPos;
    public PlayerMgr playerMgr;
    public UIManager UIMgr;
    public Camera EyesCamera = null;
    public Camera SceneCamera = null;

    // Start is called before the first frame update
    void Start()
    {
        EventCenter.GetInstance().Regist("PickUpItem", OnPickUpItem);
    }

    void OnPickUpItem(object obj, int param1, int param2)
    {
        if(SceneCamera != null && EyesCamera != null)
        {
            SceneCamera.transform.SetParent(EyesCamera.transform);
        }
        while(ArmPos.childCount > 0)
        {
            GameObject t = ArmPos.GetChild(0).gameObject;
            if (t != null)
            {
                DestroyImmediate(t);
            }
        }
        GameObject item = (GameObject)obj;
        ItemAttr attr = item.GetComponent<ItemAttr>();
        if(attr != null)
        {
            GameObject equipitem = GameObject.Instantiate(attr.AttrData.ItemEquipmentPrefab, ArmPos);

            Gun g = equipitem.GetComponent<Gun>();
            if (g != null)
            {
                g.Init((GunData)attr.AttrData, EyesCamera, SceneCamera, gameObject);
            }

            EventCenter.GetInstance().Trigger("EquipmentItem", equipitem, 0, 0);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Heal")
        {
            playerMgr.health = playerMgr.health + 20;
            Destroy(collision.gameObject);
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
