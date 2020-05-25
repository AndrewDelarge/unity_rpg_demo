using Actors.Base;
using UnityEditor;
using UnityEngine;


namespace Editor.Actors
{

    [CustomEditor(typeof(global::Actors.Base.CommonAnimator))]
    public class CommonAnimator : UnityEditor.Editor
    {
        private void OnSceneGUI()
        {
            global::Actors.Base.CommonAnimator animator = (global::Actors.Base.CommonAnimator) target;

           
            Handles.color = animator.isLookAtEnabled ? Color.red : Color.gray;
            
            Handles.DrawLine(animator.transform.position, animator.lookPoint.position);
        }
    }
}