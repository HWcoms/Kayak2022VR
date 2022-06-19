using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class DVector3 {
    public double x;
    public double y;
    public double z;

    public DVector3(double x, double y, double z) {
        this.x = x;
        this.y = y;
        this.z = z;
    }
    public DVector3(Vector3 v) {
        this.x = v.x;
        this.y = v.y;
        this.z = v.z;
    }

    public DVector3(DVector3 v) {
        this.x = v.x;
        this.y = v.y;
        this.z = v.z;
    }
    public static DVector3 operator +(DVector3 a, DVector3 b) {
        return new DVector3(a.x + b.x, a.y + b.y, a.z + b.z);
    }

    public static DVector3 operator -(DVector3 a, DVector3 b) {
        return new DVector3(a.x - b.x, a.y - b.y, a.z - b.z);
    }

    public static DVector3 operator *(DVector3 a, DVector3 b) {
        return new DVector3(a.x * b.x, a.y * b.y, a.z * b.z);
    }

    public static DVector3 operator *(float a, DVector3 b) {
        return new DVector3(a * b.x, a * b.y, a * b.z);
    }

    public static DVector3 operator *(DVector3 a, float b) {
        return new DVector3(a.x * b, a.y * b, a.z * b);
    }

    public double magnitude{
        get {
            return Mathf.Sqrt((float)(x * x + y * y + z * z));
        }
    }


    public DVector3 Clone () {
        return new DVector3(x, y, z);
    }

    public Vector3 ToVector3() => new Vector3((float)x, (float)y, (float)z);

    public override string ToString() {
        return string.Format("[{0}, {1}, {2}]", x, y, z);
    }

}


public class DRopeSegment {
    private DVector3 _currentPosition;


    private DVector3 _oldPosition;

    public DVector3 oldPosition {
        get {
            return _oldPosition;
        }
        set {
            _oldPosition.x = value.x;
            _oldPosition.y = value.y;
            _oldPosition.z = value.z;
        }
    }

    public DVector3 currentPosition {
        get {
            return _currentPosition;
        }
        set {
            _currentPosition.x = value.x;
            _currentPosition.y = value.y;
            _currentPosition.z = value.z;
        }
    }


    public DRopeSegment(Vector3 position) {
        this._currentPosition = new DVector3(position.x, position.y, position.z);
        this._oldPosition = new DVector3(position.x, position.y, position.z);
    }

} 


public class Rope : MonoBehaviour
{

    private LineRenderer lineRenderer;
    private List<DRopeSegment> ropeSegments = new List<DRopeSegment>();
    
    [SerializeField]
    private int numberOfSegments = 20;

    [SerializeField]
    [Range(0f, 1f)]
    private float ropeSegmentLength = 0.25f;

    [SerializeField]
    private float ropeWidth = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();


        Vector3 startingPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);

        for (int i = 0; i < numberOfSegments; i++) {
            ropeSegments.Add(new DRopeSegment(startingPosition));

            startingPosition.y -= ropeSegmentLength;
        }
        
    }

    public void Update() {
        
        DrawRope();
    }


    public void FixedUpdate() {
        Simulate();
    }


    private void Simulate () {

        // CONSTRAINTS

        DVector3 force = new DVector3(0f, -0.1f, 0f);

        for (int i = 0; i < ropeSegments.Count; i++) {
            DRopeSegment currentSegment = ropeSegments[i];

            DVector3 currentPositionSave = currentSegment.currentPosition.Clone();
            DVector3 oldPositionSave = currentSegment.oldPosition.Clone();
   
            DVector3 velocity = currentPositionSave - oldPositionSave;

            currentSegment.currentPosition += velocity;
            currentSegment.currentPosition += force * Time.fixedDeltaTime;

            DVector3 newVelocityAfterChange = currentSegment.currentPosition - currentSegment.oldPosition;
            currentSegment.oldPosition = currentPositionSave;

            RaycastHit hit;

            bool hasHit = Physics.Raycast(oldPositionSave.ToVector3(), newVelocityAfterChange.ToVector3(), out hit, (float)newVelocityAfterChange.magnitude);
            Debug.DrawRay(oldPositionSave.ToVector3(), newVelocityAfterChange.ToVector3(), Color.red);

            if (hasHit) {


                DVector3 hitNormal = new DVector3(hit.normal);
                DVector3 hitPoint = new DVector3(hit.point);

                // debug  hit name
                // get the  point of contact based on normal and hit point
                DVector3 CurrentToHitPoint = (hitPoint - currentSegment.oldPosition) ;

                DVector3 newOldPosition = currentSegment.oldPosition + (CurrentToHitPoint + velocity)  * hitNormal ; 

                currentSegment.oldPosition = newOldPosition;
            }

        }
    }



    void ApplyConstraint() {
      

    }

    private void DrawRope() {
        lineRenderer.startWidth = ropeWidth;
        lineRenderer.endWidth = ropeWidth;      

        lineRenderer.positionCount = ropeSegments.Count;
        // use lineRenderer.setPositions to set the positions of the line renderer
        
        lineRenderer.SetPositions(ropeSegments.ConvertAll(segment => segment.currentPosition.ToVector3()).ToArray());

    }

}
