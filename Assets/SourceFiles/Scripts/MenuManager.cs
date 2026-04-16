using UnityEngine;
using UnityEngine.SceneManagement;

//ici on gère les fonctions du menu principal (lancer le jeu et quitter le jeu)
public class MainMenuManager : MonoBehaviour
{
    public void LancerLeJeu()
    {
        SceneManager.LoadScene("Crown_Heist"); 
    }
    public void QuitterLeJeu()
    {
        Debug.Log("Le jeu se ferme !");
        Application.Quit();
    }
}