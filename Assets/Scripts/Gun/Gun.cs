using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public Transform FirePos = null;
    public Transform CasingPos = null;
    public Transform CameraPos = null;

    public ParticleSystem FireParticle = null;
    
    [HideInInspector]
    public GunData GunAttr = null;

    protected Animator AnimatorControl = null;
    protected AudioSource AudioSource = null;
    protected int BulletCount = 0;

    protected float FireTime = 0.0f;

    protected bool m_bReload = false;

    protected bool m_bIsAiming = false;
    protected int m_nAimPos = 0;

    protected float m_EyesFov = 60;

    protected Camera Eyes = null;

    protected GameObject Owner;

    private LookControl lookcontrol;

    private UIManager UIMgr;

    void Awake()
    {
        AnimatorControl = GetComponent<Animator>();
        AudioSource = GetComponent<AudioSource>();
        lookcontrol = FindObjectOfType<LookControl>();
        UIMgr = FindObjectOfType<UIManager>();
    }

    protected void Play(AudioClip clip)
    {
        AudioSource.clip = clip;
        AudioSource.Play();
    }

    public void Init(GunData gd, Camera ca, Camera sca, GameObject owner)
    {
        GunAttr = gd;
        if(GunAttr != null)
        {
            SetBullet(GunAttr.CartridgeClip);
            Play(GunAttr.DrawSound);
            m_EyesFov = GunAttr.FOVForNormal;

            Eyes = ca;

            if(CameraPos != null && sca != null)
            {
                sca.transform.SetParent(CameraPos);
            }

            Owner = owner;
        }
    }

    public void SetBullet(int n)
    {
        if(GunAttr != null)
        {
            if (n > GunAttr.CartridgeClip)
            {
                n = GunAttr.CartridgeClip;
            }
            BulletCount = n;
            UIMgr.OnSetBullet(BulletCount, GunAttr.CartridgeClip);
        }
    }

    // Update is called once per frame
    void Update()
    {
        FireTime += Time.deltaTime;

        if (GunAttr != null)
        {
            if (GunAttr.FireAway)
            {
                if (Input.GetButton("Fire1"))
                {
                    if(FireTime >= 1.0f / GunAttr.RateOfFire)
                    {
                        Fire();
                    }
                }
            }
            else if (Input.GetButtonDown("Fire1"))
            {
                Fire();
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                Reload();
            }

            Aim_Update();
        }
    }

    private void Aim_Update()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            m_bIsAiming = true;
            AnimatorControl.SetFloat("AimPos", m_nAimPos);
            AnimatorControl.SetBool("IsAimIn", m_bIsAiming);
            m_EyesFov = GunAttr.FOVForAim;

        }
        else if (Input.GetButtonUp("Fire2"))
        {
            m_bIsAiming = false;
            AnimatorControl.SetBool("IsAimIn", m_bIsAiming);
            m_EyesFov = GunAttr.FOVForNormal;
        }

        if(Eyes != null)
        {
            Eyes.fieldOfView = Mathf.Lerp(Eyes.fieldOfView, m_EyesFov, Time.deltaTime * 4);
        }
    }

    protected virtual void Reload()
    {
        if(GunAttr != null && !m_bReload)
        {
            if(BulletCount != GunAttr.CartridgeClip)
            {
                if (BulletCount > 0)
                {
                    AnimatorControl.SetTrigger("ReloadLeft");
                    Play(GunAttr.ReloadSoundLeft);
                }
                else
                {
                    AnimatorControl.SetTrigger("ReloadOut");
                    Play(GunAttr.ReloadSoundOut);
                }
                m_bReload = true;
                StartCoroutine(PlayReload());
            }
        }
    }

    protected bool AnimatorIsPlaying(string name)
    {
        AnimatorStateInfo ainfo = AnimatorControl.GetCurrentAnimatorStateInfo(0);
        return ainfo.IsName(name) && ainfo.normalizedTime < 1.0f;
    }

    IEnumerator PlayReload()
    {
        yield return new WaitForSeconds(1.0f);
        while (m_bReload)
        {
            m_bReload = AnimatorIsPlaying("ReloadLeft") || AnimatorIsPlaying("ReloadOut");
            if (!m_bReload)
            {
                SetBullet(GunAttr.CartridgeClip);
            }
            yield return null;
        }
    }

    protected virtual void Fire()
    {
        FireTime = 0.0f;
        if(GunAttr != null && !m_bReload)
        {
            if (BulletCount > 0)
            {
                AnimatorControl.SetTrigger("Fire");
                if (FireParticle != null)
                {
                    FireParticle.Play();
                }
                CreateBullet(FirePos.forward);
                CreateBulletCasing();
                Play(GunAttr.FireSound);

                BulletCount -= 1;
                lookcontrol.FiringForTest();
            }
            else
            {
                Play(GunAttr.NoBulletSound);
            }
            UIMgr.OnSetBullet (BulletCount, GunAttr.CartridgeClip);
        }
    }

    protected void CreateBulletCasing()
    {
        if (GunAttr != null && GunAttr.CasingPrefab != null)
        {
            GameObject obj = GameObject.Instantiate(GunAttr.CasingPrefab, CasingPos);
            obj.transform.SetParent(null);

            obj.GetComponent<Rigidbody>().AddForce((CasingPos.up + CasingPos.right) * 100);

            Destroy(obj, 3.0f);
        }
    }

    protected virtual void CreateBullet(Vector3 dir)
    {
        if (GunAttr != null && GunAttr.BulletPrefab != null)
        {
            GameObject obj = GameObject.Instantiate(GunAttr.BulletPrefab, FirePos);
            obj.transform.SetParent(null);

            obj.GetComponent<Rigidbody>().AddForce(dir * GunAttr.BulletForce);

            obj.GetComponent<Bullet>().Init(Owner, GunAttr.EffectiveRange, GunAttr.Damage);

            Destroy(obj, 3.0f);
        }
    }
}
