using UnityEngine;


/// <summary>
/// If a custom behaviour is required throughout a certain state (e.g. periodic jumping)
/// then a StateMachineBehaviour should be used.
/// </summary>


public class BehaviourBase : StateMachineBehaviour
{
    //public GameObject particles;            // Prefab of the particle system to play in the state.
    //public AvatarIKGoal attackLimb;         // The limb that the particles should follow.


    //private Transform particlesTransform;       // Reference to the instantiated prefab's transform.
    //private ParticleSystem particleSystem;      // Reference to the instantiated prefab's particle system.


    // This will be called when the animator first transitions to this state.
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        foreach (CatState catState in CatState.GetValues(typeof(CatState)))
        {
            if (stateInfo.IsName("Cat-" + catState.ToString()))
            {
                animator.gameObject.GetComponent<CharacterBase>().SetState(catState); //this works!!
                //animator.gameObject.SendMessage("SetState", CatState.Launch);
                return;
            }
        }
        //calback to the game object to update the State and keep consistent

        //// If the particle system already exists then exit the function.
        //if (particlesTransform != null)
        //    return;

        //// Otherwise instantiate the particles and set up references to their components.
        //GameObject particlesInstance = Instantiate(particles);
        //particlesTransform = particlesInstance.transform;
        //particleSystem = particlesInstance.GetComponent<ParticleSystem>();
    }


    //// This will be called once the animator has transitioned out of the state.
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // When leaving the special move state, stop the particles.
    //    particleSystem.Stop();
    //}


    //// This will be called every frame whilst in the state.
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // OnStateExit may be called before the last OnStateIK so we need to check the particles haven't been destroyed.
    //    if (particleSystem == null || particlesTransform == null)
    //        return;

    //    // Find the position and rotation of the limb the particles should follow.
    //    Vector3 limbPosition = animator.GetIKPosition(attackLimb);
    //    Quaternion limbRotation = animator.GetIKRotation(attackLimb);

    //    // Set the particle's position and rotation based on that limb.
    //    particlesTransform.position = limbPosition;
    //    particlesTransform.rotation = limbRotation;

    //    // If the particle system isn't playing, play it.
    //    if (!particleSystem.isPlaying)
    //        particleSystem.Play();
    //}
}