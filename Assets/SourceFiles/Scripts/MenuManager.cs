using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    // Cette fonction sera appelée quand on cliquera sur "Commencer"
    public void LancerLeJeu()
    {
        // On charge la scène de ton niveau.
        // REMPLACE "Prototype_1" par le vrai nom de la scène de ton jeu !
        SceneManager.LoadScene("Prototype_1"); 
    }

    // Optionnel : Un bouton pour quitter le jeu (ne marche qu'une fois le jeu exporté)
    public void QuitterLeJeu()
    {
        Debug.Log("Le jeu se ferme !");
        Application.Quit();
    }
}