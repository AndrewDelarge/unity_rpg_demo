using Actors.Base;
using UnityEditor;
using UnityEngine;


namespace Editor.Actors
{

    [CustomEditor(typeof(Vision))]
    public class FiledOfView : UnityEditor.Editor
    {
        private void OnSceneGUI()
        {
            Vision vision = (Vision) target;

           
            Handles.color = vision.isEnabled? Color.red : Color.gray;
            Handles.DrawWireArc(vision.transform.position, Vector3.up, Vector3.forward, 360, vision.viewRadius);
            Vector3 viewAngleA = vision.DirFromAngle(-vision.viewAngle / 2, false);
            Vector3 viewAngleB = vision.DirFromAngle(vision.viewAngle / 2, false);
            
            Handles.DrawLine(vision.transform.position, vision.transform.position + viewAngleA * vision.viewRadius);
            Handles.DrawLine(vision.transform.position, vision.transform.position + viewAngleB * vision.viewRadius);
        }
    }
}