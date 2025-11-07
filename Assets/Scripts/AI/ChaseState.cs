using UnityEngine;

/// <summary>
/// Estado de persecución: El enemigo persigue al jugador
/// </summary>
public class ChaseState : AIState
{
    public ChaseState(AIController controller) : base(controller) { }

    public override void OnEnter()
    {
        Debug.Log("Entrando en estado: PERSECUCIÓN");
        controller.Agent.speed = controller.ChaseSpeed;
        controller.Agent.isStopped = false;
    }

    public override void UpdateState()
    {
        if (!controller.Agent.isOnNavMesh || !controller.Agent.enabled)
        {
            return;
        }

        if (controller.Player == null)
        {
            controller.ChangeState(new PatrolState(controller));
            return;
        }

        float distanceToPlayer = Vector3.Distance(
            controller.transform.position,
            controller.Player.position
        );

        // NUEVO: Si está muy cerca, cambiar a estado de ataque
        if (distanceToPlayer <= controller.AttackRange)
        {
            controller.ChangeState(new AttackState(controller));
            return;
        }

        // Si está muy lejos, volver a patrullar
        if (distanceToPlayer > controller.LoseSightRadius)
        {
            controller.ChangeState(new PatrolState(controller));
            return;
        }

        // Seguir persiguiendo
        if (controller.Agent.isOnNavMesh)
        {
            controller.Agent.SetDestination(controller.Player.position);
        }
    }


    public override void OnExit()
    {
        Debug.Log("Saliendo de estado: PERSECUCIÓN");
    }
}
