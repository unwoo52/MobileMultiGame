using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchingFSM : MonoBehaviour
{
    public enum ZOMBIE_TARGETINGSTATE
    {
        Lookaround, Tracking, LostTarget
    }

    private ZOMBIE_TARGETINGSTATE _currentState;

    private void Start()
    {
        _currentState = ZOMBIE_TARGETINGSTATE.Lookaround;
    }

    private void Update()
    {
        StateMachine();
    }

    public void StateMachine()
    {
        switch (_currentState)
        {
            case ZOMBIE_TARGETINGSTATE.Lookaround:
                break;
            case ZOMBIE_TARGETINGSTATE.Tracking:
                break;
            case ZOMBIE_TARGETINGSTATE.LostTarget:
                break;
        }
    }

    public void ChangerState(ZOMBIE_TARGETINGSTATE newState)
    {
        _currentState = newState;
        switch (_currentState)
        {
            case ZOMBIE_TARGETINGSTATE.Lookaround:
                break;
            case ZOMBIE_TARGETINGSTATE.Tracking:
                break;
            case ZOMBIE_TARGETINGSTATE.LostTarget:
                break;
        }
    }

}
