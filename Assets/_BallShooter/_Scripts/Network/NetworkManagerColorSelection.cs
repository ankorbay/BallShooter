using _BallShooter._Scripts.Infrastructure.Services;
using _BallShooter._Scripts.Player;
using Mirror;
using Mirror.Examples.CharacterSelection;
using UnityEngine;

namespace _BallShooter._Scripts.Network
{
    [AddComponentMenu("")]
    public class NetworkManagerColorSelection : NetworkManager
    {
        public bool SpawnAsCharacter = true;

        public static new NetworkManagerColorSelection singleton => (NetworkManagerColorSelection)NetworkManager.singleton;
        private CharacterData characterData;

        public IStaticDataService _staticDataService;
        
        public void Configure(IStaticDataService staticDataService)
        {
            _staticDataService = staticDataService;
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
                ? Instantiate(_staticDataService.GameSettings.playerSettings.playerPrefab, startPos.position, startPos.rotation)
                : Instantiate(_staticDataService.GameSettings.playerSettings.playerPrefab);


            // Apply data from the message however appropriate for your game
            // Typically Player would be a component you write with syncvars or properties
            ColorSelection characterSelection = playerObject.GetComponent<ColorSelection>();
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

            GameObject playerObject = Instantiate(_staticDataService.GameSettings.playerSettings.playerPrefab, startPos.position, startPos.rotation);

            // Instantiate the new player object and broadcast to clients
            NetworkServer.ReplacePlayerForConnection(conn, playerObject, ReplacePlayerOptions.KeepActive);

            // Apply data from the message however appropriate for your game
            // Typically Player would be a component you write with syncvars or properties
            ColorSelection characterSelection = playerObject.GetComponent<ColorSelection>();
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