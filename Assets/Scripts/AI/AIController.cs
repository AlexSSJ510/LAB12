using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Controlador de IA para manejar el comportamiento del enemigo, incluyendo el aturdimiento.
/// </summary>
public class AIController : MonoBehaviour, IInteractable
{
    [Header("AI Settings")]
    public Transform[] waypoints;
    public float patrolSpeed = 2f;
    public float chaseSpeed = 2f;
    public float detectionRadius = 2f;
    public float loseSightRadius = 2f;

    [Header("Stun Settings")]
    public float stunDuration = 3f;

    private AIState _currentState;
    private NavMeshAgent _agent;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        ChangeState(new PatrolState(this));
    }

    private void Update()
    {
        _currentState?.UpdateState();
    }

    public void ChangeState(AIState newState)
    {
        _currentState?.OnExit();
        _currentState = newState;
        _currentState.OnEnter();
    }

    public void Interact()
    {
        Stun();
    }

    public void Stun()
    {
        ChangeState(new StunState(this, stunDuration));
    }
}
