using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Gun
{
    public int ShotBullet = 8;

    public float MaxShotRadio = 5;

    private bool m_bFire = false;

    private UIManager UIMgr;

    private void Start()
    {
        UIMgr = FindObjectOfType<UIManager>();

    }

    protected override void Reload()
    {
        if (GunAttr != null && !m_bReload)
        {
            if (BulletCount != GunAttr.CartridgeClip)
            {
                AnimatorControl.SetTrigger("ReloadLeft");
                Play(GunAttr.ReloadSoundLeft);

                m_bReload = true;
                StartCoroutine(PlayReload());
            }
        }

        IEnumerator PlayReload()
        {
            yield return new WaitForSeconds(0.3f);

            while (AnimatorIsPlaying("ReloadOpen")) {
                yield return null;
            }

            int nt = GunAttr.CartridgeClip - BulletCount;

            for (int i = 0; i < nt; ++i)
            {
                AnimatorControl.SetTrigger("ReloadInsert");
                Play(GunAttr.ReloadSoundIn);
                yield return new WaitForSeconds(0.3f);

                while (AnimatorIsPlaying("ReloadInsert")) {
                    yield return null;
                }
                BulletCount++;
                UIMgr.OnSetBullet(BulletCount, GunAttr.CartridgeClip);
            }

            AnimatorControl.SetTrigger("ReloadOut");
            Play(GunAttr.ReloadSoundOut);
            yield return new WaitForSeconds(0.3f);
            while (AnimatorIsPlaying("ReloadClose")) {
                yield return null;
            }

            m_bReload = false;
        }
    }

    protected override void CreateBullet(Vector3 dir)
    {
        for(int i = 0; i < ShotBullet; ++i)
        {
            Vector3 vPos = dir * GunAttr.EffectiveRange;
            vPos += MaxShotRadio * Random.insideUnitSphere;

            base.CreateBullet(vPos.normalized);
        } 
    }

    protected override void Fire()
    {
        FireTime = 0.0f;
        if (GunAttr != null && !m_bReload && !m_bFire)
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
            }
            else
            {
                Play(GunAttr.NoBulletSound);
            }
            m_bFire = true;
            UIMgr.OnSetBullet(BulletCount, GunAttr.CartridgeClip);
            StartCoroutine(PlayFire());
        }
    }

    IEnumerator PlayFire()
    {
        yield return new WaitForSeconds(0.3f);
        while (m_bFire)
        {
            m_bFire = AnimatorIsPlaying("Fire");
            yield return null;
        }
    }
}
