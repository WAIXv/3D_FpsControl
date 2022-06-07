using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCon : MonoBehaviour
{


    //IK���

    Animator animator;

    [SerializeField]
    Vector3 rhandOffset;

    [SerializeField]
    Vector3 lhandOffset;

    //��ǹ���

    [SerializeField]
    Transform gunaimRo;//��ת�ڵ�

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

    //������
    [SerializeField]
    Transform cmabasePoint;

    [SerializeField]
    Transform cmaaimPoint;

    [SerializeField]
    Transform cma;

    [SerializeField]
    float moveSpeed = 12.0f;//���λ��ƥ���ٶ�

    [SerializeField]
    float roHSpeed = 180.0f;//����ˮƽ��ת�ٶ�

    [SerializeField]
    float roPSpeed = 180.0f;//���������ת�ٶ�

    [SerializeField]
    float pMax = 50f;

    [SerializeField]
    float pMin = 10f;

    float pnow;//�����������������⣬������Ҫ�ֶ���¼���PitchX����

    private void Awake()
    {
        pnow = cma.localEulerAngles.x;

        Cursor.visible = false;

        mask = 1 << LayerMask.NameToLayer("Aim");

        animator = gameObject.GetComponent<Animator>();
    }

    //Temp

    float dis;//����
    float mstep;//��һ֡�˶��Ĳ���

    Transform cmago;
    Transform gungo;

    int mask;

    RaycastHit raycastHit;

    private void LateUpdate()
    {
        if (!Cursor.visible) { //������ؽ��п���

            
            if (Input.GetKeyDown(KeyCode.Escape)) {

                Cursor.visible = true;
                return;

            }

            Debug.Log("INININ");
            //������ת�������ת
            if (Input.GetAxis("Mouse X") > 0)
                transform.Rotate(new Vector3(0, roHSpeed * Time.deltaTime, 0));
            else if (Input.GetAxis("Mouse X") < 0)
                transform.Rotate(new Vector3(0, -roHSpeed * Time.deltaTime, 0));

            //���Ҫ��X��Ƕ�����

            if (Input.GetAxis("Mouse Y") > 0)
                pnow -= roPSpeed * Time.deltaTime;
            else if (Input.GetAxis("Mouse Y") < 0)
                pnow += roPSpeed * Time.deltaTime;

            pnow = Mathf.Clamp(pnow, pMin, pMax);

            cma.localEulerAngles = new Vector3(pnow, 0, 0);


            //���λ��
            if (Input.GetMouseButton(1)) { //�Ҽ�Hold������׼״̬
                cmago = cmaaimPoint;
                gungo = gunaimPoint;

                //������������߻��У�����gunanim����̬
                if (Physics.Raycast(cma.position, cma.forward, out raycastHit, 1000.0f, mask)) {
                    gunaimRo.LookAt(raycastHit.point);//�������߻��е�
                    //Debug.Log(raycastHit.transform.name);
                }
                else {
                    gunaimRo.forward = cma.forward; //ƽ��������Զ���ཻ
                }

            }
            else { //���������˳�״̬
                cmago = cmabasePoint;
                gungo = gunbasePoint;
            }

            //���λ��ƥ��
            dis = Vector3.Distance(cma.position, cmago.transform.position);
            mstep = Time.deltaTime * moveSpeed;
            cma.position = Vector3.MoveTowards(cma.position, cmago.position, mstep < dis ? mstep : dis / 20);

            //ǹλ��ƥ��
            dis = Vector3.Distance(gun.position, gungo.position);
            mstep = Time.deltaTime * gunMoveSpeed;
            gun.position = Vector3.MoveTowards(gun.position, gungo.position, mstep < dis ? mstep : dis / 20);

            //ǹ��תƥ��
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

            //��������Ҫ�л���ǹ�ı��ؿռ��˶�����


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
