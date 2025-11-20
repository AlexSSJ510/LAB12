using UnityEngine;

/// <summary>
/// Implementación concreta de IInteractable para un cofre de botín.
/// Solo se puede interactuar una vez para abrirlo.
/// </summary>
public class LootChestController : MonoBehaviour, IInteractable
{
    /// <summary>
    /// Indica si el cofre ha sido abierto.
    /// Es pública para que las pruebas de PlayMode puedan leer su estado.
    /// </summary>
    public bool IsOpened { get; private set; }

    public void Interact()
    {
        // Versión CORRECTA: solo verifica si ya estaba abierto.
        if (IsOpened)
        {
            Debug.Log("Este cofre ya ha sido abierto.");
            return;
        }

        IsOpened = true;
        Debug.Log("¡Has abierto el cofre y encontrado un tesoro!");
    }
}
