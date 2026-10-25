from pathlib import Path
import json, shutil
ROOT=Path(__file__).resolve().parent
TARGET=ROOT/'UnityProject'
files={'Assets/MetaSonGi/IRobotTransport.cs':'IRobotTransport.cs','Assets/MetaSonGi/DemoApp.cs':'DemoApp.cs','Assets/MetaSonGi/SharedRuntime.cs':'SharedRuntime.cs','Assets/Editor/DemoSetup.cs':'DemoSetup.cs'}
for target,source in files.items():
 p=TARGET/target;p.parent.mkdir(parents=True,exist_ok=True)
 if p.exists() and p.read_bytes()!=(ROOT/source).read_bytes():raise SystemExit(f'Refusing to overwrite changed file: {p}')
 shutil.copyfile(ROOT/source,p)
manifest=TARGET/'Packages/manifest.json';manifest.parent.mkdir(parents=True,exist_ok=True)
if not manifest.exists():manifest.write_text(json.dumps({'dependencies':{'com.unity.modules.inputlegacy':'1.0.0','com.unity.modules.audio':'1.0.0','com.unity.modules.imgui':'1.0.0','com.unity.modules.jsonserialize':'1.0.0','com.unity.modules.physics':'1.0.0','com.unity.modules.unitywebrequest':'1.0.0','com.unity.modules.xr':'1.0.0'}},indent=2))
settings=TARGET/'ProjectSettings';settings.mkdir(exist_ok=True)
version=settings/'ProjectVersion.txt'
if not version.exists():version.write_text('m_EditorVersion: 6000.3.0f1\n')
print('Add this folder in Unity Hub (Unity 6, Built-in renderer):',TARGET)
print('In Unity: Active Input Handling = Both, then MetaSonGi > Create Demo Scene > Play')
