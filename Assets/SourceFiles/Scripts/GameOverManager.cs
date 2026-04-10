using UnityEngine;
using UnityEngine.SceneManagement; // OBLIGATOIRE pour recharger les niveaux

public class GameOverManager : MonoBehaviour
{
    [Tooltip("Glisse le GameOverPanel ici")]
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