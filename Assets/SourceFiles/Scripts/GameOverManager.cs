using UnityEngine;
using UnityEngine.SceneManagement; // OBLIGATOIRE pour recharger les niveaux

public class GameOverManager : MonoBehaviour
{
    [Tooltip("Glisse le GameOverPanel ici")]
    public GameObject gameOverUI;

    void Start()
    {
        // On s'assure que l'écran est caché au début du jeu
        if (gameOverUI != null) gameOverUI.SetActive(false);
    }

    // Cette fonction sera appelée par le policier
    public void TriggerGameOver()
    {
        gameOverUI.SetActive(true); // Affiche le menu
        Time.timeScale = 0f;        // 🛑 MET LE JEU EN PAUSE (arrête tout le monde)
        
        // Débloque et affiche la souris pour pouvoir cliquer sur les boutons
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Fonction pour le bouton "Recommencer"
    public void RecommencerNiveau()
    {
        Time.timeScale = 1f; // 🟢 Retire la pause avant de recharger !
        
        // Recharge la scène actuellement ouverte
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Fonction pour le bouton "Menu Principal"
    public void RetourMenu()
    {
        Time.timeScale = 1f; // Retire la pause
        
        // Charge la scène du menu. IMPORTANT : il faut que ta scène s'appelle exactement "MainMenu" !
        SceneManager.LoadScene("MenuPrincipal"); 
    }
}