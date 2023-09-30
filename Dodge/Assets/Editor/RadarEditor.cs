using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor (typeof (Radar))]
public class RadarEditor: Editor {

    void OnSceneGUI() {
        Radar radar = (Radar)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(radar.transform.position, Vector3.up, Vector3.forward, 360, radar.viewRadius);
        Vector3 viewAngleA = radar.DirFromAngle(-radar.viewAngle / 2, false);
        Vector3 viewAngleB = radar.DirFromAngle(radar.viewAngle / 2, false);

        Handles.DrawLine(radar.transform.position, radar.transform.position + viewAngleA * radar.viewRadius);
        Handles.DrawLine(radar.transform.position, radar.transform.position + viewAngleB * radar.viewRadius);

        Handles.color = Color.green;
        foreach(Transform visibleTarget in radar.visibleTargets) {
            Handles.DrawLine(radar.transform.position, visibleTarget.position);
        }
    }

    [InitializeOnLoadMethod]
    static void Initialize() {
        EditorApplication.update += EditorUpdate;
    }

    static void EditorUpdate() {
        // This method gets called continuously in the Editor.
        // You can add any code here that you want to run continuously.
    }
}