
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
#if UNITY_EDITOR 
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.OdinInspector.Editor.Examples;
#endif
namespace CosmosDev.UniCosm
{

#if UNITY_EDITOR
public class UnicosmTools : OdinEditorWindow
{
    [MenuItem("Tools/Unicosm/Unicosm Tools")]    
    private static void OpenTools()
    {
        OdinEditorWindow wnd = GetWindow<UnicosmTools>();
        // Limit size of the window
        //wnd.minSize = new Vector2(720, 300);
        //wnd.maxSize = new Vector2(720, 300);
        wnd.Show(); 
    }
 
    [HideLabel]
    [DisplayAsString(false, 20, TextAlignment.Center)]
    [PropertySpace(SpaceBefore = 15, SpaceAfter = 15)]
    public string CustomFontSizeAlignmentAndRichText = "Soon!";
  
} 
public class UnicosmInformations : OdinEditorWindow
{
    [MenuItem("Tools/Unicosm/About us")]    
	private static void OpenTools()
    {
        OdinEditorWindow wnd = GetWindow<UnicosmInformations>();
        // Limit size of the window
        wnd.minSize = new Vector2(560, 250);
        wnd.maxSize = new Vector2(560, 250);
        wnd.Show(); 
        
    } 
    
 
    [ButtonGroup]
    [PropertySpace(SpaceBefore = 15)]
    [Button(50)]
    public void MainWebsite() {
        Application.OpenURL("https://github.com/cosmDev");
    }
    [ButtonGroup]    
    [Button(50)]
    [PropertySpace(SpaceBefore = 15)]
    public void WhatIsCosmos() { 
        Application.OpenURL("https://docs.cosmos.network");
    }
    [ButtonGroup]
    [PropertySpace(SpaceBefore = 15)]
    [Button(50)]
    public void ViewDoc() { 
        Application.OpenURL("https://cosmdev.github.io/unicosm-doc/");
    }

    [ButtonGroup]    
    [Button(50)]
    [PropertySpace(SpaceBefore = 15, SpaceAfter = 15)]
    public void AboutUs() { 
        Application.OpenURL("https://github.com/cosmDev");
    }
 
 
    [OnInspectorGUI("GUIBefore")]
    [PropertyOrder(-100)]
    [PropertySpace(SpaceBefore = 15, SpaceAfter = 15)]
 
    private void GUIBefore()
    {
        Texture banner = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Unicosm/icon/unicosmbanner.png", typeof(Texture));            
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.Space();
        //GUILayout.Box(banner); 
        GUILayout.Box(banner); //, GUILayout.Width(240), GUILayout.Height(100)
        EditorGUILayout.Space();
        EditorGUILayout.EndHorizontal();
    } 


} 





#endif
}