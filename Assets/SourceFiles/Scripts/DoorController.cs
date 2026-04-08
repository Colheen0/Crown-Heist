using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class DoorController : MonoBehaviour
{
    [Header("Réglages des Hauteurs")]
    public float yRepos = -2.5f;   // Position quand elle est FERMÉE
    public float yAction = 2.5f;  // Position quand elle se LÈVE (Ouverte)

    [Header("Timers du Pouvoir")]
    public float vitesse = 5f;
    public float dureePouvoir = 5f;    // Temps où elle reste levée
    public float tempsCooldown = 10f;  // Temps avant de pouvoir réutiliser

    private bool estDisponible = true;

    // Cette fonction est celle que ton OSC_manager doit appeler
    public void ToggleDoor()
    {
        if (estDisponible)
        {
            StartCoroutine(SequenceDuPouvoir());
        }
        else
        {
            Debug.Log("Pouvoir en recharge... Patiente !");
        }
    }

    void Update()
    {
        // Touche P pour tester
        if (Keyboard.current.pKey.wasPressedThisFrame)
        {
            ToggleDoor();
        }
    }

    private IEnumerator SequenceDuPouvoir()
    {
        // --- ÉTAPE 0 : VERROUILLAGE ---
        estDisponible = false; 

        // --- ÉTAPE 1 : LA PORTE SE LÈVE ---
        Debug.Log("Action lancée !");
        yield return StartCoroutine(BougerVers(yAction));

        // --- ÉTAPE 2 : ATTENTE (5 secondes) ---
        // La porte reste en haut
        yield return new WaitForSeconds(dureePouvoir);

        // --- ÉTAPE 3 : LA PORTE REDESCEND ---
        Debug.Log("Fin de l'effet, retour au repos...");
        yield return StartCoroutine(BougerVers(yRepos));

        // --- ÉTAPE 4 : COOLDOWN (10 secondes) ---
        // La porte est en bas, mais le bouton ne marche pas encore
        Debug.Log("Recharge du pouvoir...");
        yield return new WaitForSeconds(tempsCooldown);

        // --- ÉTAPE 5 : PRÊT ---
        estDisponible = true;
        Debug.Log("Pouvoir prêt à l'emploi !");
    }

    // Fonction de mouvement fluide point A vers point B
    private IEnumerator BougerVers(float cibleY)
    {
        while (Mathf.Abs(transform.localPosition.y - cibleY) > 0.01f)
        {
            Vector3 pos = transform.localPosition;
            pos.y = Mathf.MoveTowards(pos.y, cibleY, vitesse * Time.deltaTime);
            transform.localPosition = pos;
            yield return null;
        }
        
        // On force la position exacte à la fin
        transform.localPosition = new Vector3(transform.localPosition.x, cibleY, transform.localPosition.z);
    }
}