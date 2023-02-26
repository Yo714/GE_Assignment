using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMgr : MonoBehaviour
{
    public Transform AimPos;
    public PlayerMgr playerMgr;
    public UIManager UIMgr;
    public Camera EyesCamera = null;
    public Camera SceneCamera = null;

    private MoveControl moveControl;

    // Start is called before the first frame update
    void Start()
    {
        if (AimPos == null)
        {
            GameObject aimPos = new GameObject("AimPos");
            aimPos.transform.SetParent(EyesCamera.transform);
            aimPos.transform.localPosition = new Vector3(0f, -0.08557577f, 0.1786626f);
            aimPos.transform.localRotation = Quaternion.identity;
            AimPos = aimPos.transform;
        }

        UIMgr = FindObjectOfType<UIManager>();
        moveControl = FindObjectOfType<MoveControl>();
    }

    public void OnPickUpItem(object obj)
    {
        if (SceneCamera != null && EyesCamera != null)
        {
            SceneCamera.transform.SetParent(EyesCamera.transform);
        }

        while (AimPos.childCount > 0)
        {
            GameObject t = AimPos.GetChild(0).gameObject;
            if (t != null)
            {
                DestroyImmediate(t);
            }
        }

        GameObject item = (GameObject)obj;
        ItemAttr attr = item.GetComponent<ItemAttr>();
        if (attr != null)
        {
            GameObject equipitem = GameObject.Instantiate(attr.AttrData.ItemEquipmentPrefab, AimPos);

            Gun g = equipitem.GetComponent<Gun>();
            if (g != null)
            {
                g.Init((GunData)attr.AttrData, EyesCamera, SceneCamera, gameObject);
            }

            UIMgr.OnEquipmentItem(equipitem);
            moveControl.OnEquipmentItem(equipitem);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject != null && collision.gameObject.tag == "Heal")
        {
            if (playerMgr.health < 50)
            {
                playerMgr.health = playerMgr.health + 10;
            }
            Destroy(collision.gameObject);
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
