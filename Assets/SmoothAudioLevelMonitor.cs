using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothAudioLevelMonitor : MonoBehaviour
{
    [Range(0, 0.1f)]
    public float MicLevel;

    AudioClip tmp_mic;
    float t;
    float timedMicLevel;
    float micLevel;

    // Start is called before the first frame update
    void Start()
    {
        tmp_mic = Microphone.Start(null, true, 120, 24000);
        while (Microphone.GetPosition(null) < 0) { }
    }

    // Update is called once per frame
    void Update()
    {
        int pos = Microphone.GetPosition(null);

        if (timedMicLevel >= 0.1f)
        {
            if (pos > 128)
            {
                float[] sa = new float[pos - 128 * tmp_mic.channels];
                tmp_mic.GetData(sa, pos - 128);

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
        MicLevel = Mathf.Lerp(MicLevel, micLevel, t / 0.1f);
    }
}