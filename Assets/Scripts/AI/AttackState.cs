using UnityEngine;

/// <summary>
/// Estado de ataque: El enemigo ataca al jugador cuando está muy cerca
/// </summary>
public class AttackState : AIState
{
    private float attackCooldown;
    private float lastAttackTime = -999f;

    public AttackState(AIController controller) : base(controller) { }

    public override void OnEnter()
    {
        Debug.Log("Entrando en estado: ATAQUE");
        
        if (controller == null || controller.Agent == null) return;

        // Detener el movimiento cuando ataca
        if (controller.Agent.isOnNavMesh)
        {
            controller.Agent.isStopped = true;
        }
        
        // Cambiar color a rojo para indicar ataque
        Renderer renderer = controller.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = Color.red;
        }

        attackCooldown = controller.AttackCooldown;
    }

    public override void UpdateState()
    {
        if (controller == null || controller.Agent == null || controller.Player == null) return;

        // Calcular distancia al jugador
        float distanceToPlayer = Vector3.Distance(
            controller.transform.position,
            controller.Player.position
        );

        // Si el jugador está dentro del rango de ataque, atacar
        if (distanceToPlayer <= controller.AttackRange)
        {
            // Mirar hacia el jugador
            Vector3 direction = (controller.Player.position - controller.transform.position).normalized;
            direction.y = 0; // Mantener en el plano horizontal
            if (direction != Vector3.zero)
            {
                controller.transform.rotation = Quaternion.Slerp(
                    controller.transform.rotation,
                    Quaternion.LookRotation(direction),
                    Time.deltaTime * 5f
                );
            }

            // Atacar si ha pasado suficiente tiempo desde el último ataque
            if (Time.time >= lastAttackTime + attackCooldown)
            {
                Attack();
                lastAttackTime = Time.time;
            }
        }
        // Si el jugador sale del rango de ataque pero sigue en rango de persecución
        else if (distanceToPlayer > controller.AttackRange && distanceToPlayer <= controller.LoseSightRadius)
        {
            Debug.Log("Jugador fuera de rango de ataque, volviendo a perseguir");
            controller.ChangeState(new ChaseState(controller));
        }
        // Si el jugador está muy lejos, volver a patrullar
        else if (distanceToPlayer > controller.LoseSightRadius)
        {
            Debug.Log("Jugador muy lejos, volviendo a patrullar");
            controller.ChangeState(new PatrolState(controller));
        }
    }

    public override void OnExit()
    {
        Debug.Log("Saliendo de estado: ATAQUE");
        
        if (controller == null) return;

        // Reactivar el movimiento
        if (controller.Agent != null)
        {
            controller.Agent.isStopped = false;
        }
        
        // Restaurar color
        Renderer renderer = controller.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = Color.white;
        }
    }

    /// <summary>
    /// Ejecutar el ataque
    /// </summary>
    private void Attack()
    {
        Debug.Log($"¡{controller.gameObject.name} está ATACANDO al jugador!");
        
        // Aquí puedes añadir lógica de daño al jugador
        // Ejemplo: PlayerHealth playerHealth = controller.Player.GetComponent<PlayerHealth>();
        // if (playerHealth != null) playerHealth.TakeDamage(10f);
        
        // Efecto visual opcional: hacer que el enemigo "salte" un poco
        // (requiere Rigidbody en el enemigo)
    }
}
