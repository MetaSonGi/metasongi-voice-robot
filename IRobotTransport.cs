using System;
public interface IRobotTransport { bool IsConnected {get;} void Stop(); void MoveJoint(int joint,float degrees,float speed); }
public sealed class DryRunRobotTransport:IRobotTransport {
 public bool IsConnected=>false;
 public string LastCommand {get;private set;}
 public void Stop(){LastCommand="stop (not transmitted)";}
 public void MoveJoint(int joint,float degrees,float speed){if(joint<0||joint>5||float.IsNaN(degrees)||float.IsInfinity(degrees)||Math.Abs(degrees)>60||float.IsNaN(speed)||float.IsInfinity(speed)||speed<=0||speed>10)throw new ArgumentOutOfRangeException("Invalid motion");LastCommand=$"joint={joint}, degrees={degrees}, speed={speed} (not transmitted)";}
}
