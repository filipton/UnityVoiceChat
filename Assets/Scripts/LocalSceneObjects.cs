using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocalSceneObjects : MonoBehaviour
{
    public static LocalSceneObjects singleton;

    public Image MicImage;
    public RawImage MicBorderImage;

	private void Awake()
	{
        singleton = this;
	}
}