using UnityEngine;

public class Rifle : MonoBehaviour
{
    [Header("Configuración del Rifle")]
    public float range = 50f;
    public float fireRate = 0.5f;
    public KeyCode shootKey = KeyCode.F; // Tecla para disparar (presiona F)
    
    private Camera playerCamera;
    private float nextFireTime = 0f;

    private void Start()
    {
        playerCamera = Camera.main;
        
        if (playerCamera == null)
        {
            Debug.LogError("No se encontró la cámara principal. Asegúrate de que la cámara tenga el tag 'MainCamera'.");
        }
    }

    private void Update()
    {
        // Usar Input.GetKeyDown (sistema antiguo de Unity - siempre funciona)
        if (Input.GetKeyDown(shootKey) && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    private void Shoot()
    {
        if (playerCamera == null) return;

        Vector3 rayOrigin = playerCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f));
        Vector3 rayDirection = playerCamera.transform.forward;
        
        RaycastHit hit;

        if (Physics.Raycast(rayOrigin, rayDirection, out hit, range))
        {
            Debug.Log($"Raycast golpeó: {hit.collider.gameObject.name}");

            // PRIORIDAD 1: Intentar usar IDamageable (sistema avanzado)
            IDamageable damageable = hit.collider.GetComponent<IDamageable>();
            if (damageable != null && damageable.IsAlive)
            {
                // Aplicar daño de tipo "stun" (10 puntos de daño)
                damageable.TakeDamage(10f, "stun");
                Debug.Log($"¡{hit.collider.gameObject.name} dañado! (Sistema IDamageable)");
                return; // Salir después de aplicar daño
            }

            // FALLBACK: Usar IInteractable (compatibilidad con versión anterior)
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();
            if (interactable != null)
            {
                interactable.Interact();
                Debug.Log("¡Enemigo aturdido! (Sistema IInteractable)");
                return;
            }

            Debug.Log("El objeto no es dañable ni interactuable.");
        }
        else
        {
            Debug.Log("El disparo no golpeó nada.");
        }

        Debug.DrawRay(rayOrigin, rayDirection * range, Color.red, 1f);
    }

}
