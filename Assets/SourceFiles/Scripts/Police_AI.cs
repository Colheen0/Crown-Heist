using UnityEngine;
using UnityEngine.AI;

public class PoliceAI : MonoBehaviour
{
    [Header("Patrouille")]
    public Transform[] waypoints;
    public float vitessePatrouille = 3f;
    private int _currentPointIndex = 0;

    [Header("Poursuite")]
    public float vitessePoursuite = 6f;  
    private Transform _targetPlayer;
    private bool _isChasing = false;    

    private NavMeshAgent _agent;

    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.autoBraking = false;
        _agent.speed = vitessePatrouille; 

        if (_agent.isOnNavMesh && waypoints.Length > 0)
        {
            _agent.SetDestination(waypoints[0].position);
        }
    }

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
    }

    void GoToNextPoint()
    {
        if (waypoints.Length == 0) return;
        
        _currentPointIndex = (_currentPointIndex + 1) % waypoints.Length;
        _agent.SetDestination(waypoints[_currentPointIndex].position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("HALTE ! Le joueur est repéré !");
            _isChasing = true;               // On change d'état
            _targetPlayer = other.transform; // On mémorise qui poursuivre
            _agent.speed = vitessePoursuite; // Le policier se met à courir !
        }
    }

    // Quand quelqu'un SORT de la grande zone
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("On l'a perdu... Retour à la ronde.");
            _isChasing = false;              // On repasse en patrouille
            _targetPlayer = null;            // On oublie le joueur
            _agent.speed = vitessePatrouille; // On se remet à marcher calmement
            
            // On lui redonne l'ordre d'aller à son point de patrouille actuel
            if (waypoints.Length > 0 && _agent.isOnNavMesh)
            {
                _agent.SetDestination(waypoints[_currentPointIndex].position);
            }
        }
    }

    // Quand le collider PHYSIQUE du policier (pas la bulle de vision) touche quelque chose
    private void OnCollisionEnter(Collision collision)
    {
        // Si la chose qu'il touche a le tag "Player"
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Le joueur a été attrapé !");
            
            // On cherche notre GameManager dans la scène
            GameOverManager manager = FindFirstObjectByType<GameOverManager>();
            
            // S'il le trouve, on lance la fonction fatale
            if (manager != null)
            {
                manager.TriggerGameOver();
            }
        }
    }
}