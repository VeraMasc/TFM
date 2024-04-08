using UnityEngine;


namespace SimpleMan.GlobalEvents.Demo
{
    [RequireComponent(typeof(Rigidbody))]
    public class Jumper : MonoBehaviour
    {
        //******            FIELDS          	******\\
        public float force = 10;
        private Rigidbody _rigidbody;



        //******    	    METHODS  	  	    ******\\
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        public void MakeJump()
        {
            _rigidbody.AddForce(Vector3.up * force, ForceMode.Impulse);
        }
    } 
}