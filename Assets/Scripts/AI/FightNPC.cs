using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.AI
{
    public class FightNPC : MonoBehaviour
    {
        void Start()
        {
            selfEnemy = transform.parent.gameObject.GetComponent<Enemy>();
        }

        void Update()
        {
            if (isInFightingArena)
            {
                TryToMove();
                StartCoroutine(TryToFight());
            }
        }

        public void BeginFight(Transform playerTrans)
        {
            enemyTrans = playerTrans;
            isInFightingArena = true;
        }

        void TryToMove()
        {
            Vector3 moveDir = enemyTrans.position - transform.position;
            transform.rotation = Quaternion.LookRotation(moveDir, Vector3.up);
            if (moveDir.magnitude < enemyMoveMinDistance) return;
            transform.parent.position += enemyMoveSpeedCoeff * Time.deltaTime * moveDir.normalized;
            //Physics.SyncTransforms();
        }

        public void EndFight()
        {
            isInFightingArena = false;
        }

        public IEnumerator TryToFight()
        {
            float randTime = UnityEngine.Random.Range(reactionTimeWindowBegin, reactionTimeWindowEnd);
            System.Random randomAct = new();
            int randomAction = randomAct.Next(Enum.GetNames(typeof(ActionType)).Length);
            switch (randomAction)
            {
                case 0: //GUARD
                    selfEnemy.ChangeBlockValue();
                    break;
                case 1: //LIGHT
                    TryHittingSomeone(lightPunchDamage);
                    break;
                case 2: //HEAVY
                    TryHittingSomeone(heavyPunchDamage);
                    break;
            }
            yield return new WaitForSecondsRealtime(randTime);
            if(randomAction == 0) selfEnemy.ChangeBlockValue();
        }

        private void TryHittingSomeone(int damage)
        {
            //punchable_targets_detector.TryDamagingEnemies(damage);
            RaycastHit[] rayHits = Physics.RaycastAll(transform.position, transform.forward, 2.1f);
            foreach(RaycastHit rhit in rayHits)
            {
                if(rhit.collider.gameObject.CompareTag("Player"))
                {
                    Debug.Log("Attakced playeer");
                    rhit.collider.gameObject.GetComponent<IAttackable>().Punched(damage, transform.position);
                    return;
                }
            }
        }

        public enum ActionType
        {
            GUARD, LIGHT, HEAVY
        }

        private bool isInFightingArena;
        //public EnemyPunchedController epcRef;
        private Enemy selfEnemy;
        private Transform enemyTrans;
        public PunchableTargetsDetector punchable_targets_detector;

        public const float reactionTimeWindowBegin = 0.75f;
        public const float reactionTimeWindowEnd = 2.0f;
        public int lightPunchDamage = 7;
        public int heavyPunchDamage = 14;
        public const float enemyMoveSpeedCoeff = 0.9f;
        public const float enemyMoveMinDistance = 2.0f;

    }
}