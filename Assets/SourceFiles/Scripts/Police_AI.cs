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
    private Animator _animator;
    private Transform _visualModel; 

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
           MessageManager.Instance.AfficherMessageGeneral("HALTE ! Le joueur est repéré !");
            _isChasing = true;               
            _targetPlayer = other.transform;
            _agent.speed = vitessePoursuite; 
        }
    }
    
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