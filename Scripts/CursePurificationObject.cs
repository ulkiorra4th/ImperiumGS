using UnityEngine;

namespace Curse
{
    public class CursePurificationObject : MonoBehaviour
    {
        [SerializeField] private CursingObject cursingObject;

        private void OnTriggerEnter(Collider other)
        {
            Use();
            gameObject.SetActive(false);
        }
        private void Use()
        {
            cursingObject.IsActive = false;
        }
    }
}