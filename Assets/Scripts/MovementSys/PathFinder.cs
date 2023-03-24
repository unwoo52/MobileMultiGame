using System.Collections;
using UnityEngine;
using UnityEngine.AI;
public class PathFinder : MonoBehaviour
{
    private AnimContoller _anim;

    private Coroutine coNavmove;

    private NavMeshPath _path = null;

    private NavMeshAgent _agent;

    private float arrivalRange = 0.1f;
    private int currentCornerIndex = 0;

    private void Awake()
    {
        if(TryGetComponent(out AnimContoller animController)) _anim = animController;
        _path = new NavMeshPath();
    }


    public void SetDestination(Vector3 dest)
    {
        _path.ClearCorners();
        _agent.ResetPath();
        if(_agent.CalculatePath(dest, _path))
        {

            StartCoroutine(WaitPathComplete(dest));
        }
    }

    IEnumerator WaitPathComplete(Vector3 dest)
    {
        while(true){
            if (_path.status == NavMeshPathStatus.PathComplete)
            {
                if (coNavmove != null) StopCoroutine(coNavmove);
                coNavmove = StartCoroutine(NavMove(_path.corners));
                yield break;
            }
            yield return null;
        }
    }

    IEnumerator NavMove(Vector3[] pointlist)
    {
        if (_path.corners.Length <= 1)
        {
            if (coNavmove != null) StopCoroutine(NavMove(pointlist));
            yield break;
        }

        Vector3 destination = transform.position - pointlist[pointlist.Length - 1];
        float distance = destination.magnitude;

        currentCornerIndex = 1;
        _anim.OnOnlyMove();
        while (true)
        {
            if (distance < arrivalRange)
            {
                currentCornerIndex++;
                if (currentCornerIndex == pointlist.Length)
                {
                    _anim.OnStopMove();
                    yield break;
                }
                Debug.Log(currentCornerIndex);
            }
            destination = transform.position - pointlist[currentCornerIndex];
            distance = destination.magnitude;
            Debug.Log("Lookat" + pointlist[currentCornerIndex]);
            _anim.OnLookat(pointlist[currentCornerIndex], 20);

            yield return null;
        }
    }
}
