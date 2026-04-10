using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void LancerLeJeu()
    {
        SceneManager.LoadScene("Crown_Heist"); 
    }

    // Optionnel : Un bouton pour quitter le jeu (ne marche qu'une fois le jeu exporté)
    public void QuitterLeJeu()
    {
        Debug.Log("Le jeu se ferme !");
        Application.Quit();
    }
}