using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

namespace fwVoiceChat.Scripts
{
    public enum Role
    {
        Innocent,
        Traitor,
        Spectator,
        Overwatch = 69
    }

    public class PlayerStats : NetworkBehaviour
    {
        [SyncVar]
        public Role PlayerRole = Role.Spectator;
    }
}