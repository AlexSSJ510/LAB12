using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ChangeColorOnHover : MonoBehaviour
{
    private MeshRenderer meshRenderer;
    private Color originalColor;

    void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        originalColor = meshRenderer.material.color;

        var interactable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable>();

        // Suscribimos eventos correctos
        interactable.hoverEntered.AddListener(OnHoverEntered);
        interactable.hoverExited.AddListener(OnHoverExited);
    }

    private void OnHoverEntered(HoverEnterEventArgs args)
    {
        meshRenderer.material.color = Color.yellow;
    }

    private void OnHoverExited(HoverExitEventArgs args)
    {
        meshRenderer.material.color = originalColor;
    }
}