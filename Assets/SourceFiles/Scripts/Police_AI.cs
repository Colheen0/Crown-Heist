using UnityEngine;
using UnityEngine.AI;

public class PoliceAI : MonoBehaviour
{

//ici on à les stats du policier pour pouvoir les modifier et changer la difficulté
    [Header("Patrouille")]
//le tableau de points (coordonnés) du policier pour qu'il fasse ses rondes
    public Transform[] waypoints;
//la vitesse de patrouille (la marche en gros)
    public float vitessePatrouille = 3f;
//le numéro du point de départ du point de patrouille du policier
    private int _currentPointIndex = 0;


    [Header("Poursuite")]

//la vitesse de poursuite quand il à repérer le joueur (quand il cours)
    public float vitessePoursuite = 6f;  
//la variable pour savoir si le policier est en train de poursuivre le joueur ou pas
    private Transform _targetPlayer;
    private bool _isChasing = false;    

//ici on créer les variables pour le navmeshagent (pour faire bouger le policier) et pour l'animator (pour faire les animations de marche et de course) et pour le model 3d du policier (pour faire tourner le model dans la direction ou il va)
    private NavMeshAgent _agent;
    private Animator _animator;
    private Transform _visualModel; 

//ici dans la fonction start on initialise les variables précédentes et on fait en sorte que le policier commence à patrouiller dès le début du jeu
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponentInChildren<Animator>();
        _visualModel = transform.GetChild(0); 
        
        _agent.autoBraking = false;
        _agent.speed = vitessePatrouille;
        _agent.updateRotation = false; 

        if (_agent.isOnNavMesh && waypoints.Length > 0)
        {
            _agent.SetDestination(waypoints[0].position);
        }
    }

    //ici dans la fonction update on fait en sorte que si le policier est en train de poursuivre le joueur il se dirige vers lui sinon il continue sa patrouille et il va au point suivant quand il arrive à destination et on fait aussi en sorte que le model 3d du policier tourne dans la direction ou il va et que les animations de marche et de course se lancent en fonction de la vitesse du policier (le model bugait donc on à rajouter le fait qu'il se tourne dans la bonne direction)

    void Update()
    {
        if (!_agent.isOnNavMesh) return;

        if (_isChasing && _targetPlayer != null)
        {
            _agent.SetDestination(_targetPlayer.position);
        }
        else
        {
            if (!_agent.pathPending && (_agent.remainingDistance <= 1.5f || !_agent.hasPath))
            {
                GoToNextPoint();
            }
        }
        
        if (_agent.velocity.magnitude > 0.1f)
        {
            Vector3 direction = _agent.velocity.normalized;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            _visualModel.rotation = Quaternion.Slerp(_visualModel.rotation, targetRotation, Time.deltaTime * 10f);
        }
        
        if (_animator != null)
        {
            bool isMoving = _agent.velocity.magnitude > 0.5f; // Seuil plus haut ⭐
            _animator.SetBool("isWalking", isMoving);
        }
    }

//ici la fonction qui fait aller le policier au point suivant du tableau de points de patrouille et qui fait en sorte que quand il arrive au dernier point il recommence au premier point du tableau (pour faire une boucle de patrouille)
    void GoToNextPoint()
    {
        if (waypoints.Length == 0) return;
        
        _currentPointIndex = (_currentPointIndex + 1) % waypoints.Length;
        _agent.SetDestination(waypoints[_currentPointIndex].position);
    }

//ici la fonction qui détecte si le joueur entre dans le champ de vision du policier (grâce à un collider trigger) et qui fait en sorte que le policier se mette à poursuivre le joueur et que sa vitesse de déplacement augmente 
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
           MessageManager.Instance.AfficherMessageGeneral("HALTE ! Le joueur est repéré !");
            _isChasing = true;               
            _targetPlayer = other.transform;
            _agent.speed = vitessePoursuite; 
        }
    }
    
//ici la fonction qui détecte si le joueur sort du champ de vision du policier (grâce à un collider trigger) et qui fait en sorte que le policier arrête de poursuivre le joueur et qu'il retourne à sa patrouille normale et que sa vitesse redevient normale
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            MessageManager.Instance.AfficherMessageGeneral("Le joueur a échappé aux flics !");
            _isChasing = false;              
            _targetPlayer = null;            
            _agent.speed = vitessePatrouille; 
            
            if (waypoints.Length > 0 && _agent.isOnNavMesh)
            {
                _agent.SetDestination(waypoints[_currentPointIndex].position);
            }
        }
    }

//ici la fonction qui détecte si le joueur entre en collision avec le policier et qui fait en sorte que le jeu se termine et que le message de game over s'affiche
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            MessageManager.Instance.AfficherMessageGeneral("Le joueur a été attrapé !");
            
            GameOverManager manager = FindFirstObjectByType<GameOverManager>();
            
            if (manager != null)
            {
                manager.TriggerGameOver();
            }
        }
    }
    
//ici la fonction qui fait que le policier oublie le joueur (quand le pouvoir du brouilleur est activé) et qu'il retourne à sa patrouille normale et que sa vitesse redevient normale
    public void Forget()
    {
        _isChasing = false;       
        _targetPlayer = null;          
        _agent.speed = vitessePatrouille; 
        
        if (waypoints.Length > 0 && _agent.isOnNavMesh)
        {
            _agent.SetDestination(waypoints[_currentPointIndex].position);
        }
    }
}