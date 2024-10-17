using UnityEngine;

public class PlayerColorSelection : MonoBehaviour
{
    public Color playerColour;
    private Material cachedMaterial;
    public MeshRenderer[] characterRenderers;
    
    public void AssignColours()
    {
        foreach (MeshRenderer meshRenderer in characterRenderers)
        {
            cachedMaterial = meshRenderer.material;
            cachedMaterial.color = playerColour;
        }
    }
    void OnDestroy()
    {
        if (cachedMaterial) { Destroy(cachedMaterial); }
    }
}