using UnityEngine;

/// <summary>
/// Estado de patrullaje: El enemigo sigue una ruta de waypoints
/// </summary>
public class PatrolState : AIState
{
    private int currentWaypointIndex = 0;

    public PatrolState(AIController controller) : base(controller) { }

    public override void OnEnter()
    {
        Debug.Log("Entrando en estado: PATRULLA");
        controller.Agent.speed = controller.PatrolSpeed;
        controller.Agent.isStopped = false;
        
        if (controller.Waypoints.Length > 0)
        {
            SetNextWaypoint();
        }
    }

    public override void UpdateState()
    {
        // Verificar si detectamos al jugador
        float distanceToPlayer = Vector3.Distance(
            controller.transform.position, 
            controller.Player.position
        );

        if (distanceToPlayer <= controller.DetectionRadius)
        {
            // Transición a persecución
            controller.ChangeState(new ChaseState(controller));
            return;
        }

        // Lógica de patrulla
        if (controller.Waypoints.Length == 0) return;

        // Verificar si llegamos al waypoint actual
        float distanceToWaypoint = Vector3.Distance(
            controller.transform.position,
            controller.Waypoints[currentWaypointIndex].position
        );

        if (distanceToWaypoint <= controller.WaypointTolerance)
        {
            // Avanzar al siguiente waypoint
            currentWaypointIndex = (currentWaypointIndex + 1) % controller.Waypoints.Length;
            SetNextWaypoint();
        }
    }

    public override void OnExit()
    {
        Debug.Log("Saliendo de estado: PATRULLA");
    }

    private void SetNextWaypoint()
    {
        controller.Agent.SetDestination(
            controller.Waypoints[currentWaypointIndex].position
        );
    }
}
