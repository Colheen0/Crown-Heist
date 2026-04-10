using UnityEngine;
using UnityEngine.SceneManagement; 
public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverUI;

    void Start()
    {
        if (gameOverUI != null) gameOverUI.SetActive(false);
    }

    public void TriggerGameOver()
    {
        gameOverUI.SetActive(true); 
        Time.timeScale = 0f;        
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void RecommencerNiveau()
    {
        Time.timeScale = 1f; 
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void RetourMenu()
    {
        Time.timeScale = 1f;
        
        SceneManager.LoadScene("MenuPrincipal"); 
    }
}