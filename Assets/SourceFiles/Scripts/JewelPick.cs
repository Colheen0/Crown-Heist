using UnityEngine;
using UnityEngine.InputSystem;

public class JewelPickup : MonoBehaviour
{

//ici on renseigne le model 3d du bijou 
    [Tooltip("objet 3D du bijou")]
    public GameObject bijouVisuel; 

//ici le son du ramassage 
    [Header("Sons")]
    [Tooltip("audio du ramassage")]
    public AudioClip sonRamassage; 

//ici on dit que le joueur n'est par défaut pas dans la zone de ramassage
    private bool _joueurDansZone = false;

//ici on créer une fonction qui si il détecte le joueur dans la zone lance un message general pour dire au joueur de récupérer le bijou avec le bouton R
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _joueurDansZone = true;
            MessageManager.Instance.AfficherMessageGeneral("Appuyez sur 'R' pour voler le bijou");
        }
    }

//ici si le joueur est hors de la zone de ramassage le message général n'affiche plus rien

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _joueurDansZone = false;
            MessageManager.Instance.AfficherMessageGeneral(""); 
        }
    }

//ici on créer la fonction qui fait que quand on appuie sur la touche r ça active la fonction volerLeBijou
    void Update()
    {
        if (_joueurDansZone && Keyboard.current.rKey.wasPressedThisFrame)
        {
            VolerLeBijou();
        }
    }

//ici dans cette fonction on fait en sorte que lorsque le joueur récupère le bijou le model 3d renseigner avant disparait et le son de ramassage se lance et qu'un message général s'affiche pour dire que le joueur à récupérer le bijou (ça devait aussi lancer une animation mais on à pas eu le temps :( )
    private void VolerLeBijou()
    {
        StarterAssets.ThirdPersonController joueur = FindFirstObjectByType<StarterAssets.ThirdPersonController>();
        if (joueur != null)
        {
            joueur.LancerAnimationVol();
        }

        MessageManager.Instance.AjouterUnBijou();
        MessageManager.Instance.AfficherMessageGeneral("Bijou récupéré !");
        
        if (sonRamassage != null)
        {
            AudioSource.PlayClipAtPoint(sonRamassage, transform.position);
        }

        if (bijouVisuel != null) 
        {
            bijouVisuel.SetActive(false);
        }
        
        gameObject.SetActive(false); 
    }
}