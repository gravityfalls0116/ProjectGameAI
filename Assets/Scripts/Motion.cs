using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Com.Sungjin.FPSpractice
{


    public class Motion : MonoBehaviour
    {
        public float speed;
        public float sprintModifier;
        public Camera normalCam;
        public Transform groundDetector;
        public Transform orientation;
        public LayerMask ground;
        public float jumpForce;

        private Rigidbody rig;
        private float baseFOV;
        private float sprintFOVModifier = 1.5f;


        void Start()
        {
            baseFOV = normalCam.fieldOfView;
            Camera.main.enabled = false;
            rig = GetComponent<Rigidbody>();
            orientation = transform.GetChild(0);
        }


        void FixedUpdate()
        {
            //Axis
            float t_hmove =Input.GetAxisRaw("Horizontal");
            float t_vmove = Input.GetAxisRaw("Vertical");

            //Controls
            bool sprint = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
            bool jump = Input.GetKey(KeyCode.Space);

            //States
            bool isGrounded = Physics.Raycast(groundDetector.position, Vector3.down, 0.1f, ground);
            bool isJumping = jump && isGrounded;
            bool isSprinting = sprint && t_vmove > 0 && !isJumping && isGrounded;
 

            //Jumping
            if (isJumping)
            {
                rig.AddForce(Vector3.up * jumpForce);
            }

            //Movements
            Vector3 t_direction = new Vector3(t_hmove,0, t_vmove).normalized;

            float t_adjustSpeed = speed;
            if (isSprinting) t_adjustSpeed *= sprintModifier;

            
            Vector3 t_targetVelocity= orientation.TransformDirection(t_direction) * t_adjustSpeed * Time.deltaTime;
            t_targetVelocity.y = rig.velocity.y;
            rig.velocity = t_targetVelocity;

            //Field of View
            if(isSprinting) {normalCam.fieldOfView = Mathf.Lerp(normalCam.fieldOfView,baseFOV*sprintFOVModifier,Time.deltaTime*8f); }
            else { normalCam.fieldOfView = Mathf.Lerp(normalCam.fieldOfView, baseFOV, Time.deltaTime * 8f); ; }
        }
    }
}
