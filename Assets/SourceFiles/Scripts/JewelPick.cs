using UnityEngine;
using UnityEngine.InputSystem;

public class JewelPickup : MonoBehaviour
{
    [Tooltip("Glisse ici l'objet 3D du bijou (pour le faire disparaître)")]
    public GameObject bijouVisuel; 

    private bool _joueurDansZone = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _joueurDansZone = true;
            MessageManager.Instance.AfficherMessageGeneral("Appuyez sur 'R' pour voler le bijou");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _joueurDansZone = false;
            MessageManager.Instance.AfficherMessageGeneral(""); 
        }
    }

    void Update()
    {
        if (_joueurDansZone && Keyboard.current.rKey.wasPressedThisFrame)
        {
            VolerLeBijou();
        }
    }

    private void VolerLeBijou()
    {
        StarterAssets.ThirdPersonController joueur = FindFirstObjectByType<StarterAssets.ThirdPersonController>();
        if (joueur != null)
        {
            joueur.LancerAnimationVol();
        }

        MessageManager.Instance.AjouterUnBijou();
        MessageManager.Instance.AfficherMessageGeneral("Bijou récupéré !");
        
        if (bijouVisuel != null) bijouVisuel.SetActive(false);
        gameObject.SetActive(false); 
    }
}