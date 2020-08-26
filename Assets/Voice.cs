using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Voice : MonoBehaviour
{
    public AudioSource audioSource;

    [Header("Voice Chat Settings")]
    public int micFrequency = 44100;

    //its in loop but its for saving resources (bc audioclip cannot be eg. 1hour long)
    public int micRecordLenght = 120;

    //if its lower you might hear "clipping"
    public int micSamplePacketSize = 7350;


    AudioClip tmp_mic;
    int lastSample;

    void Start()
    {
        if(audioSource == null) audioSource = GetComponent<AudioSource>();

        tmp_mic = Microphone.Start(null, true, micRecordLenght, micFrequency);
        while (Microphone.GetPosition(null) < 0) { }
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.V))
        {
            int pos = Microphone.GetPosition(null);
            int diff = pos - lastSample;
            if (diff >= micSamplePacketSize)
            {
                float[] samples = new float[diff * tmp_mic.channels];
                tmp_mic.GetData(samples, lastSample);
                byte[] ba = ToByteArray(samples);

                Cmd_SendRPC(ba, tmp_mic.channels);

                lastSample = pos;
            }
        }
        else
        {
            lastSample = Microphone.GetPosition(null);
        }
    }

    public void Rpc_Send(byte[] ba, int chan)
    {
        ReciveData(ba, chan);
    }

    public void Cmd_SendRPC(byte[] ba, int chan)
    {
        ReciveData(ba, chan);
        //Rpc_Send(ba, chan);
    }

    void ReciveData(byte[] ba, int chan)
    {
        float[] f = ToFloatArray(ba);
        AudioClip ac = AudioClip.Create("voice", f.Length, chan, micFrequency, false);
        ac.SetData(f, 0);
        audioSource.clip = ac;
        if (!audioSource.isPlaying) audioSource.Play();
    }


    public byte[] ToByteArray(float[] floatArray)
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

    public float[] ToFloatArray(byte[] byteArray)
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