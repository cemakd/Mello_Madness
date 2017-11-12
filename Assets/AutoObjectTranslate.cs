﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Automated Object Translator.
[RequireComponent(typeof(Rigidbody2D))]     //Must be attached to a kinematic rigidbody.
public class AutoObjectTranslate : MonoBehaviour
{
    private Transform selfTransform;
    private float traveledDistance = 0;                 //Keeps track of how far along its path this has moved
    private float pathDistance = 0;                     //Distance that the platform must travel.
    private float speed = 0;                            //Scalar speed
    private Vector3 startingPosition = Vector3.zero;    //Starting position
    private Vector3 previousNode = Vector3.zero;        //Last visted node
    private Vector3 nextNode = Vector3.zero;            //Next node
    private int nextIndex = 0;                          //Index of the next node
    private bool reverse = true;                       //Whether the direciton of traversal has been reversed

    public List<Vector3> nodes;             //Nodes are defined relative to the starting position of the platform.

    //Movement Parameters
    public float originalVelocity = 1;      //Velocity of the platform
    public float minimumVelocity = .5f;
    public float accelerationOffset = 0;    //Between [0,1] Distance that the platform must be from its path midpoint
                                            //prior to experiencing acceleration.


    private void Awake()
    {
        DisableEditorGrids();
        selfTransform = this.GetComponent<Transform>();
        startingPosition = selfTransform.position;
        speed = originalVelocity;
        nextNode = nodes[0];
        previousNode = nodes[1];
        ComputePathDistance();
    }

    private void FixedUpdate()
    {
        
        UpdateNodes();

        
        if(accelerationOffset < 1)
        {
            ComputeSpeed();
        }
        

        
        ComputeDisplacement();
        
        
    }

    //Make sure that editor grid components are disabled on this object and all children.
    private void DisableEditorGrids()
    {
        EditorGrid selfEditorGrid = this.GetComponent<EditorGrid>();

        if(selfEditorGrid != null)
        {
            selfEditorGrid.enabled = false;
        }

        EditorGrid[] childEditorGrids = this.GetComponentsInChildren<EditorGrid>();

        foreach(EditorGrid target in childEditorGrids)
        {
            target.enabled = false;
        }

        return;
    }

    //Compute the path distance defined by the nodes.
    private void ComputePathDistance()
    {
        Vector3 current = nodes[0];
        Vector3 next = nodes[0];

        //Look at the next pair of nodes, and add their distance to the total.
        foreach(Vector3 targetNode in nodes)
        {
            current = next;
            next = targetNode;
            pathDistance += Vector3.Distance(current, next);
        }
    }

    //Compute the speed
    private void ComputeSpeed()
    {
        float pathOffset = ComputePathOffset();

        if(pathOffset > accelerationOffset)
        {
            //How far along the region in which it should accelerate that the platform is.
            float accelerationFactor = (pathOffset - accelerationOffset) / (1 - accelerationOffset);
            speed = Mathf.Lerp(originalVelocity, minimumVelocity, accelerationFactor);
        }
    }

    //Compute the offset from path midpoint.
    private float ComputePathOffset()
    {
        float travelFactor = traveledDistance / pathDistance;
        return Mathf.Abs(travelFactor - .5f) / .5f;
        
    }

    //Update Nodes
    private void UpdateNodes()
    {
        //If we have reached the next node
        if(selfTransform.position == startingPosition + nextNode)
        {
            //If the final node, reverse direction and reset traveled distance
            if( (((nextIndex + 1) == nodes.Count) && !reverse) ||
                ((nextIndex == 0) && reverse))
            {
                reverse = !reverse;
                traveledDistance = 0;
            }

            previousNode = nextNode;
            nextIndex += (reverse ? -1 : 1);
            nextNode = nodes[nextIndex];
        }
    }

    //Clamping is to prevent gradual offset over time.
    private void ComputeDisplacement()
    {
        //Determine distance between two active nodes and also distance from previous node.
        float distanceBetween = Vector3.Magnitude(nextNode - previousNode);
        float distanceFromPrevious = Vector3.Magnitude(selfTransform.position - (previousNode + startingPosition));

        //Increment distance, and normalize for distance between two active nodes.
        distanceFromPrevious += speed * Time.fixedDeltaTime;
        float displacementFactor = Mathf.Clamp01(distanceFromPrevious / distanceBetween);

        //Update distance from previous node
        Vector3 displacementFromPrevious = (nextNode - previousNode) * displacementFactor;
        Vector3 globalPosition = displacementFromPrevious + startingPosition + previousNode;

        //Update distance traveled
        traveledDistance += Vector3.Magnitude(globalPosition - selfTransform.position);

        //Update the position of this object.
        selfTransform.position = globalPosition;
    }
    
}