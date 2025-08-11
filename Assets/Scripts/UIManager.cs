using UnityEngine.SceneManagement;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject startPanel;
    public GameObject endPanel;
    public GameObject shootingSlider;

    void Start()
    {
        ShowStartUI();
        GameEvents.OnGameEnded += ShowEndUI;
    }

    public void ShowStartUI()
    {
        startPanel.SetActive(true);
        endPanel.SetActive(false);
        shootingSlider.SetActive(false);
    }

    public void StartGame()
    {
        startPanel.SetActive(false);
        shootingSlider.gameObject.SetActive(true);
        GameEvents.OnGameStarted?.Invoke();
    }

    public void ShowEndUI()
    {
        endPanel.SetActive(true);
        shootingSlider?.gameObject.SetActive(false);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
