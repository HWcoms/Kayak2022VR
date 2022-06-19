using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
    public GameObject ropeEndObj;
    public Transform ParentPos;
    private LineRenderer line;
    private List<RopeSegment> ropeSegments = new List<RopeSegment>();

    public float ropeSegLen = 0.25f;
    public int segmentLength = 35;
    [Range(0, .9f)] public float elasticity = 0.5f;

    public Vector3 force;

    public struct RopeSegment
    {
        public Vector3 posNow;
        public Vector3 posOld;
        public RopeSegment(Vector3 pos)
        {
            posNow = pos;
            posOld = pos;
        }
    }
   
    public void DrawRope()
    {
        Vector3[] ropePositions = new Vector3[segmentLength];

        for(int i=0; i< segmentLength; i++)
        {
            ropePositions[i] = ropeSegments[i].posNow;
        }

        line.positionCount = ropePositions.Length;
        line.SetPositions(ropePositions);
    }

    public void Awake()
    {
        line = GetComponent<LineRenderer>();

        Vector3 ropeStartPoint = ParentPos.position;

        for(int i = 0; i<segmentLength; i++)
        {
            ropeSegments.Add(new RopeSegment(ropeStartPoint));
            ropeStartPoint.y -= ropeSegLen;
        }
    }

    public void Update()
    {
        DrawRope();
    }

    public void FixedUpdate()
    {
        Simulate();
    }


    public void Simulate()
    {
        for(int i = 1; i< segmentLength; i++)
        {
            RopeSegment firstSegment = ropeSegments[i];
            Vector3 velocity = firstSegment.posNow - firstSegment.posOld;
            firstSegment.posOld = firstSegment.posNow;
            firstSegment.posNow += velocity;
            firstSegment.posNow += force * Time.fixedDeltaTime;
            ropeSegments[i] = firstSegment;
        }

        for(int i=0; i<50; i++)
        {
            Physics();
        }
    }

    private void Physics()
    {
        RopeSegment zero = ropeSegments[0];
        zero.posNow = ParentPos.position;
        ropeSegments[0] = zero;

        for(int i = 0; i<segmentLength -1; i++)
        {
            RopeSegment firstSeg = ropeSegments[i];
            RopeSegment secondSeg = ropeSegments[i + 1];

            float dist = (firstSeg.posNow - secondSeg.posNow).magnitude;
            float error = Mathf.Abs(dist - ropeSegLen);
            Vector3 changeDir = Vector3.zero;

            if(dist > ropeSegLen)
            {
                changeDir = (firstSeg.posNow - secondSeg.posNow).normalized;
            }
            else if (dist < ropeSegLen)
            {
                changeDir = (secondSeg.posNow - firstSeg.posNow).normalized;
            }

            Vector3 changeAmount = changeDir * error;
            if(i !=0)
            {
                firstSeg.posNow -= changeAmount * elasticity;
                ropeSegments[i] = firstSeg;
                secondSeg.posNow += changeAmount * elasticity;
                ropeSegments[i + 1] = secondSeg;
            }
            else
            {
                secondSeg.posNow += changeAmount;
                ropeSegments[i + 1] = secondSeg;
            }
        }

        ropeEndObj.transform.position = ropeSegments[segmentLength - 1].posNow;
    }
}
