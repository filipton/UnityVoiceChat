using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // Start is called before the first frame update
    void Start()
    {
		
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}