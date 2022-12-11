using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Curse.AI;
using UnityEngine.UI;

namespace Curse
{
    public enum TypeOfCurse
    {
        damageDecrease,
        deceleration,
        bleeding
    }

    [RequireComponent(typeof(CursingObjectAI))]
    public class CursingObject : MonoBehaviour
    {
        public TypeOfCurse typeOfCurse;

        [Header("UI")]
        [SerializeField] private Image curseUI;
        [SerializeField] private float imageShowingSmoothness;

        [Header("Settings")]
        [SerializeField] private float radius;

        [SerializeField] private float cursePowerRatio;

        private float decelerationRatio;
        private float damageReductionRatio;
        private float damage;

        private CursingObjectAI ai;

        [Space(10)]
        [SerializeField] private bool isActive;

        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }

        private void Awake()
        {
            ai = GetComponent<CursingObjectAI>();

            decelerationRatio *= cursePowerRatio;
            damageReductionRatio *= cursePowerRatio;
            damage *= cursePowerRatio;
        }

        private void Update()
        {
            if (IsActive)
            {
                Collider[] colliders = GetCollidersInArea(radius);

                foreach (var collider in colliders)
                {
                    var character = collider.GetComponent<Character>();
                    if (character)
                    {
                        curseUI.gameObject.SetActive(true);
                        if (curseUI.gameObject.activeInHierarchy && curseUI.color.a == 0f) StartCoroutine(SmoothShowUICursingEffect());
                        character.SetSpeedRatio(decelerationRatio);
                        character.SetDamageRatio(damageReductionRatio);
                        character.SubHealth(0.01f);
                    }
                }
            }
        }

        private void OnDrawGizmos()
        {
            if (IsActive) Gizmos.color = Color.red;
            else Gizmos.color = Color.black;

            Gizmos.DrawWireSphere(transform.position, radius);
        }

        private Collider[] GetCollidersInArea(float areaRadius)
        {
            Collider[] overlappedColliders = Physics.OverlapSphere(transform.position, areaRadius);

            return overlappedColliders;
        }

        private IEnumerator SmoothShowUICursingEffect()
        {
            while (curseUI.color.a < 0.99f)
            {
                curseUI.color = new Color(1, 1, 1, Mathf.Lerp(curseUI.color.a, 1, imageShowingSmoothness * Time.deltaTime));
                yield return new WaitForEndOfFrame();
            }

            yield return null;
        }
    }
}