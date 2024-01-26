// generic unity singleton

using UnityEngine;

namespace SimVik.Utility
{
	public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
	{
		static T instance;

		public static T Instance
		{
			get
			{
				if (instance!=null) return instance;

				instance = FindObjectOfType<T>();

				if (instance!=null) return instance;

				var singleton = new GameObject(typeof(T).Name);
				instance = singleton.AddComponent<T>();
				return instance;
			}
		}
	}
}