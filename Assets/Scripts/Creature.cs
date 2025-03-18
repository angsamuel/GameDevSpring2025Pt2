using UnityEngine;

public class Creature : MonoBehaviour
{
    public CharacterController controller;
    public AnimationStateChanger animationStateChanger;
    public float speed = 5f;

    void Awake(){
        controller = GetComponent<CharacterController>();
        animationStateChanger = GetComponent<AnimationStateChanger>();
    }

    void Update(){

    }

    public void Stop(){
        animationStateChanger.ChangeAnimation("HumanoidIdle");
    }

    public void Move(Vector3 direction){
        if(direction == Vector3.zero){
            return;
        }
        direction.Normalize();
        animationStateChanger.ChangeAnimation("HumanoidWalk");
        controller.Move(direction * speed * Time.deltaTime);
    }

    public void MoveToward(Vector3 target){
        Vector3 direction = target - transform.position;
        Move(direction);
    }
}
