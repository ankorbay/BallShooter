using UnityEngine;
using Mirror;

namespace Mirror.Examples.CharacterSelection
{
    public class CharacterSelection : NetworkBehaviour
    {
        [SyncVar(hook = nameof(HookSetColor))]
        public Color characterColour;
        private Material cachedMaterial;
        public MeshRenderer[] characterRenderers;

        void HookSetColor(Color _old, Color _new)
        {
            Debug.Log("HookSetColor");
            AssignColours();
        }

        public void AssignColours()
        {
            foreach (MeshRenderer meshRenderer in characterRenderers)
            {
                cachedMaterial = meshRenderer.material;
                cachedMaterial.color = characterColour;
            }
        }

        void OnDestroy()
        {
            if (cachedMaterial) { Destroy(cachedMaterial); }
        }
        
        [Command]
        public void CmdSetupCharacter(Color color)
        {
            characterColour = color;
        }
    }
}