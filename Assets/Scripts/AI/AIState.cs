using UnityEngine;

/// <summary>
/// Clase base abstracta que define el contrato para todos los estados de IA
/// </summary>
public abstract class AIState
{
    protected AIController controller;

    public AIState(AIController controller)
    {
        this.controller = controller;
    }

    // Métodos abstractos que cada estado concreto debe implementar
    public abstract void OnEnter();
    public abstract void UpdateState();
    public abstract void OnExit();
}
