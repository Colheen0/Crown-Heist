using UnityEngine;
using UnityEngine.SceneManagement; 
public class GameOverManager : MonoBehaviour
{
//ici on renseigne l'interface de game over 
    public GameObject gameOverUI;

//ici on fait en sorte que l'interface de game over soit désactivé au début de la scène
    void Start()
    {
        if (gameOverUI != null) gameOverUI.SetActive(false);
    }

//ici on créer la fonction qui fait que quand le joueur est attrapé par un policier le jeu se termine et que le message de game over s'affiche et que le curseur de la souris soit visible pour pouvoir cliquer sur les boutons du menu de game over
    public void TriggerGameOver()
    {
        gameOverUI.SetActive(true); 
        Time.timeScale = 0f;        
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

//ici on créer la fonction qui fait recommencer le niveau en rechargent la scene actuelle (il faut l'assigner au bouton de l'UI)
    public void RecommencerNiveau()
    {
        Time.timeScale = 1f; 
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
//ici on créer la fonction qui fait retourner au menu principal en chargeant la scène du menu principal (il faut l'assigner au bouton de l'UI aussi)
    public void RetourMenu()
    {
        Time.timeScale = 1f;
        
        SceneManager.LoadScene("MenuPrincipal"); 
    }
}