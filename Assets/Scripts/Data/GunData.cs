using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MyFPS Data/ Gun Data")]
public class GunData : ItemData
{
    public enum GunType
    {
        GT_Rifle,
        GT_HandGun,
        GT_ShotGun,
        GT_Sniper,
        GT_Launcher,
    }

    public GunType SubType;

    public int Damage;
    public int CartridgeClip;
    public float EffectiveRange;
    public bool FireAway;
    public float RateOfFire;
    public float BulletForce;
    public GameObject BulletPrefab;
    public GameObject CasingPrefab;
    public float FOVForAim;
    public float FOVForNormal = 65;
    public AudioClip FireSound;
    public AudioClip NoBulletSound;
    public AudioClip AimSound;
    public AudioClip ReloadSoundLeft;
    public AudioClip ReloadSoundOut;
    public AudioClip ReloadSoundIn;
    public AudioClip DrawSound;
}
