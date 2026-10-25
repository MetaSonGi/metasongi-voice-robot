using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System;

public class DemoApp:MonoBehaviour {
 Transform joint,arm,tool,crate;Vector3 target;bool held;string command="up",status="Virtual mode. Hardware transport not enabled.";
 float angle=20;bool stopped;float nextPoll;
 [Serializable]public class Command{public string text;}
 void Start(){DemoWorld.Light();DemoWorld.Box("Floor",new Vector3(0,-.2f,0),new Vector3(12,.2f,12),Color.gray);joint=new GameObject("RobotJoint").transform;joint.position=new Vector3(0,1,0);arm=DemoWorld.Box("Arm",new Vector3(0,2,0),new Vector3(.3f,2,.3f),Color.cyan).transform;arm.SetParent(joint,true);tool=DemoWorld.Box("Gripper",new Vector3(0,3,0),Vector3.one*.4f,Color.yellow).transform;tool.SetParent(joint,true);crate=DemoWorld.Box("Part",new Vector3(1,.3f,0),Vector3.one*.5f,Color.red).transform;var camera=DemoWorld.Camera(new Vector3(5,4,-7));camera.transform.LookAt(new Vector3(0,1.5f,0));StartCoroutine(Poll());}
 public void Execute(string text){text=(text??"").Trim().ToLowerInvariant();if(text=="stop"||text=="멈춰"||text=="정지"){stopped=true;status="Stopped";return;}if(text=="home"||text=="원위치"){stopped=false;angle=20;}else if(text=="up"||text=="올려"){stopped=false;angle=Mathf.Clamp(angle+10,-60,60);}else if(text=="down"||text=="내려"){stopped=false;angle=Mathf.Clamp(angle-10,-60,60);}else if(text=="grab"||text=="집어"){if(Vector3.Distance(tool.position,crate.position)>1){status="Part out of reach; use Place part near tool";return;}held=true;crate.SetParent(tool,true);}else if(text=="release"||text=="놓아"){held=false;crate.SetParent(null,true);crate.position=new Vector3(crate.position.x,.3f,crate.position.z);}else{status="Unknown command rejected";return;}status="Virtual: "+text;}
 IEnumerator Poll(){while(true){using(var r=UnityWebRequest.Get("http://127.0.0.1:8767/next")){r.timeout=2;yield return r.SendWebRequest();if(r.result==UnityWebRequest.Result.Success){var c=JsonUtility.FromJson<Command>(r.downloadHandler.text);if(c!=null&&!string.IsNullOrEmpty(c.text))Execute(c.text);}}yield return new WaitForSeconds(.25f);}}
 void Update(){if(!stopped)joint.rotation=Quaternion.RotateTowards(joint.rotation,Quaternion.Euler(0,0,angle),30*Time.deltaTime);}
 void OnGUI(){GUILayout.BeginArea(new Rect(15,15,430,320),GUI.skin.box);GUILayout.Label("VOICE ROBOT | virtual joint demo");command=GUILayout.TextField(command);if(GUILayout.Button("Execute text command"))Execute(command);GUILayout.Label("up / down / home / grab / release / stop");if(GUILayout.Button("Place part near tool")&&!held)crate.position=tool.position+Vector3.right*.5f;if(GUILayout.Button("STOP"))Execute("stop");GUILayout.Label(status);GUILayout.Label("Voice: run bridge.py, open localhost:8767");GUILayout.Label("Hardware requires a model-specific adapter. No motors are driven.");GUILayout.EndArea();}
}
