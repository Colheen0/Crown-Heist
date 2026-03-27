using UnityEngine;
using UnityEngine.AI;

public class PatrolAI : MonoBehaviour
{
    public Transform[] waypoints;
    private NavMeshAgent _agent;
    private int _currentPointIndex = 0;

    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();

        // CORRECTIF 1 : Désactive le freinage à l'approche d'un point. 
        // L'agent passera d'un point à l'autre de manière fluide sans s'arrêter.
        _agent.autoBraking = false;

        if (waypoints.Length > 0)
        {
            _agent.SetDestination(waypoints[0].position);
        }
    }

    void Update()
    {
        // CORRECTIF 2 : Vérification plus robuste.
        // On utilise la "stoppingDistance" définie dans l'Inspecteur plutôt qu'un chiffre fixe, 
        // ou on accepte qu'il soit à moins de 1 mètre du point.
        if (!_agent.pathPending && _agent.remainingDistance <= 1f)
        {
            GoToNextPoint();
        }
    }

    void GoToNextPoint()
    {
        if (waypoints.Length == 0) return;

        // C'EST CETTE LIGNE QUI FAIT LA BOUCLE :
        // Le "% waypoints.Length" force l'index à revenir à 0 quand il atteint la fin de la liste.
        // Exemple (0 -> 1 -> 2 -> 3 -> 0)
        _currentPointIndex = (_currentPointIndex + 1) % waypoints.Length;

        // On donne l'ordre d'aller au nouveau point
        _agent.SetDestination(waypoints[_currentPointIndex].position);
    }
}