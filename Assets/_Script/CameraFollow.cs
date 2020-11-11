using UnityEngine;

public class CameraFollow : MonoBehaviour {

	public float smoothSpeed = 0.125f;
	public Vector3 offset;

	Transform target;
	private void Start()
	{
		target = SelectPlayer.instance.player.transform;		
	}
	private void LateUpdate()
	{
		
	}
	void FixedUpdate ()
	{
		//Vector3 desiredPosition = ThridPersonInput.instance.Target.position + offset;
		//Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
		//transform.position = smoothedPosition;

		//transform.LookAt(ThridPersonInput.instance.Target);
		if (ThridPersonInput.instance.isMoveing)
		{
			Vector3 desiredPosition = target.position + offset;
			Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
			transform.position = smoothedPosition;

			transform.LookAt(ThridPersonInput.instance.Target);
		}
	}

}
