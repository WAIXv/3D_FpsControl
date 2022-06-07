using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCon : MonoBehaviour
{


    //IK相关

    Animator animator;

    [SerializeField]
    Vector3 rhandOffset;

    [SerializeField]
    Vector3 lhandOffset;

    //手枪相关

    [SerializeField]
    Transform gunaimRo;//旋转节点

    [SerializeField]
    Transform gunbasePoint;

    [SerializeField]
    Transform gunaimPoint;

    [SerializeField]
    Transform gun;

    [SerializeField]
    float gunMoveSpeed = 8.0f;

    [SerializeField]
    float gunRoSpeed = 60.0f;

    //相机相关
    [SerializeField]
    Transform cmabasePoint;

    [SerializeField]
    Transform cmaaimPoint;

    [SerializeField]
    Transform cma;

    [SerializeField]
    float moveSpeed = 12.0f;//相机位置匹配速度

    [SerializeField]
    float roHSpeed = 180.0f;//人物水平旋转速度

    [SerializeField]
    float roPSpeed = 180.0f;//相机纵向旋转速度

    [SerializeField]
    float pMax = 50f;

    [SerializeField]
    float pMin = 10f;

    float pnow;//由于万向锁反解问题，这里需要手动记录相机PitchX旋量

    private void Awake()
    {
        pnow = cma.localEulerAngles.x;

        Cursor.visible = false;

        mask = 1 << LayerMask.NameToLayer("Aim");

        animator = gameObject.GetComponent<Animator>();
    }

    //Temp

    float dis;//距离
    float mstep;//这一帧运动的步长

    Transform cmago;
    Transform gungo;

    int mask;

    RaycastHit raycastHit;

    private void LateUpdate()
    {
        if (!Cursor.visible) { //鼠标隐藏进行控制

            
            if (Input.GetKeyDown(KeyCode.Escape)) {

                Cursor.visible = true;
                return;

            }

            Debug.Log("INININ");
            //人物旋转与相机旋转
            if (Input.GetAxis("Mouse X") > 0)
                transform.Rotate(new Vector3(0, roHSpeed * Time.deltaTime, 0));
            else if (Input.GetAxis("Mouse X") < 0)
                transform.Rotate(new Vector3(0, -roHSpeed * Time.deltaTime, 0));

            //相机要做X轴角度限制

            if (Input.GetAxis("Mouse Y") > 0)
                pnow -= roPSpeed * Time.deltaTime;
            else if (Input.GetAxis("Mouse Y") < 0)
                pnow += roPSpeed * Time.deltaTime;

            pnow = Mathf.Clamp(pnow, pMin, pMax);

            cma.localEulerAngles = new Vector3(pnow, 0, 0);


            //相机位置
            if (Input.GetMouseButton(1)) { //右键Hold进行瞄准状态
                cmago = cmaaimPoint;
                gungo = gunaimPoint;

                //根据相机的射线击中，设置gunanim的姿态
                if (Physics.Raycast(cma.position, cma.forward, out raycastHit, 1000.0f, mask)) {
                    gunaimRo.LookAt(raycastHit.point);//看向射线击中点
                    //Debug.Log(raycastHit.transform.name);
                }
                else {
                    gunaimRo.forward = cma.forward; //平行线无限远处相交
                }

            }
            else { //正常第三人称状态
                cmago = cmabasePoint;
                gungo = gunbasePoint;
            }

            //相机位置匹配
            dis = Vector3.Distance(cma.position, cmago.transform.position);
            mstep = Time.deltaTime * moveSpeed;
            cma.position = Vector3.MoveTowards(cma.position, cmago.position, mstep < dis ? mstep : dis / 20);

            //枪位置匹配
            dis = Vector3.Distance(gun.position, gungo.position);
            mstep = Time.deltaTime * gunMoveSpeed;
            gun.position = Vector3.MoveTowards(gun.position, gungo.position, mstep < dis ? mstep : dis / 20);

            //枪旋转匹配
            dis = Vector3.Angle(gun.forward, gungo.forward);
            mstep = Time.deltaTime * gunRoSpeed;
            //if (mstep < dis) {
            //    Debug.Log("onLerp!!!");
            //}
            gun.rotation = Quaternion.Slerp(gun.rotation, gungo.rotation, mstep < dis ? mstep : dis / 80);

        }
        else if (Input.GetMouseButtonDown(0)) {
            Cursor.visible = false;
        }
    }


    private void OnAnimatorIK(int layerIndex)
    {
        if (layerIndex == 0 && (Input.GetMouseButton(1)&&!Cursor.visible)) {

            //差量矫正要切换至枪的本地空间运动方向


            animator.SetIKRotation(AvatarIKGoal.RightHand, Quaternion.LookRotation(gun.forward, gunaimPoint.right));
            animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1.0f);

            animator.SetIKPosition(AvatarIKGoal.RightHand, gun.position + rhandOffset.x * gunaimPoint.right + rhandOffset.y * gunaimPoint.up + rhandOffset.z * gunaimPoint.forward);
            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1.0f);


            animator.SetIKRotation(AvatarIKGoal.LeftHand, Quaternion.LookRotation(gun.right, -Vector3.up));
            animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1.0f);

            animator.SetIKPosition(AvatarIKGoal.LeftHand, gun.position + lhandOffset.x * gunaimPoint.right + lhandOffset.y * gunaimPoint.up + lhandOffset.z * gunaimPoint.forward);
            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1.0f);

            

        }

        //if (layerIndex == 1 && (true || Input.GetMouseButton(1) && !Cursor.visible)) {

            
            
        //}
    }

    private void OnGUI()
    {
        GUILayout.Button(Input.GetAxis("Mouse X").ToString());
        GUILayout.Button(Input.GetAxis("Mouse Y").ToString());
    }
}
