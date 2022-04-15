using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using namespace_GameObjectManager;

namespace Script.Weapon
{
    public class Bullet : MonoBehaviour
    {
        public float BulletSpeed;
        public GameObject ImpactPrefab;
        public ImpactAudioData ImpactAudioData;
        private Transform bulletTranform;
        private Vector3 prePosition;

        private void Start()
        {
            bulletTranform = transform;
            prePosition = bulletTranform.position;

        }

        private void Update()
        {
            prePosition = bulletTranform.position;
            bulletTranform.Translate(BulletSpeed * Time.deltaTime, 0, 0, Space.Self);
            if (!Physics.Raycast(prePosition,
                (transform.position - prePosition).normalized,
                out RaycastHit tmp_Hit,
                (transform.position - prePosition).magnitude))
            {
                Destroy(bulletTranform.gameObject, 3f);
                return;
            }
            Destroy(bulletTranform.gameObject);

            //创建弹孔特效
            GameObject tmp_BulletImpact =
            Instantiate(ImpactPrefab, tmp_Hit.point, Quaternion.LookRotation(tmp_Hit.normal, Vector3.up));
            Destroy(tmp_BulletImpact, 3);

            Debug.Log(tmp_Hit.collider.tag);
            if (tmp_Hit.collider.CompareTag("Enemy")|| tmp_Hit.collider.CompareTag("Enemy_Head"))
            {
                Debug.Log("Hit Enemy");
                tmp_Hit.collider.gameObject.SetActive(false);
                GameObjectManager.currentEnemyCount -= 1;
                Destroy(tmp_BulletImpact);
            }
            if (tmp_Hit.collider.CompareTag("Target"))
            {
                Debug.Log("Hit Target");
                tmp_Hit.collider.gameObject.SetActive(false);
                GameObjectManager.currentTargetCount -= 1;
                Destroy(tmp_BulletImpact);
            }

            //创建击中音效
            var tmp_TagsWithAudio =
                ImpactAudioData.ImpactTagWithAudios.Find((_audioData) => _audioData.Tag.Equals(tmp_Hit.collider.tag));
            if (tmp_TagsWithAudio == null) return;
            int tmp_Length = tmp_TagsWithAudio.ImpactAudioClips.Count;
            AudioClip tmp_AudioClip = tmp_TagsWithAudio.ImpactAudioClips[Random.Range(0, tmp_Length)];
            AudioSource.PlayClipAtPoint(tmp_AudioClip, tmp_Hit.point);
        }
    }
}
