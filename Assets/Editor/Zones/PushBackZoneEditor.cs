using Actors.Base;
using Gameplay.Zones;
using UnityEditor;
using UnityEngine;


namespace Editor.Zones
{

    [CustomEditor(typeof(PushBackZone))]
    public class PushBackZoneEditor : UnityEditor.Editor
    {
        private void OnSceneGUI()
        {
            PushBackZone zone = (PushBackZone) target;

            Handles.color = Color.blue;
            
            Handles.DrawWireArc(zone.transform.position, Vector3.up, Vector3.forward, 360, zone.radius);
            Handles.DrawWireArc(zone.transform.position, Vector3.forward, Vector3.up, 360, zone.radius);
            
        }
    }
}