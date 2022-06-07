using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Script.Weapon
{
    public abstract class FireArms : MonoBehaviour,IWeapon
    {
        public ReCoil ReCoil_Script;
        public Transform MuzzlePiont;
        public Transform CasingPoint;
        public GameObject BulletPrefab;
        public GameObject BulletImpactPrefab;
        public Camera MainCamera;

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
        protected Vector3 TargetPoint;
        protected Vector3 mousePosition;

        protected abstract void Shooting();
        protected abstract void Reload();
        protected abstract void Aim();

        protected virtual void Start()
        {
            MainCamera = Camera.main;
            CurrentAmmo = AmmoInMag;
            CurrentMaxAmmoCarried = MaxAmmoCarried;
            GunAnimator = GetComponent<Animator>();
            OriginFOV = MainCamera.fieldOfView;
            ReCoil_Script = transform.GetComponent<ReCoil>();
        }

        protected virtual void GetTargetPoint()
        {
            Vector3 dir = MainCamera.transform.forward;
            RaycastHit hit;
            if (Physics.Raycast(MainCamera.transform.position, dir, out hit, 1000))
            {
                TargetPoint = hit.point;
            }
            Debug.DrawRay(MainCamera.transform.position, dir * 1000);
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