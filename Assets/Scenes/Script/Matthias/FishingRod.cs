using System.Collections.Generic;
using UnityEngine;

public class RopePoint {
    public Vector3 currentPosition;
    public Vector3 oldPosition;
    public bool isPinned;
    public bool isFloater;

    public RopePoint(Vector3 position, bool isPinned = false) {
        this.currentPosition = position;
        this.oldPosition = position;
        this.isPinned = isPinned;
        this.isFloater = false;
    }

}

public class RopeSegment {
    public RopePoint A;
    public RopePoint B;

    public float stress;

    public RopeSegment(RopePoint A, RopePoint B) {
        this.A = A;
        this.B = B;
    }
}



public class FishingRod : MonoBehaviour
{

    private LineRenderer lineRenderer;
    private List<RopePoint> ropePoints = new List<RopePoint>();
    private List<RopeSegment> ropeSegments = new List<RopeSegment>();

    
    [Header("Hooks Settings")]
    [SerializeField]
    private GameObject hooks;

    
    [Header("Floater Settings")]
    [SerializeField]
    private GameObject floater;

    
    [SerializeField]
    private int numberOfPointAferFloater = 15;

    private int floaterIndex {
        get {
            return ropePoints.Count - numberOfPointAferFloater;
        }
    }

    [SerializeField]
    private float stressThreshold = 10f;

    [SerializeField]
    private float floaterDragForce = 3f;




    [Header("Rope Settings")]
    [SerializeField]
    public Transform pointToFollow;
    [SerializeField]
    private int iterationNumber = 1;

    [SerializeField]
    private Material ropeMaterial;


    [SerializeField]
    private int numberOfSegments = 20;

    [SerializeField]
    private float ropeWidth = 0.2f;


    [SerializeField]
    [Range(0f, 1f)]
    private float ropeSegmentLength = 0.25f;
    [SerializeField]
    [Range(0f, 1f)]
    private float gravityForce = 0.5f; 

    [SerializeField]
    [Range(0f, 1f)]
    private float friction = 0.9f; 



    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.useWorldSpace = true;
        // change material
        lineRenderer.material = ropeMaterial;


        Vector3 startingPosition = pointToFollow.transform.position;

        for (int i = 0; i < numberOfSegments; i++) {
            ropePoints.Add(new RopePoint(startingPosition));

            startingPosition.y -= ropeSegmentLength;
        }
        ropePoints[floaterIndex].isFloater = true;
        ropePoints[0].isPinned = true;


        // add points to the rope segments
        for (int i = 0; i < ropePoints.Count - 1; i++) {
            ropeSegments.Add(new RopeSegment(ropePoints[i], ropePoints[i + 1]));

        }
        
    }

    public void AddPointAtStart() {

        ropePoints.Insert(1, new RopePoint(pointToFollow.transform.position));

        RopeSegment newSegment = new RopeSegment(ropePoints[1], ropePoints[2]);
        ropeSegments[0].B = ropePoints[1];
        ropeSegments.Insert(1, newSegment);        
    }

    public void RemovePointAtEnd() {
        ropePoints.RemoveAt(ropePoints.Count - 1);

        ropeSegments[0].B = ropeSegments[1].B;
        ropeSegments.RemoveAt(ropeSegments.Count - 1);
    
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePoints();
        for (int i = 0; i < iterationNumber; i++) {
            UpdateSegments();
            ConstraintFloater();
            ConstraintPoints();
        }
        UpdateHooksPosition();
        UpdateFloaterPosition();
        DrawRope();
    }


    private void UpdateFloaterPosition() {
        floater.transform.position = ropePoints[floaterIndex].currentPosition;
        
        // RopeSegment segment = ropeSegments[floaterIndex];
        // Vector3 direction = segment.B.currentPosition - segment.A.currentPosition;
        // floater.transform.LookAt(ropePoints[floaterIndex].currentPosition + direction);
    }

    private void UpdateHooksPosition() {
        hooks.transform.position = ropePoints[ropePoints.Count - 1].currentPosition;

        // RopeSegment lastSegment = ropeSegments[ropeSegments.Count - 1];
        // Vector3 direction = lastSegment.B.currentPosition - lastSegment.A.currentPosition;
        // hooks.transform.LookAt(ropePoints[ropePoints.Count - 1].currentPosition + direction);
    }

    private void UpdateSegments() {

        for (int i = 0; i < ropeSegments.Count; i++) {
            Vector3 A = ropeSegments[i].A.currentPosition;
            Vector3 B = ropeSegments[i].B.currentPosition;

            Vector3 AB = B - A;
            float distance = Vector3.Distance(A, B);
            float diff = ropeSegmentLength - distance;
            float percent = diff / distance / 2;
            
            ropeSegments[i].stress = diff * -1;

            if (ropeSegments[i].A.isPinned || ropeSegments[i].B.isPinned)
                percent *= 2;
            


            // FLOATER UPDATE
            Vector3 floaterDirection = new Vector3(AB.x, 0, AB.z);

            if (ropeSegments[i].A.isFloater && ropeSegments[i].A.isPinned) {
                ropeSegments[i].A.currentPosition = A - (floaterDirection * percent) ;
            }

            if (ropeSegments[i].B.isFloater && ropeSegments[i].B.isPinned) {
                ropeSegments[i].B.currentPosition = B + (floaterDirection * percent);
            }

            // SEGEMENT UPDATE
            if (!ropeSegments[i].A.isPinned) {
                ropeSegments[i].A.currentPosition = A - (AB * percent);
            }

            if (!ropeSegments[i].B.isPinned) {
                ropeSegments[i].B.currentPosition = B + (AB * percent);
            }
        }

    }


    private void ConstraintPoints(){
        for (int i = 0; i < ropePoints.Count; i++) {
            RopePoint currentSegment = ropePoints[i];

            if (currentSegment.isPinned)
                continue;
            // do constraint here

        }
    }

    private float getOverallStressToSegmentIndex(int segmentIndex) {
        float stress = 0;
        for (int i = 0; i < ropeSegments.Count; i++) {
            stress += ropeSegments[i].stress;
        }
        return stress;
    }

    private void ConstraintFloater() {
        float stress = getOverallStressToSegmentIndex(floaterIndex);

        if (ropePoints[floaterIndex].isPinned && stress > stressThreshold) {
            ropePoints[floaterIndex].isPinned = false;

        } else if (Physics.Raycast(ropePoints[floaterIndex].currentPosition, Vector3.down, out RaycastHit hit, 0.5f)) {
            if (hit.collider.tag == "Water") {
                ropePoints[floaterIndex].isPinned = true;
            }
        }
    }


    private void UpdatePoints() {

        ropeSegments[0].A.currentPosition = pointToFollow.transform.position;

        for (int i = 1; i < ropePoints.Count; i++) {
            RopePoint currentPoint = ropePoints[i];

            if (currentPoint.isPinned && !currentPoint.isFloater) {

                if (currentPoint.isFloater)
                    Debug.Log("is floater");
                continue;
            }
            Debug.Log("currentPoint.isFloater" + currentPoint.isFloater);

            Vector3 velocity = currentPoint.currentPosition - currentPoint.oldPosition;

            currentPoint.oldPosition = currentPoint.currentPosition;


            if (currentPoint.isFloater && currentPoint.isPinned) {
                Debug.Log("floater");
                velocity.y = 0;
                currentPoint.currentPosition += velocity;
            } else {
                currentPoint.currentPosition += velocity * friction;
            }

            if (!currentPoint.isFloater)
                currentPoint.currentPosition +=  Vector3.down * gravityForce * Time.deltaTime;
        }
    }

    private void DrawRope() {
        lineRenderer.startWidth = ropeWidth;
        lineRenderer.endWidth = ropeWidth;      

        lineRenderer.positionCount = ropePoints.Count;
        // use lineRenderer.setPositions to set the positions of the line renderer
        
        lineRenderer.SetPositions(ropePoints.ConvertAll(segment => segment.currentPosition).ToArray());

    }

}