using UnityEngine;

public class DestructibleBox : MonoBehaviour, IDamageable
{
    [SerializeField] private float maxHealth = 50f;
    private float currentHealth;

    public Vector3 Position => transform.position;
    public bool IsAlive => currentHealth > 0;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount, string damageType)
    {
        if (!IsAlive) return;
        
        currentHealth -= amount;
        Debug.Log($"Caja recibió {amount} de daño. Salud: {currentHealth}/{maxHealth}");
        
        // Cambiar color según la salud
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            float healthPercent = currentHealth / maxHealth;
            renderer.material.color = Color.Lerp(Color.red, Color.yellow, healthPercent);
        }
        
        if (!IsAlive)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("¡Caja destruida!");
        // Aquí podrías instanciar efectos de partículas, drop de items, etc.
        Destroy(gameObject);
    }
}
