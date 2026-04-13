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
           MessageManager.Instance.AfficherMessage("HALTE ! Le joueur est repéré !");
            _isChasing = true;               
            _targetPlayer = other.transform;
            _agent.speed = vitessePoursuite; 
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
           MessageManager.Instance.AfficherMessage("On l'a perdu... Retour à la ronde.");
            _isChasing = false;              
            _targetPlayer = null;            
            _agent.speed = vitessePatrouille; 
            
            if (waypoints.Length > 0 && _agent.isOnNavMesh)
            {
                _agent.SetDestination(waypoints[_currentPointIndex].position);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            MessageManager.Instance.AfficherMessage("Le joueur a été attrapé !");
            
            GameOverManager manager = FindFirstObjectByType<GameOverManager>();
            
            if (manager != null)
            {
                manager.TriggerGameOver();
            }
        }
    }
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