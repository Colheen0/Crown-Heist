using UnityEngine;
using UnityEngine.Video;

public class VictoryVideoHandler : MonoBehaviour
{
    [Header("L'interface de fin")]
    [Tooltip("Glisse ici le panneau ou le bouton à afficher à la fin de la vidéo")]
    public GameObject interfaceVictoire; 

    private VideoPlayer vPlayer;

    void Awake()
    {
        vPlayer = GetComponent<VideoPlayer>();
        
        if (interfaceVictoire != null)
        {
            interfaceVictoire.SetActive(false);
        }

        vPlayer.loopPointReached += OnVideoEnd;
        
        vPlayer.Play();
    }

    void OnVideoEnd(UnityEngine.Video.VideoPlayer vp)
    {
        Debug.Log("Vidéo terminée ! Affichage du menu.");
        
        if (interfaceVictoire != null)
        {
            interfaceVictoire.SetActive(true);
        }

        // --- LA MAGIE OPÈRE ICI ---
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}