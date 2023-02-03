using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public string customName; 

    protected State mainStateType; // default/starting state for this machine

    public State CurrentState { get; private set; }
    protected State nextState;

    // Update is called once per frame
    void Update()
    {
        //if a new state has been set since the previous frame, change to it
        if (nextState != null)
        {
            SetState(nextState);
        }

        //States are not mono behaviors so we have to call OnUpdate for them
        if (CurrentState != null)
            CurrentState.OnUpdate();
    }

    //performs all state transition behavior
    private void SetState(State _newState)
    {
        nextState = null;
        if (CurrentState != null)
        {
            CurrentState.OnExit();
        }
        CurrentState = _newState;
        CurrentState.OnEnter(this);
    }

    //Sets the new state to be transitioned to next Update() frame
    public void SetNextState(State _newState)
    {
        if (_newState != null)
        {
            nextState = _newState;
        }
    }

    //States are not mono behaviors so we have to call LateUpdate for them
    private void LateUpdate()
    {
        if (CurrentState != null)
            CurrentState.OnLateUpdate();
    }

    //States are not mono behaviors so we have to call FixedUpdate for them
    private void FixedUpdate()
    {
        if (CurrentState != null)
            CurrentState.OnFixedUpdate();
    }

    public void SetNextStateToMain()
    {
        nextState = mainStateType;
    }

    private void Awake()
    {
        SetNextStateToMain();

    }


    private void OnValidate()
    {
        if (mainStateType == null)
        {
            if (customName == "Combat")
            {
                mainStateType = new IdleCombatState();
            }
        }
    }
}