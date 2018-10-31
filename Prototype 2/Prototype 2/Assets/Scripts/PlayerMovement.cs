using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour {

    public Transform prefab1;
	public Transform TrackFolder;
    public float speed;
    public float rotateSpeed;
	public float roadNumber = 0;
	
    private Transform lastEmitted = null;
	private float roadRenderDist = 4;
	private float currentSpeed = 0.5f;
	private float minSpeed = 0.5f;
	private float maxSpeed = 3;
	private float accel = 0.01f;
	private float torque = 0;
	private float torqueLimit = 3;
	private float torqueLoss = 5;
	

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
       // float mult = 1 + (Input.GetAxis("Vertical") * 0.5f);
		
		float input = Input.GetAxis("Vertical");
		if (input != 0)
		{
			torque = torque + input;
			if (torque > torqueLimit)
			{
				torque = torqueLimit;
			}
			else if (torque < -torqueLimit)
			{
				torque = -torqueLimit;
			}
		} else if (input == 0){
			torque = torque/torqueLoss;
		}
		
		currentSpeed = currentSpeed + (accel*torque);
		
		if (currentSpeed > maxSpeed) currentSpeed = maxSpeed;
		if (currentSpeed < minSpeed) currentSpeed = minSpeed;
		
		float mult = currentSpeed;
        transform.position += transform.forward * speed * 0.5f * mult;
        Quaternion rot = Quaternion.Euler(0, 90, 0);
        rot = transform.rotation;
        
        rot *= Quaternion.AngleAxis(90, Vector3.up);
        //Debug.Log(lastEmitted);
        bool closeEnough = lastEmitted == null || Vector3.Distance(lastEmitted.transform.position, transform.position) > roadRenderDist * mult;
        if (closeEnough)
        {
            lastEmitted = Object.Instantiate(prefab1, transform.position, rot, TrackFolder);
            lastEmitted.localScale = new Vector3(lastEmitted.localScale.x * mult, lastEmitted.localScale.y, lastEmitted.localScale.z * mult);
			roadNumber++;
			lastEmitted.name = roadNumber.ToString();
			//lastEmitted.Bumper_Left.collider.tag = "Wall";
			//lastEmitted.Bumper_Right.collider.tag = "Wall";
        }
        transform.Rotate(0, Input.GetAxis("Horizontal")*rotateSpeed, 0);
		
		int rays = 5;
		for(int i = 0; i < rays; i++)
		{
			RaycastHit hit;
			Ray newRay = new Ray(transform.position, transform.forward + (transform.right * ((i-(rays/2))*2))); 
			
			if (Physics.Raycast(newRay, out hit, mult*5))
			{
				if (hit.collider.name == "Bumper_Left" || hit.collider.name == "Bumper_Right")
				{
					hit.collider.gameObject.SetActive(false);
					//Debug.Log("hit");
				}
			}
		}
		
        if(Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
