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
            MessageManager.Instance.AfficherMessage("Brouilleur en recharge... Patiente !");
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
        MessageManager.Instance.AfficherMessage("BROUILLEUR ACTIVÉ ! Les flics sont aveugles.");

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

        MessageManager.Instance.AfficherMessage("Fin du brouilleur. Ils retrouvent la vue !");
        
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

        // 5. Temps de recharge
        MessageManager.Instance.AfficherMessage("Recharge du brouilleur...");
        yield return new WaitForSeconds(tempsCooldown);

        estDisponible = true;
        MessageManager.Instance.AfficherMessage("Brouilleur prêt à l'emploi !");
    }
}