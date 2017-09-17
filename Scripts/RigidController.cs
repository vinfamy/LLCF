using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Rigidbody))]
[RequireComponent (typeof (MeshCollider))]

public class RigidController : MonoBehaviour {

	public float speed = 10.0f;
	public float gravity = 10.0f;
	public float maxVelocityChange = 10.0f;
	// private bool canJump = true;
	private bool canCrouch = true;
	private float targetTime = 1.0f ;
	public float jumpHeight = 2.0f;
	private bool grounded = false;
	public Rigidbody rigidbody;
	public Animator anim;
	public GameObject feet;
	private bool crouching = false;


	void Awake () {
		rigidbody.freezeRotation = true;
		rigidbody.useGravity = false;
	}

	void Update () 
	{
		targetTime -= Time.deltaTime;
		if (targetTime <= 0.0f) 
		{
			canCrouch = true;
		}
	}

	void FixedUpdate () {
		if (grounded) {
			
			if (Input.GetButton("Crouch") && canCrouch && !anim.GetBool("Swim")) 
			{
				canCrouch = false;
				targetTime = 1.0f;
				crouching = !crouching;
				anim.SetBool ("Crouch", crouching);
			}

			// Calculate how fast we should be moving
			float Hori = Input.GetAxis("Horizontal");
			float Verti = Input.GetAxis ("Vertical");
			Vector3 targetVelocity = new Vector3(Hori / 2, 0, Verti);
			targetVelocity = transform.TransformDirection(targetVelocity);
			targetVelocity *= speed;

			// Play animation
			if (Verti > 0)
			{
				if (Input.GetButton("Run") && !crouching) 
				{
					anim.SetFloat ("Speed", 1.2f);
					targetVelocity *= 2;
				} 
				else 
				{
					anim.SetFloat ("Speed", 0.2f);
				}
				anim.SetBool("Back", false);
			}
			else if (Verti < 0)
			{
				anim.SetFloat("Speed", 0.2f);
				anim.SetBool("Back", true);
				targetVelocity *= 0.5f;
			}
			else
			{
				anim.SetFloat("Speed", 0.0f);
			}

			if (crouching) 
			{
				targetVelocity *= 0.75f;
			}

			// Apply a force that attempts to reach our target velocity
			Vector3 velocity = rigidbody.velocity;
			Vector3 velocityChange = (targetVelocity - velocity);
			velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
			velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
			velocityChange.y = 0;

			if (!Physics.Raycast(feet.transform.position, Vector3.forward, 0.15f))
			{
				rigidbody.AddForce(velocityChange, ForceMode.VelocityChange);
			}

			transform.Rotate (0, Hori * 100.0f * Time.deltaTime, 0);



			// Jump
			if (!anim.GetBool("Swim") && Input.GetButton("Jump")) {
				rigidbody.velocity = new Vector3(velocity.x, CalculateJumpVerticalSpeed() * 0.9f, velocity.z);
				if (crouching == true) 
				{
					crouching = false;
					anim.SetBool ("Crouch", false);
				}
				anim.SetTrigger("Jump");
			}
		}

		// We apply gravity manually for more tuning control
		rigidbody.AddForce(new Vector3 (0, -gravity * rigidbody.mass, 0));

		grounded = false;
	}

	void OnCollisionStay () {
		grounded = true;    
	}

	float CalculateJumpVerticalSpeed () {
		// From the jump height and gravity we deduce the upwards speed 
		// for the character to reach at the apex.
		return Mathf.Sqrt(2 * jumpHeight * gravity);
	}
}