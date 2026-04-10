using UnityEngine;
using UnityEngine.SceneManagement;

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