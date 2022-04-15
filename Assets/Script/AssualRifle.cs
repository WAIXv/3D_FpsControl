using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Script.Weapon
{
    public class AssualRifle : FireArms
    {
        private IEnumerator reloadAnimCheckCoroutine;
        private IEnumerator doAimCoroutine;

        protected override void Start()
        {
            base.Start();
            reloadAnimCheckCoroutine = CheckReloadAnimationEnd();
            doAimCoroutine = DoAim();
        }

        private void LateUpdate()
        {
            if (Input.GetMouseButton(0))
            {
                DoAttack();
            }
            if(Input.GetKeyDown(KeyCode.R))
            {
                Reload();
            }
            if(Input.GetMouseButton(1)&&!isLoading)
            {
                //Aim in
                isAiming = true;
                Aim();
            }
            if(Input.GetMouseButtonUp(1))
            {
                //Aim out
                isAiming = false;
                Aim();
            }

            test();
        }

        protected override void Aim()
        {
            if(doAimCoroutine == null)
            {
                doAimCoroutine = DoAim();
                StartCoroutine(doAimCoroutine);
            }
            else
            {
                StopCoroutine(doAimCoroutine);
                doAimCoroutine = null;
                doAimCoroutine = DoAim();
                StartCoroutine(doAimCoroutine);      
            }
            GunAnimator.SetBool("Aim",isAiming);
        }

        protected override void Reload()
        {
            GunAnimator.SetLayerWeight(2, 1);
            GunAnimator.SetTrigger(CurrentAmmo > 0 ? "ReloadLeft" : "ReloadOutOf");

            FirearmsReloadAudioSource.clip = 
                CurrentAmmo > 0 
                ? FirearmsAudioData.ReloadLeft 
                : FirearmsAudioData.ReloadOutof;
            FirearmsReloadAudioSource.Play();
            
            if(reloadAnimCheckCoroutine == null)
            {
                reloadAnimCheckCoroutine = CheckReloadAnimationEnd();
                StartCoroutine(reloadAnimCheckCoroutine);
            }
            else
            {
                StopCoroutine(reloadAnimCheckCoroutine);
                reloadAnimCheckCoroutine = null;
                reloadAnimCheckCoroutine = CheckReloadAnimationEnd();
                StartCoroutine(reloadAnimCheckCoroutine);
            }
        }

        protected override void Shooting()
        {
            if (CurrentAmmo <= 0) return;
            if (!isAllowShooting()) return;
            if (isLoading) return;
            MuzzleParticle.Play();
            CurrentAmmo -= 1;

            GunAnimator.Play("Fire", isAiming ? 1 : 0, 0);
            FirearmsShootingAudioSource.clip = FirearmsAudioData.ShootingAudio;
            FirearmsShootingAudioSource.Play();

            CreateBullet();
            CasingParticle.Play();
            LastFireTime = Time.time;
        }
        protected void CreateBullet()
        {
            GameObject tmp_Bullet = Instantiate(BulletPrefab, MuzzlePiont.position, MuzzlePiont.rotation);
            var tmp_BulletScript = tmp_Bullet.GetComponent<Bullet>();
            tmp_BulletScript.ImpactPrefab = BulletImpactPrefab;
            tmp_BulletScript.ImpactAudioData = ImpactAudioData;
            tmp_BulletScript.BulletSpeed = 200f;
        }

        private IEnumerator CheckReloadAnimationEnd()
        {
            while (true)
            {
                yield return null;
                isLoading = true;
                GunStateInfo = GunAnimator.GetCurrentAnimatorStateInfo(2);
                if (GunStateInfo.IsTag("ReloadAmmo"))
                {
                    if (GunStateInfo.normalizedTime >= 0.9f)
                    {
                        int tmp_NeedAmmoCount = AmmoInMag - CurrentAmmo;
                        int tmp_RemainAmmo = CurrentMaxAmmoCarried - tmp_NeedAmmoCount;
                        isLoading = false;
                        if(tmp_RemainAmmo <= 0)
                        {
                            CurrentAmmo += CurrentMaxAmmoCarried;
                        }
                        else
                        {
                            CurrentAmmo = AmmoInMag;
                        }
                        CurrentMaxAmmoCarried = tmp_RemainAmmo <= 0 ? 0 : tmp_RemainAmmo;
                        yield break;
                    }
                }
            }
        }
        private IEnumerator DoAim()
        {
            while(true)
            {
                yield return null;
                float tmp_CurrentFOV = 0;
                ViewCamera.fieldOfView = 
                    Mathf.SmoothDamp(ViewCamera.fieldOfView, 
                    isAiming ? TargetAimFOV : OriginFOV,
                    ref tmp_CurrentFOV, 
                    Time.deltaTime * 5);
            }
        }

        private void test()
        {
            if(Input.GetKeyDown(KeyCode.Q))
            {
                CurrentMaxAmmoCarried = MaxAmmoCarried;
                CurrentAmmo = AmmoInMag;
            }
        }
    }
}

