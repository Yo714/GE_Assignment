using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : Gun
{
    public GameObject Bullet = null;
    protected override void CreateBullet(Vector3 dir)
    {
        base.CreateBullet(dir);
        if(Bullet != null)
        {
            Bullet.SetActive(false);
        }
    }

    protected override void Reload()
    {
        if (GunAttr != null && !m_bReload)
        {
            AnimatorControl.SetTrigger("ReloadLeft");
            Play(GunAttr.ReloadSoundLeft);

            m_bReload = true;
            StartCoroutine(PlayReload());
        }
    }

    IEnumerator PlayReload()
    {
        yield return new WaitForSeconds(0.6f);

        if (Bullet != null)
        {
            Bullet.SetActive(true);
        }

        yield return new WaitForSeconds(1.6f);

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

}
