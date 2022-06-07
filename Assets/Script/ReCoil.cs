using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Script.Weapon;

namespace Script.Weapon
{
    public class ReCoil : MonoBehaviour
    {

        //Rotations
        private Vector3 currentRotation;
        private Vector3 targetRotation;

        //Recoil
        [SerializeField] private Vector3 recoil;

        //Aim Recoil
        [SerializeField] private Vector3 aimRecoil;

        //Settings
        [SerializeField] private float snappiness;
        [SerializeField] private float returnSpeed;

        void Update()
        {
            targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, returnSpeed * Time.deltaTime);      //后座仰角值回正
            currentRotation = Vector3.Slerp(currentRotation, targetRotation, snappiness * Time.fixedDeltaTime);     //枪口跳动
            transform.localRotation = Quaternion.Euler(currentRotation);
        }

        public Vector3 RecoilFire()
        {
            targetRotation += new Vector3(recoil.x, Random.Range(-recoil.y, recoil.y), Random.Range(-recoil.z, recoil.z));
            return targetRotation;
        }
    }

}

