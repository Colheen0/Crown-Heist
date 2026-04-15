using UnityEngine;
using TMPro; 
using UnityEngine.SceneManagement; 

public class MessageManager : MonoBehaviour
{
    public static MessageManager Instance; 

    [Header("Les Écrans de Texte")]
    public TextMeshProUGUI texteGeneral;
    public TextMeshProUGUI textePorte;
    public TextMeshProUGUI texteBrouilleur;
    
    [Header("Le Score")]
    public TextMeshProUGUI texteScore; 
    private int scoreActuel = 0;
    private int scoreMax = 8;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        if (texteGeneral != null) texteGeneral.text = "";
        if (textePorte != null) textePorte.text = "[ PORTES : PRÊTES ]";
        if (texteBrouilleur != null) texteBrouilleur.text = "[ BROUILLEUR : PRÊT ]";
        
        MettreAJourScore();
    }


    public void AjouterUnBijou()
    {
        scoreActuel++; 
        MettreAJourScore();

        if (scoreActuel >= scoreMax)
        {
           
            SceneManager.LoadScene("Victoire"); 
        }
    }

    private void MettreAJourScore()
    {
        if (texteScore != null)
        {
            texteScore.text = "BIJOUX VOLÉS : " + scoreActuel + " / " + scoreMax;
        }
    }


    public void AfficherMessageGeneral(string message)
    {
        if (texteGeneral != null) texteGeneral.text = message;
    }

    public void AfficherMessagePorte(string message)
    {
        if (textePorte != null) textePorte.text = message;
    }

    public void AfficherMessageBrouilleur(string message)
    {
        if (texteBrouilleur != null) texteBrouilleur.text = message;
    }
}