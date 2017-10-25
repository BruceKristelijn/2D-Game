using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraFollow : MonoBehaviour {

	public GameObject Player;

	void Start () {
		this.transform.position = new Vector3(Player.transform.position.x, Player.transform.position.y, -10);
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.position = new Vector3 (0, Player.transform.position.y+5, -10);;		
	}
}
