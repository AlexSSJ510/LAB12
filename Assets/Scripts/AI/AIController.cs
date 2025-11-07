using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour, IInteractable, IDamageable
{
    [Header("Configuración de Patrulla")]
    [SerializeField] private Transform[] _waypoints;
    [SerializeField] private float _patrolSpeed = 2f;
    [SerializeField] private float _waypointTolerance = 1f;

    [Header("Configuración de Persecución")]
    [SerializeField] private float _chaseSpeed = 5f;
    [SerializeField] private float _detectionRadius = 10f;
    [SerializeField] private float _loseSightRadius = 15f;

    [Header("Configuración de Aturdimiento")]
    [SerializeField] private float _stunDuration = 3f; // IMPORTANTE: Esta variable debe existir

    [Header("Referencias")]
    [SerializeField] private Transform _player;

    [Header("Sistema de Vida")]
    [SerializeField] private float _maxHealth = 100f;

    [Header("Configuración de Ataque")]
    [SerializeField] private float _attackRange = 2f; // Distancia para atacar
    [SerializeField] private float _attackCooldown = 1.5f; // Tiempo entre ataques

    // Propiedades públicas
    public float AttackRange => _attackRange;
    public float AttackCooldown => _attackCooldown;

    private float _currentHealth;

    private NavMeshAgent agent;
    private AIState currentState;

    // PROPIEDADES PÚBLICAS - TODAS son necesarias
    public Transform[] Waypoints => _waypoints;
    public float PatrolSpeed => _patrolSpeed;
    public float ChaseSpeed => _chaseSpeed;
    public float DetectionRadius => _detectionRadius;
    public float LoseSightRadius => _loseSightRadius;
    public float WaypointTolerance => _waypointTolerance;
    public float StunDuration => _stunDuration; // IMPORTANTE: Esta propiedad debe existir
    public Transform Player => _player;
    public NavMeshAgent Agent => agent;
    // Implementación de IDamageable
    public Vector3 Position => transform.position;
    public bool IsAlive => _currentHealth > 0;




    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        
        if (agent == null)
        {
            Debug.LogError($"NavMeshAgent no encontrado en {gameObject.name}");
            return;
        }
        
        if (_player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                _player = playerObj.transform;
                Debug.Log($"Player encontrado: {_player.name}");
            }
            else
            {
                Debug.LogError("NO se encontró ningún GameObject con el tag 'Player'.");
            }
        }
    }

    private void Start()
    {
        // AÑADIR al principio del método Start()
        _currentHealth = _maxHealth;

        if (agent == null || _player == null)
        {
            Debug.LogError("Configuración incompleta.");
            enabled = false;
            return;
        }

        if (!agent.isOnNavMesh)
        {
            Debug.LogError($"El enemigo {gameObject.name} NO está sobre el NavMesh.");
            enabled = false;
            return;
        }

        if (_waypoints == null || _waypoints.Length == 0)
        {
            Debug.LogWarning($"No hay waypoints asignados a {gameObject.name}.");
        }

        ChangeState(new PatrolState(this));
    }

    private void Update()
    {
        currentState?.UpdateState();
    }

    public void ChangeState(AIState newState)
    {
        currentState?.OnExit();
        currentState = newState;
        currentState.OnEnter();
    }

    // Implementación de IInteractable
    public void Interact()
    {
        Stun();
    }

    public void Stun()
    {
        Debug.Log($"{gameObject.name} ha sido aturdido!");
        ChangeState(new StunState(this));
    }

    public void TakeDamage(float amount, string damageType)
    {
        if (!IsAlive) return; // Si ya está muerto, no hacer nada
        
        // Reducir la salud
        _currentHealth -= amount;
        Debug.Log($"{gameObject.name} recibió {amount} de daño tipo '{damageType}'. Salud restante: {_currentHealth}/{_maxHealth}");
        
        // Comportamiento según el tipo de daño
        switch (damageType.ToLower())
        {
            case "stun":
                // Daño de aturdimiento - solo aturde sin quitar mucha vida
                if (IsAlive)
                {
                    Stun();
                }
                break;
                
            case "fire":
                // Daño de fuego - podrías añadir efectos visuales aquí
                Debug.Log($"{gameObject.name} está ardiendo!");
                // Cambiar color a rojo temporalmente
                Renderer renderer = GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.material.color = Color.red;
                }
                break;
                
            case "physical":
                // Daño físico normal - solo reduce vida
                Debug.Log($"{gameObject.name} recibió daño físico!");
                break;
                
            default:
                Debug.LogWarning($"Tipo de daño desconocido: {damageType}");
                break;
        }
        
        // Verificar si murió
        if (!IsAlive)
        {
            Die();
        }
    }

    /// <summary>
    /// Método que se ejecuta cuando el enemigo muere
    /// </summary>
    private void Die()
    {
        Debug.Log($"{gameObject.name} ha muerto!");
        
        // Detener el agente
        if (agent != null)
        {
            agent.isStopped = true;
            agent.enabled = false;
        }
        
        // Desactivar el comportamiento de IA
        enabled = false;
        
        // Cambiar color a gris para indicar muerte
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = Color.gray;
        }
        
        // Destruir el objeto después de 2 segundos
        Destroy(gameObject, 2f);
    }
}
