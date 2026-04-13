using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using Unity.AI.Navigation; 

public class DoorController : MonoBehaviour
{
    [Header("Réglages des Hauteurs")]
    public float yRepos = -2f;   
    public float yAction = 1f;  

    [Header("Timers du Pouvoir")]
    public float vitesse = 5f;
    public float dureePouvoir = 5f;    
    public float tempsCooldown = 10f;  

    [Header("Le Lien NavMesh (Le pont invisible)")]
    public NavMeshLink lienNavMesh; 

    private bool estDisponible = true;

    void Start()
    {
        // CORRECTION : À -2f, la porte est baissée (ouverte). Le passage est libre.
        if (lienNavMesh != null) lienNavMesh.enabled = true;
    }

    public void ToggleDoor()
    {
        if (estDisponible)
        {
            StartCoroutine(SequenceDuPouvoir());
        }
        else
        {
            MessageManager.Instance.AfficherMessage("Pouvoir en recharge... Patiente !");
        }
    }

    void Update()
    {
        if (Keyboard.current.pKey.wasPressedThisFrame)
        {
            ToggleDoor();
        }
    }

    private IEnumerator SequenceDuPouvoir()
    {
        estDisponible = false; 

        // CORRECTION : La porte va se lever à 1f (fermée). On bloque l'IA immédiatement.
        if (lienNavMesh != null) lienNavMesh.enabled = false;

       MessageManager.Instance.AfficherMessage("Action lancée ! La porte se ferme.");
        yield return StartCoroutine(BougerVers(yAction));

        yield return new WaitForSeconds(dureePouvoir);

        MessageManager.Instance.AfficherMessage("Fin de l'effet, la porte se rouvre...");
        yield return StartCoroutine(BougerVers(yRepos));

        if (lienNavMesh != null) lienNavMesh.enabled = true;

        MessageManager.Instance.AfficherMessage("Recharge du pouvoir...");
        yield return new WaitForSeconds(tempsCooldown);

        estDisponible = true;
        MessageManager.Instance.AfficherMessage("Pouvoir prêt à l'emploi !");
    }

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