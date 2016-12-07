namespace DynamicLight2D{
	using UnityEngine;
	using UnityEngine.Events;
	using System.Collections;
	using System;
	
	[Serializable]
	public class DynamicLightEvent : UnityEvent<GameObject>{}
	//public class Delegator<T>: UnityAction<T>{}

	//public class DynamicLightEventArray : UnityEvent<GameObject[]>{}
}

