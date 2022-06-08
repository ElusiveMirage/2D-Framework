using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// T is any class that can be added to Generic Singleton
public class GenericSingleton<T> : MonoBehaviour where T : Component {
	
	private static T instance;
	public static T Instance {
		get {
			if (instance == null) {
				instance = FindObjectOfType<T> ();
				if (instance == null) {
					GameObject obj = new GameObject ();
					obj.name = typeof(T).Name;
					instance = obj.AddComponent<T> ();
				}
			}
			return instance;
		}
	}

	public virtual void Awake ()
	{
		if (instance == null) {
			instance = this as T;
			if(transform.parent != null)
            {
				DontDestroyOnLoad(transform.parent.gameObject);
			}
			else
            {
				DontDestroyOnLoad(this.gameObject);
			}			
		} else {
			Destroy (gameObject);
		}
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
