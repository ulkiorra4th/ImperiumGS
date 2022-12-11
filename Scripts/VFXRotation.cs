using UnityEngine;

public class VFXRotation : MonoBehaviour
{
    [SerializeField] private float rotationScale = 0.01f;

    private void Update()
    {
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + rotationScale, transform.rotation.eulerAngles.z);
    }
}
