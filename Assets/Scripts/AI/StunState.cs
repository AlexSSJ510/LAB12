using UnityEngine;
using System.Collections;

public class StunState : AIState
{
    public StunState(AIController controller) : base(controller) { }

    public override void OnEnter()
    {
        Debug.Log("Entrando en estado: ATURDIDO");
        
        // Verificar que el controller no sea null
        if (controller == null)
        {
            Debug.LogError("Controller es null en StunState.OnEnter()");
            return;
        }

        if (controller.Agent != null && controller.Agent.isOnNavMesh)
        {
            controller.Agent.isStopped = true;
        }
        
        Renderer renderer = controller.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = Color.yellow;
        }
        
        controller.StartCoroutine(StunCoroutine());
    }

    public override void UpdateState()
    {
        // No hace nada mientras está aturdido
    }

    public override void OnExit()
    {
        Debug.Log("Saliendo de estado: ATURDIDO");
        
        if (controller == null) return;
        
        if (controller.Agent != null)
        {
            controller.Agent.isStopped = false;
        }
        
        Renderer renderer = controller.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = Color.white;
        }
    }

    private IEnumerator StunCoroutine()
    {
        if (controller == null)
        {
            Debug.LogError("Controller es null en StunCoroutine");
            yield break;
        }

        // Esperar la duración del aturdimiento
        yield return new WaitForSeconds(controller.StunDuration);
        
        // Verificar nuevamente que controller exista antes de cambiar estado
        if (controller != null)
        {
            controller.ChangeState(new PatrolState(controller));
        }
    }
}
