using UnityEngine;
using UnityEngine.XR;
using System;
using System.IO;

public static class DemoWorld {
 public static GameObject Box(string name,Vector3 pos,Vector3 size,Color color){var g=GameObject.CreatePrimitive(PrimitiveType.Cube);g.name=name;g.transform.position=pos;g.transform.localScale=size;var shader=Shader.Find("Standard");if(shader==null)shader=Shader.Find("Universal Render Pipeline/Lit");g.GetComponent<Renderer>().material=new Material(shader){color=color};return g;}
 public static void Light(){if(UnityEngine.Object.FindObjectOfType<Light>()==null){var g=new GameObject("Sun");var l=g.AddComponent<Light>();l.type=LightType.Directional;l.intensity=1.2f;g.transform.rotation=Quaternion.Euler(50,-35,0);}RenderSettings.ambientLight=new Color(.5f,.55f,.65f);}
 public static Camera Camera(Vector3 pos){var c=UnityEngine.Camera.main;if(c==null){var g=new GameObject("Main Camera");g.tag="MainCamera";c=g.AddComponent<Camera>();g.AddComponent<AudioListener>();}c.transform.position=pos;return c;}
 public static void Save(string name,string json){File.WriteAllText(Path.Combine(Application.persistentDataPath,name),json);}
 public static string Load(string name){var p=Path.Combine(Application.persistentDataPath,name);return File.Exists(p)?File.ReadAllText(p):null;}
}
public class DemoNavigator:MonoBehaviour {
 public Camera view; public Action<GameObject> interact; float yaw,pitch; bool lastTrigger;
 void Update(){
  if(XRSettings.isDeviceActive){
   view.transform.localPosition=InputTracking.GetLocalPosition(XRNode.Head);
   view.transform.localRotation=InputTracking.GetLocalRotation(XRNode.Head);
   var device=InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
   bool trigger;device.TryGetFeatureValue(CommonUsages.triggerButton,out trigger);
   Vector3 p;Quaternion r;
   if(device.TryGetFeatureValue(CommonUsages.devicePosition,out p)&&device.TryGetFeatureValue(CommonUsages.deviceRotation,out r)&&trigger&&!lastTrigger)Hit(transform.TransformPoint(p),transform.rotation*r*Vector3.forward);
   lastTrigger=trigger;
   Vector2 axis;if(InputDevices.GetDeviceAtXRNode(XRNode.LeftHand).TryGetFeatureValue(CommonUsages.primary2DAxis,out axis)){var f=view.transform.forward;f.y=0;transform.position+=(f.normalized*axis.y+view.transform.right*axis.x)*Time.deltaTime*2;}
  }else{
   if(Input.GetMouseButton(1)){yaw+=Input.GetAxis("Mouse X")*2;pitch=Mathf.Clamp(pitch-Input.GetAxis("Mouse Y")*2,-75,75);view.transform.localRotation=Quaternion.Euler(pitch,yaw,0);}
   var f=view.transform.forward;f.y=0;transform.position+=(f.normalized*Input.GetAxis("Vertical")+view.transform.right*Input.GetAxis("Horizontal"))*Time.deltaTime*3;
   if(Input.GetKeyDown(KeyCode.E))Hit(view.transform.position,view.transform.forward);
  }
 }
 void Hit(Vector3 origin,Vector3 direction){RaycastHit h;if(Physics.Raycast(origin,direction,out h,20))interact?.Invoke(h.collider.gameObject);}
}
