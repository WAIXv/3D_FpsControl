using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Script.Controll;

namespace Script.Weapon
{
    public class HandGun_TPS : FireArms
    {
        public static bool fireInFrame = false;
        private IEnumerator reloadAnimCheckCoroutine;

        private void Update()
        {
            if (Input.GetMouseButton(0))
            {
                DoAttack();
            }
            GetTargetPoint();
            test();
            Debug.DrawLine(MuzzlePiont.position, TargetPoint);
        }

        protected override void Aim()
        {
            
        }

        protected override void Reload()
        {
            GunAnimator.SetLayerWeight(1, 1);
            GunAnimator.SetTrigger(CurrentAmmo > 0 ? "ReloadLeft" : "ReloadOutOf");

            FirearmsReloadAudioSource.clip =
                CurrentAmmo > 0
                ? FirearmsAudioData.ReloadLeft
                : FirearmsAudioData.ReloadOutof;
            FirearmsReloadAudioSource.Play();

            if (reloadAnimCheckCoroutine == null)
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
            if(!PlayerController_TPS_Anim.isAiming)
            {
                StartCoroutine(ShootWithOutAim());
            }

            fireInFrame = true;
            MuzzleParticle.Play();
            CurrentAmmo -= 1;
            GunAnimator.Play("Fire", isAiming ? 1 : 0, 0);
            FirearmsShootingAudioSource.clip = FirearmsAudioData.ShootingAudio;
            FirearmsShootingAudioSource.Play();

            MuzzlePiont.LookAt(TargetPoint);
            CreateBullet();
            CasingParticle.Play();
            LastFireTime = Time.time;
        }

        private IEnumerator ShootWithOutAim()
        {
            LastFireTime = Time.deltaTime;
            while (true)
            {
                yield return null;
                PlayerController_TPS_Anim.isAiming = true;
                Debug.Log("Enter Corontine");
                
                if (Input.GetMouseButtonDown(1))
                {
                    yield break;
                }
                if (Time.time - LastFireTime >= 2f)
                {
                    PlayerController_TPS_Anim.isAiming = false;
                    yield break;
                }
            }
        }

        private IEnumerator CheckReloadAnimationEnd()
        {
            while (true)
            {
                yield return null;
                isLoading = true;
                GunStateInfo = GunAnimator.GetCurrentAnimatorStateInfo(1);
                if (GunStateInfo.IsTag("ReloadAmmo"))
                {
                    if (GunStateInfo.normalizedTime >= 0.9f)
                    {
                        int tmp_NeedAmmoCount = AmmoInMag - CurrentAmmo;
                        int tmp_RemainAmmo = CurrentMaxAmmoCarried - tmp_NeedAmmoCount;
                        isLoading = false;
                        if (tmp_RemainAmmo <= 0)
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

        protected void CreateBullet()
        {
            GameObject tmp_Bullet = Instantiate(BulletPrefab, MuzzlePiont.position, MuzzlePiont.rotation);
            var tmp_BulletScript = tmp_Bullet.GetComponent<Bullet>();
            tmp_BulletScript.ImpactPrefab = BulletImpactPrefab;
            tmp_BulletScript.ImpactAudioData = ImpactAudioData;
            tmp_BulletScript.BulletSpeed = 200f;
        }

        private void test()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                CurrentMaxAmmoCarried = MaxAmmoCarried;
                CurrentAmmo = AmmoInMag;
            }
            
        }

    }
}

