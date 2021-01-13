using Mirror;
using UnityEngine;

namespace fwVoiceChat.Scripts
{
	public class PlayerMovement : NetworkBehaviour
	{
		public float Speed = 1;

		void Update()
		{
			if (isLocalPlayer)
			{
				float x = Input.GetAxis("Horizontal");
				float y = Input.GetAxis("Vertical");

				x *= Speed;
				y *= Speed;

				transform.position += new Vector3(x, y, 0);
				Vector3 pos = transform.position;
				CmdSyncMovement(pos.x, pos.y, pos.z);
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
}