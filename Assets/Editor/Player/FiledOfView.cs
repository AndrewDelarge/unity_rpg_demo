using Player;
using UnityEditor;
using UnityEngine;


namespace Editor.Player
{

    [CustomEditor(typeof(PlayerController))]
    public class FiledOfView : UnityEditor.Editor
    {
        private void OnSceneGUI()
        {
            PlayerController pc = (PlayerController) target;
            
            Handles.color = Color.white;
            Handles.DrawWireArc(pc.transform.position, Vector3.up, Vector3.forward, 360, pc.viewRadius);
            Vector3 viewAngleA = pc.DirFromAngle(-pc.viewAngle / 2, false);
            Vector3 viewAngleB = pc.DirFromAngle(pc.viewAngle / 2, false);
            
            Handles.DrawLine(pc.transform.position, pc.transform.position + viewAngleA * pc.viewRadius);
            Handles.DrawLine(pc.transform.position, pc.transform.position + viewAngleB * pc.viewRadius);
        }
    }
}