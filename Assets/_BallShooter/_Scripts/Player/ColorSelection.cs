using Mirror;
using UnityEngine;

namespace _BallShooter._Scripts.Player
{
    public class ColorSelection : NetworkBehaviour
    {
        [SyncVar(hook = nameof(HookSetColor))]
        public Color characterColour;

        public MeshRenderer[] characterRenderers;

        private Material _cachedMaterial;

        private void OnDestroy()
        {
            if (_cachedMaterial) { Destroy(_cachedMaterial); }
        }

        private void HookSetColor(Color oldValue, Color newValue)
        {
            Debug.Log("HookSetColor");
            AssignColours();
        }

        public void AssignColours()
        {
            foreach (MeshRenderer meshRenderer in characterRenderers)
            {
                _cachedMaterial = meshRenderer.material;
                _cachedMaterial.color = characterColour;
            }
        }

        [Command]
        public void CmdSetupCharacter(Color color)
        {
            characterColour = color;
        }
    }
    
}