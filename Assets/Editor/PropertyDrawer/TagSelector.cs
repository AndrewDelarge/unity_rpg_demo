using System.Collections.Generic;
using Player;
using UnityEditor;
using UnityEngine;


namespace Editor.PropertyDrawer
{
    [CustomPropertyDrawer(typeof(TagSelectorAttribute))]
    public class TagSelector : UnityEditor.PropertyDrawer
    {
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType == SerializedPropertyType.String)
            {
                EditorGUI.BeginProperty(position, label, property);
    
                var attrib = this.attribute as TagSelectorAttribute;
                
                {
                    List<string> tagList = new List<string>();
                    tagList.Add("<NoTag>");
                    tagList.AddRange(UnityEditorInternal.InternalEditorUtility.tags);
                    
                    string propertyString = property.stringValue;
                    int index = -1;
                    
                    if(propertyString == "")
                    {
                        index = 0;
                    } 
                    else
                    {
                        for (int i = 1; i < tagList.Count; i++)
                        {
                            if (tagList[i] == propertyString)
                            {
                                index = i;
                                break;
                            }
                        }
                    }
                    
                    index = EditorGUI.Popup(position, label.text, index, tagList.ToArray());
    
                    if(index==0)
                    {
                        property.stringValue = "";
                    }
                    else if (index >= 1)
                    {
                        property.stringValue = tagList[index];
                    }
                    else
                    {
                        property.stringValue = "";
                    }
                }
    
                EditorGUI.EndProperty();
            }
            else
            {
                EditorGUI.PropertyField(position, property, label);
            }
        }
        
        
    }
}
