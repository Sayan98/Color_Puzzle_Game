using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject winPanel;

    [SerializeField] private AudioSource audioSource;

    [SerializeField] private AudioClip clickClip;
    [SerializeField] private AudioClip winClip;

    public void CheckResult(int targetScore)
    {
        if (targetScore != 0) return;
        PlayWinSound();
        winPanel.SetActive(true);
    }

    public void LoadNextMap() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    public void ReturnToMainMenu() => SceneManager.LoadScene(0);

    public void PlayClickSound() => audioSource.PlayOneShot(clickClip);

    private void PlayWinSound() => audioSource.PlayOneShot(winClip);
}
