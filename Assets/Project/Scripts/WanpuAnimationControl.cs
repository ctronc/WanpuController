using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CMF
{
	public class WanpuAnimationControl : MonoBehaviour {

		Controller controller;
		Animator animator;
		Transform animatorTransform;
		Transform tr;

		private float smoothingFactor = 0.8f;
		Vector3 oldMovementVelocity = Vector3.zero;

		//Setup;
		void Awake () {
			controller = GetComponent<Controller>();
			animator = GetComponentInChildren<Animator>();
			animatorTransform = animator.transform;

			tr = transform;
		}

		//OnEnable;
		void OnEnable()
		{
			//Connect events to controller events;
			controller.OnLand += OnLand;
			controller.OnJump += OnJump;
		}

		//OnDisable;
		void OnDisable()
		{
			//Disconnect events to prevent calls to disabled gameobjects;
			controller.OnLand -= OnLand;
			controller.OnJump -= OnJump;
		}
		
		//Update;
		void Update () {

			//Get controller velocity;
			Vector3 _velocity = controller.GetVelocity();

			//Split up velocity;
			Vector3 _horizontalVelocity = VectorMath.RemoveDotVector(_velocity, tr.up);
			Vector3 _verticalVelocity = _velocity - _horizontalVelocity;

			//Smooth horizontal velocity for fluid animation;
			_horizontalVelocity = Vector3.Lerp(oldMovementVelocity, _horizontalVelocity, smoothingFactor);
			oldMovementVelocity = _horizontalVelocity;

			animator.SetFloat("VerticalSpeed", _verticalVelocity.magnitude * VectorMath.GetDotProduct(_verticalVelocity.normalized, tr.up));
			animator.SetFloat("HorizontalSpeed", _horizontalVelocity.magnitude);

			//Pass values to animator;
			animator.SetBool("IsGrounded", controller.IsGrounded());
		}


		void OnLand(Vector3 _v)
		{
			animator.SetTrigger("OnLand");
		}

		void OnJump(Vector3 _v)
		{
			
		}
	}
}
