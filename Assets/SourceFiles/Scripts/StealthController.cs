using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class StealthPower : MonoBehaviour
{
    [Header("Timers du Brouilleur")]
    public float dureeInvisibilite = 5f;
    public float tempsCooldown = 10f;

    private bool estDisponible = true;

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

    void Update()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            ToggleStealth();
        }
    }

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