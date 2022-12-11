using UnityEngine;

public class FireLight : MonoBehaviour
{
    [SerializeField] private new Light light;
    [SerializeField] private float minIntensity;
    [SerializeField] private float maxIntensity;
    private void Update()
    {
        light.intensity = Random.Range(minIntensity, maxIntensity);
    }
}