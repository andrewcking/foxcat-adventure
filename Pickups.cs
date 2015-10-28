using UnityEngine;

using System.Collections;

public class Pickups : MonoBehaviour {
	// Update is called once per frame

	void OnTriggerEnter(Collider other){
		if (other.gameObject.CompareTag ("Player")) {
			StartCoroutine(Example());

		}
	}

	IEnumerator Example() {
		GetComponent<Rigidbody>().velocity = new Vector3(0, 2, 0);;
		yield return new WaitForSeconds(3);
		gameObject.SetActive (false);
	}


}
