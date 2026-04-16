using UnityEngine;
using TMPro; 
using UnityEngine.SceneManagement; 

public class MessageManager : MonoBehaviour
{

//Ici on créer les 3 cases des messages du jeu qu'on doit remplir avec des éléments TextMesh dans l'éditeur 
    public static MessageManager Instance; 

    [Header("Les Écrans de Texte")]
    public TextMeshProUGUI texteGeneral;
    public TextMeshProUGUI textePorte;
    public TextMeshProUGUI texteBrouilleur;
    

//ici on créer les variables pour le nombre de bijoux avec un score actuel qui débute à 0 et un score max qui est à 8 
    [Header("Le Score")]
    public TextMeshProUGUI texteScore; 
    private int scoreActuel = 0;
    private int scoreMax = 8;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

//Ici on initialise les textes des types de messages du jeu
    void Start()
    {
        if (texteGeneral != null) texteGeneral.text = "";
        if (textePorte != null) textePorte.text = "[ PORTES : PRÊTES ]";
        if (texteBrouilleur != null) texteBrouilleur.text = "[ BROUILLEUR : PRÊT ]";
        
        MettreAJourScore();
    }

//ici on créer la fonction qui augmente la variable scroeActuel de 1 à chaque bijoux volé et elle vérifie que le score actuel égale ou non à la variable scroeMax pour changer la scène pour la scène de victoire
    public void AjouterUnBijou()
    {
        scoreActuel++; 
        MettreAJourScore();

        if (scoreActuel >= scoreMax)
        {
           
            SceneManager.LoadScene("Victoire"); 
        }
    }

//ici on créer le texte qui se met à jour en fonction des variables précédentes (la variable scoreActuel augmente avec la fonction d'avant ce qui rend le texte dynamique)
    private void MettreAJourScore()
    {
        if (texteScore != null)
        {
            texteScore.text = "BIJOUX VOLÉS : " + scoreActuel + " / " + scoreMax;
        }
    }

//ici on créer les fonctions pour afficher les messages dans les cases de texte du jeu

//ici les messages généraux 
    public void AfficherMessageGeneral(string message)
    {
        if (texteGeneral != null) texteGeneral.text = message;
    }

//ici les messages pour le pouvoir des portes
    public void AfficherMessagePorte(string message)
    {
        if (textePorte != null) textePorte.text = message;
    }

//et là les messages pour le pouvoir du brouilleur
    public void AfficherMessageBrouilleur(string message)
    {
        if (texteBrouilleur != null) texteBrouilleur.text = message;
    }
}