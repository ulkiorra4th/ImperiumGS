using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;

    public bool IsPaused { get; private set; }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) IsPaused = !IsPaused;
        if (IsPaused)
        {
            pausePanel.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            pausePanel.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void OnMainMenuButtonClicked()
    {
        SceneManager.LoadScene(0);
    }
}
