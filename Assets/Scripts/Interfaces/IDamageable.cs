using UnityEngine;

/// <summary>
/// Interfaz para objetos que pueden recibir daño
/// Permite múltiples tipos de daño: Stun, Fire, Physical, etc.
/// </summary>
public interface IDamageable
{
    /// <summary>
    /// Método para aplicar daño a este objeto
    /// </summary>
    /// <param name="amount">Cantidad de daño a aplicar</param>
    /// <param name="damageType">Tipo de daño (ejemplo: "stun", "fire", "physical")</param>
    void TakeDamage(float amount, string damageType);
    
    /// <summary>
    /// Obtener la posición de este objeto
    /// </summary>
    Vector3 Position { get; }
    
    /// <summary>
    /// Verificar si el objeto sigue vivo/activo
    /// </summary>
    bool IsAlive { get; }
}
