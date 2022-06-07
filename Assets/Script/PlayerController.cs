using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using namespace_GameObjectManager;

namespace Script.Controll
{
    public class PlayerController : MonoBehaviour
    {
        public Rigidbody Rigid;
        public Transform Player;
        public Camera MainCamera;
        public Animator Anim;
        public GameObject View;
        public GameObject GroundCheck_L;
        public GameObject GroundCheck_R;
        [Header("“∆∂Ø")]
        public float speed = 3f;
        public float runSpeed = 7f;
        public float rotateSpeed = 3f;
        public float groundCheckRange = 0.04f;
        public LayerMask groundMask;
        public float gravity = -9.81f;
        [Header(" ”Ω«")]
        public float CameraSensitivity = 3f;
        public float topAngle = 90f;
        public float lowAngle = -90f;
        [Header("…‰ª˜")]
        public Transform gun;
        public float shootDistance = 1000;
        public float aimFOV = 42;
        [Header("Ã¯‘æ")]
        public float jumpSpeed = 10;

        public static float move_Horizontal;
        public static float move_Vertical;
        public static float move_HorizontalRaw;
        public static float move_VerticalRaw;
        public static float mouseX;
        public static float mouseY;
        public Vector3 moveDir;
        protected float xRotation;
        protected float yRotation;
        protected float runMultiplier;
        protected bool playerOnGround;
        protected float originFOV;

        public virtual void Start()
        {
            Player = transform;
            MainCamera = Camera.main;
            Rigid = GetComponent<Rigidbody>();
            gun = GameObject.FindGameObjectWithTag("Gun").transform;
            if (Anim == null) { Anim = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>(); }
            if (View == null)   { View = GameObject.FindGameObjectWithTag("Player_View"); }
            GroundCheck_L = GameObject.Find("GroundCheck_L");
            GroundCheck_R = GameObject.Find("GroundCheck_R");
            originFOV = MainCamera.fieldOfView;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            transform.eulerAngles = Vector3.zero;
            runMultiplier = runSpeed / speed;   
        }

        protected virtual void FixedUpdate()
        {
            Move();
        }

        protected virtual void Update()
        {
            //Ω≈≤»µÿºÏ≤‚
            playerOnGround = Physics.Raycast(GroundCheck_L.transform.position, Vector3.down, groundCheckRange, groundMask) 
                || Physics.Raycast(GroundCheck_R.transform.position, Vector3.down, groundCheckRange, groundMask);
            Rotate();
            GetInput();
            Jump();
            
        }
        protected virtual void Rotate()
        {
            yRotation = mouseX * CameraSensitivity;
            xRotation += mouseY * -CameraSensitivity;
            xRotation = Mathf.Clamp(xRotation, lowAngle, topAngle);
            Player.Rotate(transform.up,yRotation);
            View.transform.eulerAngles = new Vector3(xRotation, View.transform.eulerAngles.y, View.transform.eulerAngles.z);
        }

        protected virtual void Move()
        {
            Rigid.velocity += Vector3.up * gravity * Time.deltaTime;    //÷ÿ¡¶
            moveDir = (Player.right * move_HorizontalRaw + Player.forward * move_VerticalRaw).normalized;
            if (Input.GetButton("Fire3"))
            {
                moveDir = runMultiplier * (Player.right * move_HorizontalRaw + Player.forward * move_VerticalRaw).normalized;
            }
            Anim.SetFloat("speed", moveDir.magnitude*speed, 0.2f, Time.deltaTime);
            Rigid.velocity = new Vector3(moveDir.x * speed, Rigid.velocity.y, moveDir.z * speed);
        }

        protected virtual void Jump()
        {
            if(Input.GetButtonDown("Jump"))
            {
                Debug.Log("jump");
                Rigid.velocity += jumpSpeed * Vector3.up;
                Anim.SetTrigger("Jump");
            }
        }

        protected virtual void Aim()
        {

        }


        public static void GetInput()
        {
            move_Vertical = Input.GetAxis("Vertical");
            move_Horizontal = Input.GetAxis("Horizontal");
            move_VerticalRaw = Input.GetAxisRaw("Vertical");
            move_HorizontalRaw = Input.GetAxisRaw("Horizontal");
            mouseX = Input.GetAxis("Mouse X");
            mouseY = Input.GetAxis("Mouse Y");
        }

    }
}


