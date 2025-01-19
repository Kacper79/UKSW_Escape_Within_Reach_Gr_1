using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.AI
{
    /// <summary>
    /// This is a sctipt that controlls how NPC move and fight during a fighting tournament
    /// </summary>
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
                if(!startLock) StartCoroutine(TryToFight());
            }
        }

        /// <summary>
        /// Initialize the fightee' state
        /// </summary>
        /// <param name="playerTrans">transform of the prisoner to follow during fight</param>
        public void BeginFight(Transform playerTrans)
        {
            enemyTrans = playerTrans;
            isInFightingArena = true;
        }

        /// <summary>
        /// Rotates NPC body to face the player
        /// </summary>
        void TryToMove()
        {
            Vector3 moveDir = enemyTrans.position - transform.position;
            transform.rotation = Quaternion.LookRotation(moveDir, Vector3.up);
            if (moveDir.magnitude < enemyMoveMinDistance) return;
            transform.parent.position += enemyMoveSpeedCoeff * Time.deltaTime * moveDir.normalized;
            //Physics.SyncTransforms();
        }

        /// <summary>
        /// Ends the fight and kills the fightee
        /// </summary>
        public void EndFight()
        {
            isInFightingArena = false;
            Destroy(this.transform.parent.gameObject);
        }

        /// <summary>
        /// This is a coroutine that handles NPC making decisions during a fight.
        /// </summary>
        /// <returns></returns>
        public IEnumerator TryToFight()
        {
            startLock = true;
            float randTime = UnityEngine.Random.Range(reactionTimeWindowBegin, reactionTimeWindowEnd);
            System.Random randomAct = new();
            int randomAction = randomAct.Next(4);
            switch (randomAction)
            {
                case 0: //GUARD
                case 1:
                    selfEnemy.ChangeBlockValue();
                    break;
                case 2: //LIGHT
                    TryHittingSomeone(lightPunchDamage);
                    break;
                case 3: //HEAVY
                    TryHittingSomeone(heavyPunchDamage);
                    break;
            }
            Debug.Log("Trying to fight: " + randomAction + " for " + randTime + " seconds.");
            yield return new WaitForSecondsRealtime(randTime);
            if(randomAction == 0) selfEnemy.ChangeBlockValue();
            startLock = false;
        }

        /// <summary>
        /// This method is being used to check whether the NPC managed to attack the player
        /// </summary>
        /// <param name="damage"></param>
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

        /// <summary>
        /// Types of action that the NPC can make during fight
        /// </summary>
        public enum ActionType
        {
            GUARD, LIGHT, HEAVY
        }

        private bool isInFightingArena;
        private bool startLock = false;
        //public EnemyPunchedController epcRef;
        private Enemy selfEnemy;
        private Transform enemyTrans;

        /// <summary>
        /// The decisions made by NPC are not instantenous and take time (reaction time).
        /// That time is a random value from range.
        /// This variable is a lower limit/bound.
        /// </summary>
        public const float reactionTimeWindowBegin = 0.75f;
        /// <summary>
        /// The decisions made by NPC are not instantenous and take time (reaction time).
        /// That time is a random value from range.
        /// This variable is a higher limit/bound.
        /// </summary>
        public const float reactionTimeWindowEnd = 2.0f;
        /// <summary>
        /// How much HP points light attack punch takes from the player
        /// </summary>
        public int lightPunchDamage = 3;
        /// <summary>
        /// How much HP points heavy attack punch takes from the player
        /// </summary>
        public int heavyPunchDamage = 7;
        /// <summary>
        /// How fast NPC tries to move
        /// </summary>
        public const float enemyMoveSpeedCoeff = 0.9f;
        /// <summary>
        /// The minimum distance between NPC and the player so that NPC wouldn't try to hug the player's collider
        /// </summary>
        public const float enemyMoveMinDistance = 2.0f;

    }
}