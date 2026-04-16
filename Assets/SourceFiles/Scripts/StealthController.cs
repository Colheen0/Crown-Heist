using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class StealthPower : MonoBehaviour
{
//ici on renseigne les stats du brouilleur pour les modifier si besoin avec la durée et le cooldown
    [Header("Timers du Brouilleur")]
    public float dureeInvisibilite = 5f;
    public float tempsCooldown = 10f;

//comme pour les porte on fait une variable estDisponible
    private bool estDisponible = true;

//ici on créer la fonction avec des if en fonction des utilisations si le pouvoir est dispo on lance l'action du pouvoir sinon on envoie juste un message général qui dit que le pouvoir est en charge
    public void ToggleStealth()
    {
        if (estDisponible)
        {
            StartCoroutine(SequenceStealth());
        }
        else
        {
            MessageManager.Instance.AfficherMessageBrouilleur("Brouilleur en recharge... Patiente !");
        }
    }

//ici pour tester on fait en sorte d'activer le pouvoir du brouilleur avec la touche e 
    void Update()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            ToggleStealth();
        }
    }

//ici la fonction qui lance le pouvoir elle agis en 3 temps d'abord elle mets la variable estDisponible en false et envoie un message pour dire que le pouvoir est activé ensuite elle récupère tous les policiers dans la scène et pour chacun d'eux elle lance la fonction forget 
//pour qu'ils oublient le joueur et elle désactive les colliders trigger de chaque policier pour les rendre aveugle après elle lance un timer (renseigné en haut) et à la fin de ce timer elle envoie un message pour dire que le pouvoir est finis puis elle réactive les colliders trigger de chaque policier pour qu'ils puissent 
//voir à nouveau le joueur après elle lance un second timer (le cooldwon aussi renseigné en haut) pendant ce timer un message s'envoie pour dore que le pouvoir est en recharge et à la fin de celui la elle remet la variable estDisponible en true et envoie un message : brouilleur prêt
    private IEnumerator SequenceStealth()
    {
        estDisponible = false;
        MessageManager.Instance.AfficherMessageBrouilleur("BROUILLEUR ACTIVÉ ! Les flics sont aveugles.");

        PoliceAI[] policiers = FindObjectsByType<PoliceAI>(FindObjectsSortMode.None);
        
        foreach (PoliceAI flic in policiers)
        {
            flic.Forget();
            Collider[] colliders = flic.GetComponents<Collider>();
            foreach (Collider col in colliders)
            {
                if (col.isTrigger) 
                {
                    col.enabled = false;
                }
            }
        }

        yield return new WaitForSeconds(dureeInvisibilite);

        MessageManager.Instance.AfficherMessageBrouilleur("Fin du brouilleur. Ils retrouvent la vue !");
        
        foreach (PoliceAI flic in policiers)
        {
            Collider[] colliders = flic.GetComponents<Collider>();
            foreach (Collider col in colliders)
            {
                if (col.isTrigger)
                {
                    col.enabled = true;
                }
            }
        }

        MessageManager.Instance.AfficherMessageBrouilleur("Recharge du pouvoir...");
        yield return new WaitForSeconds(tempsCooldown);

        estDisponible = true;
        MessageManager.Instance.AfficherMessageBrouilleur("Brouilleur : prêt");
    }
}