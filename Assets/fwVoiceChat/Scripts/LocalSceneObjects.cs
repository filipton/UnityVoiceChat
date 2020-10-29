using UnityEngine;
using UnityEngine.UI;

namespace fwVoiceChat.Scripts
{
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
}