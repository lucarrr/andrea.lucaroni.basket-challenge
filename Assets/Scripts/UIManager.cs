using UnityEngine.SceneManagement;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject startPanel;
    public GameObject endPanel;
    public GameObject shootingSlider;
    public TMPro.TextMeshProUGUI scoreText;

    void Start()
    {
        ShowStartUI();
        GameEvents.OnGameEnded += ShowEndUI;
        ScoreManager.Instance.OnScoreChanged += UpdateScoreUI;
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
        endPanel.SetActive(false);
        shootingSlider.gameObject.SetActive(true);
        GameEvents.OnGameStarted?.Invoke();
    }

    public void ShowEndUI()
    {
        endPanel.SetActive(true);
        shootingSlider?.gameObject.SetActive(false);
    }

    void UpdateScoreUI(int newScore)
    {
        scoreText.text = newScore.ToString();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
