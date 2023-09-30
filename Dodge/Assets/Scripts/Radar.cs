using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Radar: MonoBehaviour {

    public float viewRadius;
    [Range(0, 360)]
    public float viewAngle;

    public LayerMask targetMask;
    public LayerMask obstacleMask;

    public List<Transform> visibleTargets = new List<Transform>();
    public List<float> visibleAngles = new List<float>();

    public List<float> visibleDistance = new List<float>();

    void Start() {
        StartCoroutine("FindTargetsWithDelay", .02f);
    }

    IEnumerator FindTargetsWithDelay(float delay) {
        while (true) {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets ();
            
        }
    }

    void FindVisibleTargets() {
        visibleTargets.Clear();
        visibleAngles.Clear();
        visibleDistance.Clear();
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

        for (int i = 0; i < targetsInViewRadius.Length; i++) {
            Transform target = targetsInViewRadius[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToTarget) <= viewAngle / 2) {
                float dstToTarget = Vector3.Distance (transform.position, target.position);

                if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask)) {
                    visibleTargets.Add(target);
                    float angle = Vector3.Angle(transform.forward, dirToTarget);
                    if(transform.position.x > target.position.x) {
                        angle += 180;
                    }
                    visibleAngles.Add(angle);
                    visibleDistance.Add(dstToTarget);
                    // Draw a line from the radar to the target
                    Debug.DrawLine(transform.position, target.position, Color.red);
                }
            }
        }
    }

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal) {
        if (!angleIsGlobal) {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad),
         0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}