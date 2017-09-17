using UnityEngine;

public class CameraScroll : MonoBehaviour {
	public Camera[] Cameras;
	// 0 - Bird view, 1 - Third Person, 2 - First person
	public float currentCam = 2;
	public float speed = 10;
	public float speedlook = 3;
	public bool rotated = false;
	public Camera Myself;
	private int ExcludeHead;
	
	// Update is called once per frame
	void Update () {
		if (Input.GetAxis("Mouse ScrollWheel") > 0f && currentCam < 2) // forward
			{

			float step = speed * Time.deltaTime;
			if (currentCam < 1) {
				step *= 3;
			}
			int camIdx = Mathf.CeilToInt (currentCam);
			if (camIdx == currentCam){
				camIdx += 1;
			}
			Camera Destination = Cameras [camIdx];
			Vector3 Previous = transform.position;
			transform.position = Vector3.MoveTowards(transform.position, Destination.transform.position, step);
			int PreviousIdx = Mathf.FloorToInt (currentCam);
			if (PreviousIdx < 0){
				PreviousIdx = 0;
				Destination = Cameras [1];
			}
			float change = (Vector3.Distance (Previous, transform.position) / (Vector3.Distance (Destination.transform.position, Cameras [PreviousIdx].transform.position)));
				if (change == 0) {
					currentCam = camIdx;
					transform.position = Cameras [camIdx].transform.position;
				} 
				else {
					currentCam += change;
				}
			if (currentCam > 0.90 && rotated){
				GetComponent<CameraLook>().enabled = true;
				transform.rotation = Cameras [1].transform.rotation;
				rotated = false;
			}
			if (currentCam >= 2) {
				currentCam = 2;
				transform.rotation = Cameras [2].transform.rotation;
				transform.position = Cameras [2].transform.position;
				Myself.cullingMask = ExcludeHead;
			}

			}

		else if (Input.GetAxis("Mouse ScrollWheel") < 0f && currentCam > 0) // backwards
			{

			float step = speed * Time.deltaTime;
			if (currentCam < 1) {
				step *= 3;
			}
			int camIdx = Mathf.FloorToInt (currentCam);
			if (camIdx == currentCam){
				camIdx -= 1;
			}
			Camera Destination = Cameras [camIdx];
			Vector3 Previous = transform.position;
			transform.position = Vector3.MoveTowards (transform.position, Destination.transform.position, step);
			int PreviousIdx = Mathf.CeilToInt (currentCam);
			if (PreviousIdx > 2){
				PreviousIdx = 2;
				Destination = Cameras [1];
			}
			float change = (Vector3.Distance (Previous, transform.position) / (Vector3.Distance (Destination.transform.position, Cameras [PreviousIdx].transform.position)));
				if (change == 0) {
					currentCam = camIdx;
					transform.position = Cameras [camIdx].transform.position;
				} 
				else {
					currentCam -= change;
				}
			if (currentCam < 0.90 && !rotated){
				GetComponent<CameraLook>().enabled = false;
				transform.rotation = Cameras [0].transform.rotation;
				rotated = true;
			}
			if (currentCam < 0) {
				currentCam = 0;
			}
			if (Myself.cullingMask == ExcludeHead) 
			{
				Myself.cullingMask |= 1 << 8;
			}
			}
			

		//if (Input.GetAxis("Mouse X") != 0)
		//{
		//	float step = Time.deltaTime * speedlook;
		//	//transform.position += new Vector3(Input.GetAxisRaw("Mouse X") * step, 0.0f, Input.GetAxisRaw("Mouse Y") * step);
		//	Vector3 NewPosition = transform.position + new Vector3(Input.GetAxisRaw("Mouse X") * step, 0.0f, 0.0f);
		//	if (Vector3.Distance(NewPosition, 
		//	transform.position = NewPosition;
		//}
	}

	void Start(){
		ExcludeHead = Myself.cullingMask;
	}
}
