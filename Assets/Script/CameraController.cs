using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Script.Controll
{
    public class CameraController : MonoBehaviour
    {
        public Transform Player;
        public static Transform CameraRo;
        public Transform Camera;
        public LayerMask PlayerMask;
        public float topAngle = 90f;
        public float lowAngle = -90f;
        public float CameraOffsetY = 2f;
        public float CameraOffsetX = 1f;
        public float AimOffsetX = 1f;
        public float AimOffsetY = 1f;
        public Quaternion AimRotation;
        public float cameraDistance = 3f;
        public float cameraSensitivity = 3f;
        public float cameraSpeed = 3f;

        protected float xRotation;
        protected float yRotation;
        protected float zMove;
        protected float xMove;
        protected float originOffsetY;
        protected float originOffsetX;

        void Start()
        {
            Player = GameObject.FindGameObjectWithTag("Player").transform;
            CameraRo = GameObject.Find("CameraRo").transform;
            Camera = transform;
            Camera.position = CameraRo.position - CameraRo.forward * cameraDistance;
            originOffsetX = CameraOffsetX;
            originOffsetY = CameraOffsetY;
        }

        void Update()
        {
            RotateCamera();
            MoveTarget();
            CollisionDetect();
        }
        protected void MoveTarget()
        {
            if (PlayerController_TPS_Anim.isAiming)
            {
                CameraOffsetX = originOffsetX + AimOffsetX;
                CameraOffsetY = originOffsetY + AimOffsetY;
                Camera.localRotation = AimRotation;
            }
            else
            {
                CameraOffsetX = originOffsetX;
                CameraOffsetY = originOffsetY;
                Camera.LookAt(CameraRo);
            }
            CameraRo.position = Player.position + CameraOffsetY * Vector3.up + Quaternion.Euler(0, yRotation, 0) * Vector3.right * CameraOffsetX;
        }

        protected void RotateCamera()
        {
            xRotation += PlayerController.mouseY * -cameraSensitivity;
            yRotation += PlayerController.mouseX * cameraSensitivity;
            xRotation = Mathf.Clamp(xRotation,lowAngle,topAngle);
            Quaternion localRotation = Quaternion.Euler(xRotation, yRotation, 0);
            CameraRo.localRotation = localRotation;
        }
        protected void CollisionDetect()
        {
            RaycastHit hitInfo;
            if (Physics.Raycast(CameraRo.position, (Camera.position - CameraRo.position).normalized, out hitInfo, cameraDistance))
            {
                Camera.position = hitInfo.point;
            }
            else 
            {
                if(PlayerController_TPS_Anim.isAiming)
                Camera.position = Vector3.Lerp(Camera.position, CameraRo.position - CameraRo.forward * cameraDistance,cameraSpeed*Time.deltaTime);
            }
        }
    }

}
