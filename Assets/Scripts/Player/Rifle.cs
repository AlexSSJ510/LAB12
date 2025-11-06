using UnityEngine;

public class Rifle : MonoBehaviour
{
    public float range = 50f;
    public KeyCode fireKey = KeyCode.F;

    private void Update()
    {
        if (Input.GetKeyDown(fireKey))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.forward);

        if (Physics.Raycast(ray, out hit, range))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();
            if (interactable != null)
            {
                interactable.Interact();
                Debug.Log("¡Enemigo aturdido!");
            }
        }
    }
}
