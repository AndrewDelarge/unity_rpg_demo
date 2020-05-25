
using Actors.Base.Interface;
using Actors.Base.StatsStuff;
using Animation;
using UnityEditor;
using UnityEngine;

namespace Actors.Base
{
    [RequireComponent(typeof(Stats))]
    public abstract class CommonAnimator : BaseAnimator
    {
        public AnimationClip replaceableAttackClip;
        public AnimationClip[] defaultAttackAnimSet;
        public string attackLayerName = "Attack";
        public string attackInRunLayerName = "AttackInRun";


        protected Combat combat;
        protected IControlable movement;
        protected Stats stats;
        protected const float locomotionAnimationSmoothTime = .1f;
        protected AnimationClip[] currentAttackAnimSet;

        [Header("Pseudo IK")] 
        public bool isLookAtEnabled = false;
        public Transform lookPoint; 
        public Transform neckBone; 
        public Transform chestBone; 
        public Transform neckPos;
        [HideInInspector] 
        public float excessAngle;

        private float angleChestHorz;
        private float angleNeckVert;
        
        private void Awake()
        {
            enabled = false;
        }

        public virtual void Init(Combat actCombat, IControlable actMovement, Stats actStats)
        {
            base.Init();
            combat = actCombat;
            movement = actMovement;
            stats = actStats;
            currentAttackAnimSet = defaultAttackAnimSet;
            combat.OnAttack += OnAttack;
            combat.OnAttackEnd += OnAttackEnd;
            stats.onGetDamage += OnGetHit;
            enabled = true;
        }

        private void LateUpdate()
        {
            if (!isLookAtEnabled)
            {
                return;
            }

            if (lookPoint == null || stats.IsDead())
            {
                return;
            }

            if (movement.target != null)
            {
                Vector3 position = movement.target.position;
                lookPoint.transform.position = new Vector3(position.x,
                    position.y +1, position.z);
            }
            
            
            CalculateBodyRotation(lookPoint);
        }
        
        
        
        void Update()
        {
            float speedPercent = movement.GetCurrentMagnitude() / stats.GetMovementSpeed();
            animator.SetFloat("speedPercent", speedPercent, locomotionAnimationSmoothTime, Time.deltaTime);
            animator.SetFloat("movementMultiplier", movement.GetSpeedMultiplier());

            ToggleAttackLayers(speedPercent);
            animator.SetBool("inCombat", combat.InCombat());
        }

        public void Disable()
        {
            animator.enabled = false;
        }

        public void Enable()
        {
            animator.enabled = true;
        }

        protected virtual void OnAttack()
        {
            animator.SetTrigger("attack");
            
            int animIndex = GetCurrentAttackAnimationIndex();

            overrideController[replaceableAttackClip.name] = currentAttackAnimSet[animIndex];
        }

        protected virtual void OnAttackEnd()
        {
        }

        protected virtual void OnGetHit(Damage damage)
        {
            animator.SetTrigger("getHit");
        }

        protected int GetCurrentAttackAnimationIndex()
        {
            int animIndex = combat.GetCurrentSuccessAttack();

            if (animIndex < currentAttackAnimSet.Length)
            {
                return animIndex;
            }
            
            return Random.Range(0, currentAttackAnimSet.Length);
        }


        public void Trigger(string triggerName)
        {
            animator.SetTrigger(triggerName);
        }

        void ToggleAttackLayers(float speed)
        {
            int index = animator.GetLayerIndex(attackLayerName);
            int indexRun = animator.GetLayerIndex(attackInRunLayerName);

            if (indexRun == -1 || index == -1)
            {
                return;
            }

            if (speed > 0)
            {
                animator.SetLayerWeight(index, 0f);
                animator.SetLayerWeight(indexRun, 1f);
                return;
            }
            
            animator.SetLayerWeight(index, 1f);
            animator.SetLayerWeight(indexRun, 0f);
        }


        protected virtual void CalculateBodyRotation(Transform lookAt)
        {
            Vector3 localPosition = lookAt.localPosition;
            Vector3 lookPos = new Vector3(localPosition.x, 0, localPosition.z);
            Vector3 lookPosVert = new Vector3(0, localPosition.y, 1);
            
            Vector3 neckBoneLocalPosition = new Vector3(0, neckPos.localPosition.y, 1);
            
            angleChestHorz = Vector3.SignedAngle(Vector3.forward,lookPos, Vector3.up);
            angleNeckVert = Vector3.SignedAngle(neckBoneLocalPosition, lookPosVert, Vector3.right);

            excessAngle = angleChestHorz > 0 ? Mathf.Max(angleChestHorz - 40, 0) : Mathf.Min(angleChestHorz + 40, 0);
            
            //Max chest rotation angle
            angleChestHorz = angleChestHorz > 0 ? Mathf.Min(angleChestHorz, 40f) : Mathf.Max(angleChestHorz, -40f);
            //Max neck rotation angle
            angleNeckVert = angleNeckVert > 0 ? Mathf.Min(angleNeckVert, 20f) : Mathf.Max(angleNeckVert, -20f);
            
            chestBone.Rotate(0, angleChestHorz, 0);
            neckBone.Rotate(angleNeckVert, 0, 0);
        }

        private void RotateBody()
        {
            chestBone.Rotate(0, angleChestHorz, 0);
            neckBone.Rotate(angleNeckVert, 0, 0);
        }
    }
    
    
}