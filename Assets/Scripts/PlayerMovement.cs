using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    public float Speed = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		if (isLocalPlayer)
		{
            float x = Input.GetAxis("Horizontal");
            float y = Input.GetAxis("Vertical");

            x *= Speed;
            y *= Speed;

            transform.position += new Vector3(x, y, 0);
            CmdSyncMovement(transform.position.x, transform.position.y, transform.position.z);
        }
    }

    [Command]
    public void CmdSyncMovement(float x, float y, float z)
	{
		if (isServerOnly)
		{
            Vector3 vec = new Vector3(x, y, z);
            transform.position = vec;
        }
        RpcSyncMovement(x, y, z);
	}

    [ClientRpc]
    public void RpcSyncMovement(float x, float y, float z)
    {
		if (!isLocalPlayer && !isServerOnly)
		{
            Vector3 vec = new Vector3(x, y, z);
            transform.position = vec;
        }
    }
}