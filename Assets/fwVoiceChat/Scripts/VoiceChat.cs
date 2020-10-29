using Mirror;
using UnityEngine;

namespace fwVoiceChat.Scripts
{
    public class VoiceChat : NetworkBehaviour
    {
        public AudioSource audioSource;
        public AnimationCurve VoiceRollOff;
        public bool HearYourself = false;

        [Header("Voice Chat Settings")]
        [Tooltip("Its frequency of input (mic) in khz")]
        public int micFrequency = 44100;

        [Tooltip("Its in loop but its for saving resources (because audio clip cannot be eg. 1hour long)")]
        public int micRecordLenght = 120;

        [Tooltip("If its lower you might hear \"clipping\"")]
        public int micSamplePacketSize = 7350;

        byte mineChannel = 1;
        int lastSample;

        void Start()
        {
            if (isServer)
            {
                VoiceChatController.singleton.VoiceChatChannels.Add(this, 1);
            }

            if(!isLocalPlayer)
            {
                GetComponent<AudioListener>().enabled = false;
            }
            if(audioSource == null) audioSource = GetComponent<AudioSource>();
        }

        void Update()
        {
            if (isLocalPlayer)
            {
                if (Input.GetKey(KeyCode.V))
                {
                    if (VoiceChatController.singleton.UseProximityVoiceChat) SendProximityVoice();
                    else SendChanneledVoice();
                }
                else
                {
                    lastSample = MicrophoneCore.singleton.MicPos;
                }

                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    mineChannel++;
                    CmdChangeChannel(mineChannel);
                }
                else if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    mineChannel--;
                    CmdChangeChannel(mineChannel);
                }
            }
        }

        void SendChanneledVoice()
        {
            int pos = MicrophoneCore.singleton.MicPos;
            int diff = pos - lastSample;

            if (diff >= micSamplePacketSize)
            {
                float[] samples = new float[diff * MicrophoneCore.singleton.micAudioClip.channels];
                MicrophoneCore.singleton.micAudioClip.GetData(samples, lastSample);
                byte[] ba = ToByteArray(samples);

                CmdSendVoice(ba);

                lastSample = pos;
            }
        }

        public void SendProximityVoice()
        {
            int pos = MicrophoneCore.singleton.MicPos;
            int diff = pos - lastSample;
            if (diff >= micSamplePacketSize)
            {
                float[] samples = new float[diff * MicrophoneCore.singleton.micAudioClip.channels];
                MicrophoneCore.singleton.micAudioClip.GetData(samples, lastSample);
                byte[] ba = ToByteArray(samples);

                CmdSendProximityVoice(ba);

                lastSample = pos;
            }
        }

        [Command]
        public void CmdSendProximityVoice(byte[] ba)
        {
            foreach(var vckp in VoiceChatController.singleton.VoiceChatChannels)
            {
                if (vckp.Key)
                {
                    VoiceChatController.singleton.VoiceChatChannels.Remove((vckp.Key));
                }
                else
                {
                    if (vckp.Key != this && (transform.position - vckp.Key.transform.position).sqrMagnitude < VoiceChatController.singleton.SpeakingDistance*VoiceChatController.singleton.SpeakingDistance)
                    {
                        TargetRpcReciveVoice(vckp.Key.connectionToClient, ba);
                    }   
                }
            }
        }

        [Command]
        public void CmdChangeChannel(byte channel)
        {
            VoiceChatController.singleton.VoiceChatChannels[this] = channel;
        }

        [TargetRpc]
        public void TargetRpcReciveVoice(NetworkConnection conn, byte[] ba)
        {
            if (VoiceChatController.singleton.UseProximityVoiceChat) audioSource.volume = VoiceRollOff.Evaluate(-((transform.position - conn.identity.transform.position).magnitude / VoiceChatController.singleton.SpeakingDistance) +1);
            else if (!VoiceChatController.singleton.UseProximityVoiceChat && audioSource.volume != 1) audioSource.volume = 1;

            ParseVoiceData(ba);
        }

        [Command]
        public void CmdSendVoice(byte[] ba)
        {
            byte channel = VoiceChatController.singleton.VoiceChatChannels[this];
            
            foreach (var pl in VoiceChatController.singleton.VoiceChatChannels)
            {
                if (pl.Key)
                {
                    VoiceChatController.singleton.VoiceChatChannels.Remove((pl.Key));
                }
                else
                {
                    if ((pl.Value == channel && pl.Key != this) || (pl.Key == this && HearYourself))
                    {
                        TargetRpcReciveVoice(pl.Key.connectionToClient, ba);
                    }   
                }
            }
        }

        void ParseVoiceData(byte[] ba)
        {
            float[] f = ToFloatArray(ba);
            AudioClip ac = AudioClip.Create("voice", f.Length, 1, micFrequency, false);
            ac.SetData(f, 0);
            audioSource.clip = ac;
            if (!audioSource.isPlaying) audioSource.Play();
        }

        byte[] ToByteArray(float[] floatArray)
        {
            int len = floatArray.Length * 4;
            byte[] byteArray = new byte[len];
            int pos = 0;
            foreach (float f in floatArray)
            {
                byte[] data = System.BitConverter.GetBytes(f);
                System.Array.Copy(data, 0, byteArray, pos, 4);
                pos += 4;
            }
            return byteArray;
        }

        float[] ToFloatArray(byte[] byteArray)
        {
            int len = byteArray.Length / 4;
            float[] floatArray = new float[len];
            for (int i = 0; i < byteArray.Length; i += 4)
            {
                floatArray[i / 4] = System.BitConverter.ToSingle(byteArray, i);
            }
            return floatArray;
        }
    }
}