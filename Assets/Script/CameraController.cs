using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Script.Controll
{
    public class CameraController : MonoBehaviour
    {
        public Transform Player;
        public Transform Target;
        public Transform Camera;
        public float topAngle = 90f;
        public float lowAngle = -90f;
        public float cameraPositionY = 2f;
        public float cameraPositionX = 1f;
        public float cameraDistance = 3f;
        public float cameraSensitivity = 3f;
        public float cameraSpeed = 3f;

        protected float xRotation;
        protected float yRotation;
        protected float zMove;
        protected float xMove;
        protected Vector3 offset;
        protected float offsetRange;

        void Start()
        {
            Player = GameObject.FindGameObjectWithTag("Player").transform;
            Target = GameObject.Find("CameraTarget").transform;
            Camera = transform;
            Camera.position = Target.position - Target.forward * cameraDistance;
        }

        void Update()
        {
            RotateCamera();
            MoveTarget();
            CollisionDetect();
        }
        protected void MoveTarget()
        {
            Target.position = Player.position +  cameraPositionY * Vector3.up + Quaternion.Euler(0,yRotation,0) * Vector3.right * cameraPositionX  ;
        }

        protected void RotateCamera()
        {
            xRotation += PlayerController.mouseY * -cameraSensitivity;
            yRotation += PlayerController.mouseX * cameraSensitivity;
            xRotation = Mathf.Clamp(xRotation,lowAngle,topAngle);
            Quaternion localRotation = Quaternion.Euler(xRotation, yRotation, 0);
            Target.localRotation = localRotation;
        }
        protected void CollisionDetect()
        {
            RaycastHit hitInfo;
            if (Physics.Raycast(Target.position, (Camera.position - Target.position).normalized, out hitInfo, cameraDistance))
            {
                Camera.position = hitInfo.point;
            }
            else 
            {
                Camera.position = Vector3.Lerp(Camera.position, Target.position - Target.forward * cameraDistance,cameraSpeed*Time.deltaTime);
            }
        }
    }

}
