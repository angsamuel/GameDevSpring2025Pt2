using UnityEngine;
using UnityEngine.AI;

public class VillagerAI : BaseAI
{


    public Creature creature;
    public GameObject foodReturnObject; //where we drop off the food
    public float checkFoodRadius = 10f;
    NavMeshAgent navMeshAgent;

    protected void Awake(){
        base.Awake();
        navMeshAgent = GetComponent<NavMeshAgent>();
        creature = GetComponent<Creature>();
        //ChangeState(WanderState);
        ChangeState(PathfindCCState);
        //ChangeState(PathfindState);
    }

    void CheckForFood(){
        Collider[] colliders = Physics.OverlapSphere(transform.position, checkFoodRadius, LayerMask.GetMask("Food"));

        if(colliders.Length > 0){
            targetFood = colliders[0].gameObject;
            targetFood.gameObject.layer = LayerMask.NameToLayer("ReservedFood");
        }
    }

    Vector3 wanderPosition = Vector3.zero;
    public void WanderState(){
        stateImIn = "Wander State";
        if(stateTick == 1){
            wanderPosition = transform.position + new Vector3(Random.Range(-10f,10f),0,Random.Range(-10f,10f));
        }
        creature.MoveToward(wanderPosition);

        CheckForFood();
        if(targetFood != null){
            ChangeState(GetFoodState);
            return;
        }

        if(Vector3.Distance(transform.position, wanderPosition) < 1f){
            ChangeState(PauseState);
            return;
        }
    }

    float pauseTime = 0f;
    void PauseState(){
        stateImIn = "Pause State";
        creature.Stop();
        if(stateTick == 1){
            pauseTime = Random.Range(2f,5f);
        }

        if(stateTime > pauseTime){
            ChangeState(WanderState);
            return;
        }
    }

    public GameObject targetFood; //piece of food to pick up
    public void GetFoodState(){
        stateImIn = "Get Food State";
        if (targetFood == null)
        {
            ChangeState(PauseState);
            return;
        }

        creature.MoveToward(targetFood.transform.position);



        if(Vector3.Distance(transform.position, targetFood.transform.position) < 2f){
            creature.PickUpFood(targetFood);
            ChangeState(ReturnFoodState);
            return;
        }
    }


    void ReturnFoodState(){
        stateImIn = "Return Food State";
        creature.MoveToward(foodReturnObject.transform.position);
        if(Vector3.Distance(transform.position, foodReturnObject.transform.position) < 2f){
            Destroy(targetFood);
            ChangeState(WanderState);
            return;
        }
    }


    // void PathfindState(){
    //     navMeshAgent.isStopped = false;
    //     navMeshAgent.destination = targetFood.transform.position;
    // }

    NavMeshPath path;
    int currentCorner = 0;
    void PathfindCCState(){
        if(path == null){
            path = new NavMeshPath();
            bool pathFound = navMeshAgent.CalculatePath(targetFood.transform.position, path);
        }
        if(currentCorner >= path.corners.Length){
         //   ChangeState(WanderState);
            return;
        }

        creature.MoveToward(path.corners[currentCorner]);
        if(Vector3.Distance(transform.position, path.corners[currentCorner]) < 1f){
            currentCorner+=1;
        }
    }

}
