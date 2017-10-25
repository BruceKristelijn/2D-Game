using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hoofdScript : MonoBehaviour {

	public GameObject balletje;
	public GameObject blokje;

	public Transform balStart;
	public Transform[] blokStart;


	void Start () {
		Instantiate (balletje, new Vector2(0,0), Quaternion.identity);
		/*for (int i = 0; i < blokStart.Length; i++) {
			Instantiate (blokje, blokStart [i].position, Quaternion.identity);
		}*/
		for (int i = 0; i < 4; i++) {
			createBlokje();			
		}
	}
	
	public void createBlokje(){
		Instantiate (blokje, new Vector3(Random.Range(-3, 3), Random.Range(-4, 4), 0), Quaternion.identity);
	}
}
