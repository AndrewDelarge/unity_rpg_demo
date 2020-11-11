using UnityEditor;
using UnityEngine;


namespace Editor.Actors
{

    [CustomEditor(typeof(global::Gameplay.Actors.Base.CommonAnimator))]
    public class CommonAnimator : UnityEditor.Editor
    {
        private void OnSceneGUI()
        {
            global::Gameplay.Actors.Base.CommonAnimator animator = (global::Gameplay.Actors.Base.CommonAnimator) target;

           
            Handles.color = animator.isLookAtEnabled ? Color.red : Color.gray;
            
            Handles.DrawLine(animator.transform.position, animator.lookPoint.position);
        }
    }
}