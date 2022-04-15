using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using namespace_EnemyController;


namespace namespace_GameObjectManager
{
    public class GameObjectManager : MonoBehaviour
    {
        public GameObject[] Enemy;
        public GameObject[] Target;
        private int enemyCount;
        private int targetCount;
        public static int currentEnemyCount;
        public static int currentTargetCount;

        void Start()
        {
            Enemy = GameObject.FindGameObjectsWithTag("Enemy");
            enemyCount = Enemy.Length;
            currentEnemyCount = enemyCount;

            Target = GameObject.FindGameObjectsWithTag("Target");
            targetCount = Target.Length;
            currentTargetCount = targetCount;

        }

        void Update()
        {
            if (!(currentEnemyCount == enemyCount))
            {
                StartCoroutine(reActiveObject(Enemy));
            }
            else if (!(currentTargetCount == targetCount))
            {
                Debug.Log(currentTargetCount);
                Debug.Log(targetCount);
                StartCoroutine(reActiveObject(Target));
            }
        }

        IEnumerator reActiveObject(GameObject[] obj)
        {
            for (int i = obj.Length - 1; i >= 0; i--)
            {
                if (!obj[i].activeSelf)
                {
                    if (obj[i].tag == "Enemy")
                    {
                        Debug.Log("Enemy重生" + obj[i].GetComponent<EnemyController>().reActiveTime + "s");
                        float r = Random.Range(obj[i].GetComponent<EnemyController>().rightLimit, obj[i].GetComponent<EnemyController>().leftLimit);
                        obj[i].transform.position = new Vector3(obj[i].transform.position.x, obj[i].transform.position.y, r);
                        Debug.Log(r);
                        currentEnemyCount += 1;

                        yield return new WaitForSeconds(obj[i].GetComponent<EnemyController>().reActiveTime);
                        obj[i].SetActive(true);

                    }
                    else if (obj[i].tag == "Target")
                    {
                        Debug.Log("Target重生" + obj[i].GetComponent<TargetController>().reActiveTime + "s");
                        Vector3 centerPoint = obj[i].GetComponent<TargetController>().centerPoint;
                        float range = obj[i].GetComponent<TargetController>().range;
                        float xmove = Random.Range(-range, range);
                        float ymove = Random.Range(0, range);
                        float zmove = Random.Range(-range, range);
                        obj[i].transform.position = new Vector3(centerPoint.x + xmove, centerPoint.y + ymove, centerPoint.z + zmove);
                        currentTargetCount += 1;
                        Debug.Log(obj[i].name+obj[i].transform.position);

                        yield return new WaitForSeconds(obj[i].GetComponent<TargetController>().reActiveTime);
                        obj[i].SetActive(true);

                    }
                }
            }
        }
    }

}

