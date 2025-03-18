using UnityEngine;

public class AnimationStateChanger : MonoBehaviour
{
    public Animator animator;
    public string currentState;
    void Awake(){
        currentState = "HumanoidIdle";
        animator = GetComponent<Animator>();
    }

    public void ChangeAnimation(string newState, float crossFadeTime = 0.2f){
        if(currentState == newState){
            return;
        }
        animator.CrossFade(newState, crossFadeTime);
        currentState = newState;
    }

}
