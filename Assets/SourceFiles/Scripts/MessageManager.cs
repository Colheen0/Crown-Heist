using UnityEngine;
using TMPro; // Obligatoire pour utiliser TextMeshPro
using System.Collections;

public class MessageManager : MonoBehaviour
{
    public static MessageManager Instance; 

    [Header("Interface")]
    public TextMeshProUGUI texteNotification; 

    [Header("Réglages")]
    public float tempsAffichage = 3f; 
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        if (texteNotification != null) texteNotification.text = "";
    }

    public void AfficherMessage(string nouveauMessage)
    {
        if (texteNotification == null) return;

        StopAllCoroutines(); 
        StartCoroutine(SequenceAffichage(nouveauMessage));
    }

    private IEnumerator SequenceAffichage(string message)
    {
        texteNotification.text = message; 
        yield return new WaitForSeconds(tempsAffichage); 
        texteNotification.text = ""; 
    }
}