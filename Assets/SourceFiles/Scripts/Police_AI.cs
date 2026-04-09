using UnityEngine;
using UnityEngine.AI;

public class PoliceAI : MonoBehaviour
{
    [Header("Patrouille")]
    public Transform[] waypoints;
    public float vitessePatrouille = 3f; // Vitesse de marche
    private int _currentPointIndex = 0;

    [Header("Poursuite")]
    public float vitessePoursuite = 6f;  // Vitesse de course quand il voit le joueur
    private Transform _targetPlayer;
    private bool _isChasing = false;     // L'état actuel du policier (Faux = Patrouille, Vrai = Poursuite)

    private NavMeshAgent _agent;

    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.autoBraking = false;
        _agent.speed = vitessePatrouille; // On commence doucement

        // Lancement de la patrouille
        if (_agent.isOnNavMesh && waypoints.Length > 0)
        {
            _agent.SetDestination(waypoints[0].position);
        }
    }

    void Update()
    {
        if (!_agent.isOnNavMesh) return;

        // --- LE CERVEAU DE L'IA ---
        if (_isChasing && _targetPlayer != null)
        {
            // ÉTAT 1 : POURSUITE
            // À chaque image, on met à jour la destination avec la position actuelle du joueur
            _agent.SetDestination(_targetPlayer.position);
        }
        else
        {
            // ÉTAT 2 : PATROUILLE
            // Le code que tu connais déjà : on passe de point en point
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

    // --- LE CAPTEUR DE DÉTECTION (Le Trigger) ---

    // Quand quelqu'un ENTRE dans la grande zone (Is Trigger)
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
}