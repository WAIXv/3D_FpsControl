using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Script.Controll
{
    public class PlayerController_TPS : PlayerController
    {
        [Header("TPS")]
        public Transform Target;
        public float turnSpeed = 15f;

        protected float xMove;
        protected float zMove;
        protected Vector3 direction;
        protected float targetAngle;
        protected float smoothAngle;
        protected float tmp_turnsmoothAngle;

        public override void Start()
        {
            base.Start();
            Target = GameObject.Find("CameraTarget").transform;
        }

        protected override void Update()
        {
            base.Update();
        }
        protected override void Move()
        {
            //使得玩家目标移动方向转向视角前方
            direction = (Vector3.forward * move_Vertical + Vector3.right * move_Horizontal).normalized;
            Rigid.velocity += Vector3.up * gravity * Time.deltaTime;    //重力
            if (Input.GetButton("Fire3"))
            {
                direction = (Vector3.forward * move_Vertical + Vector3.right * move_Horizontal).normalized * runMultiplier ;
            }
            Anim.SetFloat("speed", moveDir.magnitude * speed, 0.2f, Time.deltaTime);
            moveDir = Quaternion.Euler(0f, Target.eulerAngles.y, 0f) * direction;
            Rigid.velocity = new Vector3(moveDir.x * speed, Rigid.velocity.y, moveDir.z * speed);
        }
        protected override void Rotate()
        {
            if (direction.magnitude >= 0.1f)
            {
                targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + Target.eulerAngles.y;
                smoothAngle = Mathf.SmoothDampAngle(Player.eulerAngles.y, targetAngle, ref tmp_turnsmoothAngle, turnSpeed * Time.deltaTime);
                Player.eulerAngles = new Vector3(0, smoothAngle, 0);
            }
        }
    }
}

