using UnityEngine;
using UnityEngine.Video;

public class VictoryVideoHandler : MonoBehaviour
{

//ici on renseigne l'interface de fin 
    [Header("L'interface de fin")]
    public GameObject interfaceVictoire; 

    private VideoPlayer vPlayer;

//ici on fait en sorte que la vidéo se lance dès le début de la scène et que l'interface de fin soit désactivé au début de la scène 
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

//ici avec la fonction on fait en sorte que l'interface renseigné plus haut s'affiche à la fpin de la vidéo
    void OnVideoEnd(UnityEngine.Video.VideoPlayer vp)
    {
        Debug.Log("Vidéo terminée ! Affichage du menu.");
        
        if (interfaceVictoire != null)
        {
            interfaceVictoire.SetActive(true);
        }

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}