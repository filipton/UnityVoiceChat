using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicrophoneCore : NetworkBehaviour
{
    public static MicrophoneCore singleton;

    [Range(0, 0.1f)]
    public float MicLevel;
    public AudioClip micAudioClip;

    public int MicPos;
    
    float t;
    float timedMicLevel;
    float micLevel;

    private void Awake()
	{
		
    }

	// Start is called before the first frame update
	void Start()
    {
        if (isLocalPlayer)
        {
            singleton = this;

            micAudioClip = AudioClip.Create("mic", 1, 1, 24000, false);

            micAudioClip = Microphone.Start(null, true, 120, 24000);
            while (Microphone.GetPosition(null) < 0) { }
        }
    }

    // Update is called once per frame
    void Update()
    {
		if (isLocalPlayer)
		{
            MicPos = Microphone.GetPosition(null);

            if (timedMicLevel >= 0.07f)
            {
                if (MicPos > 128)
                {
                    float[] sa = new float[(MicPos - 128) * micAudioClip.channels];
                    micAudioClip.GetData(sa, MicPos - 128);

                    float levelMax = 0;
                    for (int i = 0; i < 128; i++)
                    {
                        float wavePeak = sa[i] * sa[i];
                        if (levelMax < wavePeak)
                        {
                            levelMax = wavePeak;
                        }
                    }
                    micLevel = Mathf.Sqrt(levelMax);
                    t = 0;
                    timedMicLevel = 0;
                }
            }
            else
            {
                timedMicLevel += Time.deltaTime;
            }

            t += Time.deltaTime;
            MicLevel = Mathf.Lerp(MicLevel, micLevel, t / 0.05f);
            LocalSceneObjects.singleton.MicImage.fillAmount = micLevel * 10;
		}
    }
}