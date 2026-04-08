using UnityEngine;
using UnityEngine.AI;

public class PatrolAI : MonoBehaviour
{
    [Tooltip("Glisse ici tes 2 points de patrouille")]
    public Transform[] waypoints; 
    
    private NavMeshAgent _agent;
    private int _currentPointIndex = 0;

    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.autoBraking = false; // Mouvement fluide

        // SÉCURITÉ : Vérifie que le policier touche bien le NavMesh au démarrage
        if (_agent.isOnNavMesh)
        {
            if (waypoints.Length > 0)
            {
                _agent.SetDestination(waypoints[0].position);
            }
        }
        else
        {
            Debug.LogWarning(gameObject.name + " ne touche pas le NavMesh ! Descends-le un peu.");
        }
    }

    void Update()
    {
        // On ne cherche le point suivant que si l'agent est bien sur le sol
        if (_agent.isOnNavMesh)
        {
            if (!_agent.pathPending && _agent.remainingDistance <= 1f)
            {
                GoToNextPoint();
            }
        }
    }

    void GoToNextPoint()
    {
        if (waypoints.Length == 0 || !_agent.isOnNavMesh) return;

        // La magie opère ici : avec 2 points, ça fera 0, 1, 0, 1...
        _currentPointIndex = (_currentPointIndex + 1) % waypoints.Length;

        _agent.SetDestination(waypoints[_currentPointIndex].position);
    }
}