using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Script.Controll
{
    public class PlayerController_TPS_Anim : PlayerController
    {
        [Header("TPS")]
        public Transform Target;
        public float speedUpTime = 3f;
        public float turnTime = 3f;

        protected float currentAngle;
        protected float targetAngle;
        protected float currentIncreaseSpeed;
        protected float targetIncreaseSpeed;
        protected float tmp_turnAngle = 0f;
        protected float tmp_addSpeed = 0f;
        protected int speedID = Animator.StringToHash("Speed");
        protected int turnID = Animator.StringToHash("Turn");


        public override void Start()
        {
            base.Start();
            Target = GameObject.Find("CameraTarget").transform;
            targetIncreaseSpeed = runSpeed - speed;
        }

        protected override void Update()
        {
            base.Update();
        }
        protected override void Move()
        {
            Rigid.velocity += Vector3.up * gravity * Time.deltaTime;    //ÖØÁ¦
            
            if(Input.GetKey(KeyCode.LeftShift))
            {
                currentIncreaseSpeed = Mathf.SmoothDamp(currentIncreaseSpeed, targetIncreaseSpeed, ref tmp_addSpeed, speedUpTime * Time.deltaTime);
                Debug.Log(currentIncreaseSpeed);
            }
            else
            {
                currentIncreaseSpeed = Mathf.SmoothDamp(currentIncreaseSpeed, 0, ref tmp_addSpeed, speedUpTime * Time.deltaTime);
            }
            Anim.SetFloat(speedID, Mathf.Clamp(moveDir.magnitude, 0, 1) + currentIncreaseSpeed);
        }
        protected override void Rotate()
        {
            moveDir = move_Vertical * Vector3.forward + move_Horizontal * Vector3.right;
            if (moveDir.magnitude >= 0.1f)
            {
                targetAngle = Mathf.Atan2(moveDir.x, moveDir.z) * Mathf.Rad2Deg + Target.eulerAngles.y;
                currentAngle = Mathf.SmoothDampAngle(currentAngle, targetAngle, ref tmp_turnAngle, turnTime * Time.deltaTime);
                Player.eulerAngles = new Vector3(0, currentAngle, 0);
            }
        }
        protected override void Jump()
        {
            
        }
    }
}

