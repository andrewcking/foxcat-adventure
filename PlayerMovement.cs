using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent (typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour {
	
	public float movementSpeed = 2.0f;
	public float mouseSensitivity = 3.0f;
	public float jumpSpeed = 2.0f;
	public float upDownRange = 60.0f;
	private float forwardSpeed;
	private int count;
	float verticalRotation = 0;
	float verticalVelocity = 0;
	public Text countText;
	public Text winText;
	AudioSource tingAudio;
	AudioSource winAudio;
	
	Animator anim;
	CharacterController characterController;
	AudioSource walkAudio;
	AudioSource jumpAudio;

	// Use this for initialization
	void Start () {
		count = 0;
		setCountText ();
		Screen.lockCursor = true;
		characterController = GetComponent<CharacterController>();
		anim = GetComponent<Animator>();;
		AudioSource[] audios = GetComponents<AudioSource>();
		walkAudio = audios[0];
		jumpAudio = audios[1];
		tingAudio = audios[3];
		winAudio = audios[4];
	}

	// Update is called once per frame
	void Update () {

		//Restart Key
		if(Input.GetKeyDown(KeyCode.R)){
			Application.LoadLevel(0);
		}

		// Rotation
		float rotLeftRight = Input.GetAxis("Mouse X") * mouseSensitivity;
		transform.Rotate(0, rotLeftRight, 0);


		// Movement

		if (Input.GetAxis ("Vertical") < 0) {
			 forwardSpeed = (Input.GetAxis ("Vertical") * movementSpeed) / 2;
		} else {
			 forwardSpeed = Input.GetAxis ("Vertical") * movementSpeed;
		}
		verticalVelocity += Physics.gravity.y * Time.deltaTime;
		
		if (characterController.isGrounded && Input.GetButton ("Jump")) {
			verticalVelocity = jumpSpeed;
			//jump animation
			anim.SetBool ("Jump", true);
			jumpAudio.Play();
			walkAudio.Stop();
		} else {
			anim.SetBool ("Jump", false);
		}

		//walking sound
		if (characterController.isGrounded && Input.GetButton ("Vertical") && !walkAudio.isPlaying) {
			walkAudio.Play();
		}else if(Input.GetButtonUp ("Vertical")){
			walkAudio.Stop();
		}

		//Actual Movement commands
		Vector3 speed = new Vector3( 0.0f, verticalVelocity, forwardSpeed );
		speed = transform.rotation * speed;
		characterController.Move( speed * Time.deltaTime );

		//animation variables
		anim.SetFloat("Speed", Mathf.Max(0, forwardSpeed));
		anim.SetFloat("AngleSpeed", Mathf.Abs(rotLeftRight));
		anim.SetFloat("Reverse", forwardSpeed);
	}


	//UI and Ting Sounds
	void OnTriggerEnter(Collider other){
		if (other.gameObject.CompareTag ("Coin")) {
			tingAudio.Play();
			count++;
			setCountText ();
		}
		if (other.gameObject.CompareTag ("Treasure")) {
			tingAudio.Play();
			count = count + 10;
			setCountText ();
		}
	}
	void setCountText () {
			countText.text = "Coins: " + count.ToString () + "/50";
			if (count >= 50) {
				winText.text = "You Win! \r\n Press R to Play Again";
				winAudio.Play();
				//restart
			}
		}
}
