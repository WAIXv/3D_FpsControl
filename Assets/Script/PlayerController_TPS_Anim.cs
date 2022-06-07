using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Script.Weapon;

namespace Script.Controll
{
    public class PlayerController_TPS_Anim : PlayerController
    {
        [Header("TPS")]
        public ReCoil ReCoil_Script;
        public Transform CameraRo;
        public float SpineOffset;
        public float speedUpTime = 3f;
        public float turnTime = 3f;
        public float aimInSpeed = 10f;
        public float aimOutSpeed = 5f;
        public Vector3 GunHold_PositionOffset;
        public Vector3 RightHandIK_PositionOffset;
        public Vector3 LeftHandIK_PositionOffset;
        public Quaternion RightHandIK_RotationOffset;
        public Quaternion LeftHandIK_RotationOffset;
        
        [Header("AimMatching")]
        [SerializeField]
        Transform PlayerSpine;
        [SerializeField]
        Transform PlayerSpine2;
        [SerializeField]
        Transform PlayerRightShoulder;
        [SerializeField]
        Transform PlayerRighthand;
        [SerializeField]
        Transform AimPoint;
        float aimDistance = 5f;
        [SerializeField]
        float spineOffsetAngle = 30f;
        [SerializeField]
        float spine2OffsetAngle = 5f;
        [SerializeField]
        float rightShoulderOffsetAngle_X = 2.5f;
        [SerializeField]
        float rightShoulderOffsetAngle_Y = 10f;


        protected float currentAngle;
        protected float targetAngle;
        protected float currentIncreaseSpeed;
        protected float targetIncreaseSpeed;
        protected float tmp_turnAngle = 0f;
        protected float tmp_addSpeed = 0f;
        protected int speedID = Animator.StringToHash("Speed");
        protected int turnID = Animator.StringToHash("Turn");
        protected int aimID = Animator.StringToHash("Aiming");
        public static int aimLayerID = 3;
        protected float aimLayerWeight = 0f;
        public static bool isAiming;
        public static bool isAimingIn;
        public static bool isHoldingGun;
        public static Vector3 TargetPoint;
        protected Vector3 currentGunPosition;
        protected Vector3 AimPoint_Horizontal;
        protected Vector3 TargetPoint_Vertical;

        [SerializeField]
        Transform gunaimRo;

        [SerializeField]
        Transform gunaimPoint;

        protected IEnumerator doAimCoroutine;

        public GameObject testcube;
        public Transform GunRotateCenter;


        public override void Start()
        {
            base.Start();
            CameraRo = GameObject.Find("CameraRo").transform;
            targetIncreaseSpeed = runSpeed - speed;
            PlayerSpine = GameObject.Find("mixamorig:Spine").transform;
            PlayerSpine2 = GameObject.Find("mixamorig:Spine2").transform;
            PlayerRightShoulder = GameObject.Find("mixamorig:RightShoulder").transform;
            PlayerRighthand = GameObject.Find("mixamorig:RightHand").transform;

            doAimCoroutine = DoAim();
        }

        Transform gungo;
        float dis;
        float mstep;

        protected override void Update()
        {
            base.Update();
            //SetGunPosition();
        }
        protected void LateUpdate()
        {
            if (Input.GetMouseButtonDown(1))
            {
                isAiming = true;
                isAimingIn = true;
                Aim();
            }
            if (Input.GetMouseButtonUp(1))
            {
                isAiming = false;
                isAimingIn = false;
                Aim();
            }
            GetTargetPoint();
            AimMatching();
            if(HandGun_TPS.fireInFrame)
            {
                StartCoroutine(ReCoil());
            }
        }

        private void OnAnimatorIK(int layerIndex)
        {
            if (layerIndex == aimLayerID)
            {
                Anim.SetIKRotation(AvatarIKGoal.LeftHand, Quaternion.LookRotation(gun.forward, -gunaimPoint.right));
                Anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, PlayerController_TPS_Anim.isAiming ? 1 : 0);

                Anim.SetIKPosition(AvatarIKGoal.LeftHand, gun.position + LeftHandIK_PositionOffset.x * gun.transform.right + LeftHandIK_PositionOffset.y * gun.transform.up + LeftHandIK_PositionOffset.z * gun.transform.forward);
                Anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, PlayerController_TPS_Anim.isAiming ? 1 : 0);

                Anim.SetIKRotation(AvatarIKGoal.RightHand, Quaternion.LookRotation(gun.forward, gunaimPoint.right));
                Anim.SetIKRotationWeight(AvatarIKGoal.RightHand, PlayerController_TPS_Anim.isAiming? 1 : 0);    

                Anim.SetIKPosition(AvatarIKGoal.RightHand, gun.position + RightHandIK_PositionOffset.x * gun.transform.right + RightHandIK_PositionOffset.y * gun.transform.up + RightHandIK_PositionOffset.z * gun.transform.forward);
                Anim.SetIKPositionWeight(AvatarIKGoal.RightHand, PlayerController_TPS_Anim.isAiming ? 1 : 0);
            }
        }

        protected IEnumerator ReCoil()
        {
            while (true)
            {
                yield return null;
            }
        }

        protected void AimMatching()
        {
            if(isAiming)
            {
                aimInSpeed = 10;
                gungo = gunaimPoint;
                PlayerSpine.rotation = Quaternion.Euler(0, CameraRo.eulerAngles.y, 0);
                PlayerSpine2.localRotation = Quaternion.Euler(0, spine2OffsetAngle, 0);
                gunaimRo.LookAt(TargetPoint);


                dis = Vector3.Distance(gun.position, gungo.position);
                mstep = Time.deltaTime * aimInSpeed * 10;
                gun.position = Vector3.MoveTowards(gun.position, gungo.position, dis < mstep ? mstep : dis / 30);

                dis = Vector3.Angle(gun.forward, gungo.forward);
                mstep = Time.deltaTime * aimInSpeed;
                gun.rotation = Quaternion.Slerp(gun.rotation, gungo.rotation, dis < mstep ? mstep : dis / 80);
            }
            else
            {
                aimInSpeed = 100;
                gun.position = PlayerRighthand.position + GunHold_PositionOffset.x * PlayerRighthand.right + GunHold_PositionOffset.y*PlayerRighthand.up+GunHold_PositionOffset.z*PlayerRighthand.forward;
                gun.rotation = Quaternion.LookRotation(PlayerRighthand.right, PlayerRighthand.forward);
            }
        }
        protected void GetTargetPoint()
        {
            Vector3 dir = MainCamera.transform.forward;
            RaycastHit hit; 
            if (Physics.Raycast(MainCamera.transform.position, dir, out hit, 1000))
            {
                TargetPoint = hit.point;
            }
            Debug.DrawRay(MainCamera.transform.position, dir * 1000);
        }

        protected override void Aim()
        {
            if (doAimCoroutine == null)
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
        }
        protected IEnumerator DoAim()
        {
            while (true)
            {
                yield return null;
                    aimLayerWeight = isAiming ? Mathf.Lerp(aimLayerWeight, 1f, aimInSpeed * Time.deltaTime) : Mathf.Lerp(aimLayerWeight, 0f, aimOutSpeed * Time.deltaTime);
                    Anim.SetLayerWeight(3, aimLayerWeight);
                    Anim.SetLayerWeight(2, aimLayerWeight);
                    Anim.SetBool(aimID, isAiming);
                float tmp_CurrentFOV = 0;
                MainCamera.fieldOfView =
                    Mathf.SmoothDamp(MainCamera.fieldOfView,
                    isAiming ? aimFOV : originFOV,
                    ref tmp_CurrentFOV,
                    Time.deltaTime * 5);
                if (aimLayerWeight > 0.95f)
                {
                    isAimingIn = false;
                }
                else
                {
                    isAimingIn = true;
                }
                Debug.Log(isAimingIn);
            }     
        }
        protected override void Move()
        {
            Rigid.velocity += Vector3.up * gravity * Time.deltaTime;    //ÖØÁ¦
            var speed = Mathf.Clamp(moveDir.magnitude, 0, 1);
            if (Input.GetKey(KeyCode.LeftShift))
            {
                currentIncreaseSpeed = Mathf.SmoothDamp(currentIncreaseSpeed, targetIncreaseSpeed, ref tmp_addSpeed, speedUpTime * Time.deltaTime);
            }
            else
            {
                currentIncreaseSpeed = Mathf.SmoothDamp(currentIncreaseSpeed, 0, ref tmp_addSpeed, speedUpTime * Time.deltaTime);
            }
            Anim.SetFloat(speedID, Mathf.Clamp(moveDir.magnitude, 0, 1) + currentIncreaseSpeed);
            if (moveDir.z < 0 && isAiming)
            {
                Anim.SetFloat(speedID, -Mathf.Clamp(moveDir.magnitude, 0, 1) - currentIncreaseSpeed);
            }
        }
        protected override void Rotate()
        {
            moveDir = move_Vertical * Vector3.forward + move_Horizontal * Vector3.right;
            if (moveDir.magnitude >= 0.1f || isAiming)
            {
                if (Anim.GetFloat(speedID) < 0)
                    return;
                targetAngle = Mathf.Atan2(moveDir.x, moveDir.z) * Mathf.Rad2Deg + CameraRo.eulerAngles.y;
                currentAngle = Mathf.SmoothDampAngle(currentAngle, targetAngle, ref tmp_turnAngle, turnTime * Time.deltaTime);
                Player.eulerAngles = new Vector3(0, currentAngle, 0);
            }
        }
        protected override void Jump()
        {
            
        }
    }
}

