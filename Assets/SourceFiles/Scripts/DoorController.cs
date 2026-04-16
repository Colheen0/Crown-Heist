using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using Unity.AI.Navigation; 

public class DoorController : MonoBehaviour
{

//ici on renseigne les différentes stats de la portes pour pouvoir les modifier si besoin
    [Header("Réglages des Hauteurs")]
    public float yRepos = -2f;   
    public float yAction = 1f;  

    [Header("Timers du Pouvoir")]
    public float vitesse = 5f;
    public float dureePouvoir = 5f;    
    public float tempsCooldown = 10f;  

//ici on renseigne les navLinks de chaque duo de portes

    [Header("Le Lien NavMesh (Le pont invisible)")]
    public NavMeshLink lienNavMesh; 

//on créer une variable pour connaitre si le pouvoir est utilisable
    private bool estDisponible = true;

//
    void Start()
    {
        if (lienNavMesh != null) lienNavMesh.enabled = true;
    }
//ici on créer la fonction avec des if en fonction des utilisations si le pouvoir est dispo on lance l'action du pouvoir sinon on envoie juste un message général qui dit que le pouvoir est en charge
    public void ToggleDoor()
    {
        if (estDisponible)
        {
            StartCoroutine(SequenceDuPouvoir());
        }
        else
        {
            MessageManager.Instance.AfficherMessagePorte("Pouvoir en recharge... Patiente !");
        }
    }

//ici pour tester on fait en sorte d'activer le pouvoir de la porte avec la touche p 
    void Update()
    {
        if (Keyboard.current.pKey.wasPressedThisFrame)
        {
            ToggleDoor();
        }
    }

//ici la fonction qui lance le pouvoir elle agis en 3 temps d'abord elle mets la variable estDisponible en false etdésactive les navlink pour embecher les policier de passer ensuite elle baisse les porte dans la position fermer
//elle envoie un message general pour dire que le pouvoir est activé après elle lance un timer (renseigné en haut) et à la fin de ce timer elle envoie un message pour dire que le pouvoir est finis 
//puis elle remet les portes en position ouverte et elle réactive les navlinks après elle lance un second timer (le cooldwon aussi renseigné en haut) pendant ce timer un message s'envoie pour dore que le pouvoir est en recharge et à la fin de celui la elle remet la variable estDisponible en true et envoie un message : portes prêtes
    private IEnumerator SequenceDuPouvoir()
    {
        estDisponible = false; 

        if (lienNavMesh != null) lienNavMesh.enabled = false;

       MessageManager.Instance.AfficherMessagePorte("Action lancée ! La porte se ferme.");
        yield return StartCoroutine(BougerVers(yAction));

        yield return new WaitForSeconds(dureePouvoir);

        MessageManager.Instance.AfficherMessagePorte("Fin de l'effet, la porte se rouvre...");
        yield return StartCoroutine(BougerVers(yRepos));

        if (lienNavMesh != null) lienNavMesh.enabled = true;

        MessageManager.Instance.AfficherMessagePorte("Recharge du pouvoir...");
        yield return new WaitForSeconds(tempsCooldown);

        estDisponible = true;
        MessageManager.Instance.AfficherMessagePorte("Portes : prêtes");
    }

//ici la fonction qui fait le mouvement de la porte elle prend en paramètre la position y cible et elle bouge la porte vers cette position à la vitesse renseigné en haut
    private IEnumerator BougerVers(float cibleY)
    {
        while (Mathf.Abs(transform.localPosition.y - cibleY) > 0.01f)
        {
            Vector3 pos = transform.localPosition;
            pos.y = Mathf.MoveTowards(pos.y, cibleY, vitesse * Time.deltaTime);
            transform.localPosition = pos;
            yield return null;
        }
        
        transform.localPosition = new Vector3(transform.localPosition.x, cibleY, transform.localPosition.z);
    }
}