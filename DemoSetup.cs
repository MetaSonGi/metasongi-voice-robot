#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using System.IO;
public static class DemoSetup {
 [MenuItem("MetaSonGi/Create Demo Scene")]
 public static void Create(){Directory.CreateDirectory("Assets/Scenes");var s=EditorSceneManager.NewScene(NewSceneSetup.EmptyScene,NewSceneMode.Single);new GameObject("DemoApp").AddComponent<DemoApp>();EditorSceneManager.SaveScene(s,"Assets/Scenes/Demo.unity");EditorBuildSettings.scenes=new[]{new EditorBuildSettingsScene("Assets/Scenes/Demo.unity",true)};Debug.Log("Open Demo scene and press Play.");}
}
#endif
