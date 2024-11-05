using System;
using System.Collections;

public class BaseNode
{
    public enum NodeState
    {
        RUNNING = 0,
        SUCCESS,
        FAILURE
    }
    private BehaviourTree bTree;
    protected NodeState _nodeState;
    public NodeState nodeState
    {
        get => _nodeState;
    }

    public BaseNode(BehaviourTree bt)
    {
        bTree = bt;
    }

    

}
