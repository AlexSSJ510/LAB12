using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class StunState : AIState
{
    private float _stunDuration;

    public StunState(AIController controller, float stunDuration) : base(controller)
    {
        _stunDuration = stunDuration;
    }

    public override void OnEnter()
    {
        Debug.Log("Entrando en estado de Aturdimiento.");
        m_agent.isStopped = true;  // Detener el NavMeshAgent
        m_controller.StartCoroutine(StunDurationCoroutine());
    }

    private IEnumerator StunDurationCoroutine()
    {
        yield return new WaitForSeconds(_stunDuration);  // Esperar la duración del aturdimiento
        m_controller.ChangeState(new PatrolState(m_controller));  // Regresar al estado de patrullaje
    }

    public override void UpdateState() { /* No necesita lógica de actualización mientras está aturdido */ }

    public override void OnExit()
    {
        m_agent.isStopped = false;  // Asegurarse de que el agente pueda moverse después de salir del estado
    }
}
