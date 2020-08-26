using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoiceChatController : NetworkBehaviour
{
    public Dictionary<VoiceChat, byte> VoiceChatChannels = new Dictionary<VoiceChat, byte>();

    [Header("Settings")]
    [SyncVar] public bool UseProximityVoiceChat = false;

    [Header("ProximityVoiceChatSettings")]
    [SyncVar] public float SpeakDistance = 10;


    public static VoiceChatController singleton { set; get; }

	private void Awake()
	{
        singleton = this;
	}

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		if (Input.GetKeyDown(KeyCode.H))
		{
            UseProximityVoiceChat = !UseProximityVoiceChat;
		}
    }
}