using UnityEngine;
using UnityEngine.UI;


public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private Character character;

    [Tooltip("Image to show the Health Bar, should be set as Fill, the script modifies fillAmount")]
    [SerializeField] private Image image;

    private float smoothness = 0.02f;

    private void Update()
    {
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        image.fillAmount = Mathf.Lerp(image.fillAmount, character.Health / 100, smoothness);
    }
}
