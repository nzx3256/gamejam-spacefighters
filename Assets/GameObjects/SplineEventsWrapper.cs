using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Splines;

[System.Serializable]
public class SplineEventsWrapper
{
    [SerializeField]
    int _knotIndex = 0;
    [SerializeField]
    int _knotNumberOfPasses = 0;
    [SerializeField]
    private UnityEvent<SplineContainer, BezierKnot, int> _onReachedKnot = null;

    public int KnotIndex
    {
        get => _knotIndex;
    }
    public int KnotNumberOfPasses
    {
        get => _knotNumberOfPasses;
    }
    public UnityEvent<SplineContainer,BezierKnot, int> OnReachedKnot
    {
        get => _onReachedKnot;
    }
}