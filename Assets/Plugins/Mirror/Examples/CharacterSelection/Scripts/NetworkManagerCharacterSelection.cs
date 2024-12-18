using UnityEngine;

namespace Mirror.Examples.CharacterSelection
{
    [AddComponentMenu("")]
    public class NetworkManagerCharacterSelection : NetworkManager
    {
        // See the scene 'SceneMapSpawnWithNoCharacter', to spawn as empty player.
        // 'SceneMap' will auto spawn as random player character.
        // Compare Network Manager inspector setups to see the difference between the two.
        // Either of these allow selecting character after spawning in too.
        public bool SpawnAsCharacter = true;

        public static new NetworkManagerCharacterSelection singleton => (NetworkManagerCharacterSelection)NetworkManager.singleton;
        private CharacterData characterData;

        public override void Start()
        {
            characterData = CharacterData.characterDataSingleton;
            if (characterData == null)
            {
                Debug.Log("Add CharacterData prefab singleton into the scene.");
                return;
            }
            base.Awake();
        }

        public struct CreateCharacterMessage : NetworkMessage
        {
            public Color characterColour;
        }

        public struct ReplaceCharacterMessage : NetworkMessage
        {
            public CreateCharacterMessage createCharacterMessage;
        }

        public override void OnStartServer()
        {
            base.OnStartServer();

            NetworkServer.RegisterHandler<CreateCharacterMessage>(OnCreateCharacter);
            NetworkServer.RegisterHandler<ReplaceCharacterMessage>(OnReplaceCharacterMessage);
        }

        public override void OnClientConnect()
        {
            base.OnClientConnect();

            if (SpawnAsCharacter)
            {
                // you can send the message here, or wherever else you want
                CreateCharacterMessage characterMessage = new CreateCharacterMessage
                {
                    characterColour = StaticVariables.characterColour
                };

                NetworkClient.Send(characterMessage);
            }
        }

        void OnCreateCharacter(NetworkConnectionToClient conn, CreateCharacterMessage message)
        {
            Transform startPos = GetStartPosition();
            
            // check if the save data has been pre-set
            if (message.characterColour == new Color(0, 0, 0, 0))
            {
                Debug.Log("OnCreateCharacter colour invalid or not set, use random.");
                message.characterColour = Random.ColorHSV(0f, 1f, 1f, 1f, 0f, 1f);
            }

            GameObject playerObject = startPos != null
                ? Instantiate(characterData.characterPrefabs[0], startPos.position, startPos.rotation)
                : Instantiate(characterData.characterPrefabs[0]);


            // Apply data from the message however appropriate for your game
            // Typically Player would be a component you write with syncvars or properties
            CharacterSelection characterSelection = playerObject.GetComponent<CharacterSelection>();
            characterSelection.characterColour = message.characterColour;

            // call this to use this gameobject as the primary controller
            NetworkServer.AddPlayerForConnection(conn, playerObject);
            Debug.Log("---------->>>> NetworkServer.AddPlayerForConnection");
        }

        void OnReplaceCharacterMessage(NetworkConnectionToClient conn, ReplaceCharacterMessage message)
        {
            Transform startPos = GetStartPosition();
            // Cache a reference to the current player object
            GameObject oldPlayer = conn.identity.gameObject;

            GameObject playerObject = Instantiate(characterData.characterPrefabs[0], startPos.position, startPos.rotation);

            // Instantiate the new player object and broadcast to clients
            NetworkServer.ReplacePlayerForConnection(conn, playerObject, ReplacePlayerOptions.KeepActive);

            // Apply data from the message however appropriate for your game
            // Typically Player would be a component you write with syncvars or properties
            CharacterSelection characterSelection = playerObject.GetComponent<CharacterSelection>();
            characterSelection.characterColour = message.createCharacterMessage.characterColour;

            // Remove the previous player object that's now been replaced
            // Delay is required to allow replacement to complete.
            Destroy(oldPlayer, 0.1f);
        }

        public void ReplaceCharacter(ReplaceCharacterMessage message)
        {
            NetworkClient.Send(message);
        }
    }
}
