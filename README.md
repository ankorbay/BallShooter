In case you expirience "Can't resolve desination host" in Lobby" or have some issues with creating. Please try the following:  
1 - Open Assets/Resources/NetworkManagerColorSelection.prefab  
2 - Copy token value from DummyTokenHolder.cs component attached  
3 - Paste it into EdgegapLobbyKcpTransport Editor menu (Create & Deploy Lobby), add new Lobby SERVICE Name using 1-5 (STRICT) letters or digits (not a lobby itself, just a service for creating them) and press "Create".  

P.s. i did expirienced some SSL errors straight after new Lobby SERVICE created while using Belarussian IP, it seems to work better with european VPN enabled, but i'm not confident that it's the reason.
