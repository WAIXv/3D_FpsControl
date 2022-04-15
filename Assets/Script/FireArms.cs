using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Script.Weapon
{
    public abstract class FireArms : MonoBehaviour,IWeapon
    {
        public Transform MuzzlePiont;
        public Transform CasingPoint;
        public GameObject BulletPrefab;
        public GameObject BulletImpactPrefab;
        public Camera ViewCamera;

        public ParticleSystem MuzzleParticle;
        public ParticleSystem CasingParticle;

        public AudioSource FirearmsShootingAudioSource;
        public AudioSource FirearmsReloadAudioSource;
        public FirearmsAudioData FirearmsAudioData;
        public ImpactAudioData ImpactAudioData;
 
        public int AmmoInMag = 30;
        public int MaxAmmoCarried = 120;
        public float FireRate = 11.7f;
        public float TargetAimFOV = 26;

        protected int CurrentAmmo;
        protected int CurrentMaxAmmoCarried;
        protected float LastFireTime;
        protected Animator GunAnimator;
        protected AnimatorStateInfo GunStateInfo;
        protected float OriginFOV;
        protected bool isAiming;
        protected bool isLoading;

        protected abstract void Shooting();
        protected abstract void Reload();
        protected abstract void Aim();

        protected virtual void Start()
        {
            ViewCamera = Camera.main;
            CurrentAmmo = AmmoInMag;
            CurrentMaxAmmoCarried = MaxAmmoCarried;
            GunAnimator = GetComponent<Animator>();
            OriginFOV = ViewCamera.fieldOfView;
        }

        public void DoAttack()
        {
            Shooting();
        }

        protected bool isAllowShooting()
        {
           return Time.time - LastFireTime >= 1 / FireRate;
        }
    }
}