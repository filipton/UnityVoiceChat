using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

namespace fwVoiceChat.Scripts
{
	[Serializable]
	public struct VCClient
	{
		public VoiceChat vc;
		public byte Channel;

		public VCClient(VoiceChat VC, byte chann)
		{
			vc = VC;
			Channel = chann;
		}
	}
	
	public class VoiceChatController : NetworkBehaviour
	{
		public Dictionary<VoiceChat, byte> VoiceChatChannels = new Dictionary<VoiceChat, byte>();
		public List<VCClient> vcclients = new List<VCClient>();

		public bool DictToList;
		public bool ListToDict;

		[Header("Settings")]
		[SyncVar] public bool UseProximityVoiceChat = false;

		[Header("ProximityVoiceChatSettings")]
		[SyncVar] public float SpeakingDistance = 10;


		public static VoiceChatController singleton { set; get; }

		private void Awake()
		{
			singleton = this;
		}

		void Update()
		{
			if (Input.GetKeyDown(KeyCode.H))
			{
				UseProximityVoiceChat = !UseProximityVoiceChat;
			}

			if (DictToList)
			{
				DictToList = false;
				vcclients.Clear();
				foreach (var kvp in VoiceChatChannels)
				{
					vcclients.Add(new VCClient(kvp.Key, kvp.Value));
				}
			}
			else if (ListToDict)
			{
				ListToDict = false;
				VoiceChatChannels.Clear();
				foreach (var vc in vcclients)
				{
					VoiceChatChannels.Add(vc.vc, vc.Channel);
				}
			}
		}
	}
}