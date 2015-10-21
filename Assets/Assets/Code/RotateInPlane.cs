using UnityEngine;
using System.Collections;

public class RotateInPlane : MonoBehaviour {
	public float speed = 30F;
	// Use this for initialization
	void Start () {
		speed = 30;
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate (0,Time.deltaTime * speed,0);
	}
}
