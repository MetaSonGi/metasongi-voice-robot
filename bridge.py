"""Local speech/text command queue for the Unity virtual robot. No hardware IO."""
from http.server import BaseHTTPRequestHandler, ThreadingHTTPServer
from queue import Queue, Empty, Full
import json
COMMANDS={'up','down','home','grab','release','stop','올려','내려','원위치','집어','놓아','멈춰','정지'}
queue=Queue(maxsize=16)
PAGE='''<!doctype html><html lang="ko"><meta charset="utf-8"><title>Voice Robot</title><body style="background:#111a29;color:white;font:20px system-ui;padding:40px"><h1>MetaSonGi Voice Robot</h1><p>Unity를 실행한 뒤 명령을 보내세요. 실제 모터는 제어하지 않습니다.</p><input id="text" value="up"><button id="send">보내기</button><button id="mic">한국어 음성 인식</button><button id="stop">STOP</button><p>올려 · 내려 · 원위치 · 집어 · 놓아 · 멈춰</p><pre id="status"></pre><script>
const $=x=>document.getElementById(x);async function send(text){let r=await fetch('/command',{method:'POST',headers:{'Content-Type':'application/json'},body:JSON.stringify({text})});$('status').textContent=await r.text()};$('send').onclick=()=>send($('text').value);$('stop').onclick=()=>send('stop');$('mic').onclick=()=>{let API=window.SpeechRecognition||window.webkitSpeechRecognition;if(!API){$('status').textContent='이 브라우저는 음성 인식을 지원하지 않습니다. 텍스트 명령을 사용하세요.';return}let s=new API;s.lang='ko-KR';s.onresult=e=>{let t=e.results[0][0].transcript.trim().replace(/[.!?。]/g,'');$('text').value=t;send(t)};s.onerror=e=>$('status').textContent=e.error;s.start()};</script></body></html>'''
class Handler(BaseHTTPRequestHandler):
 def reply(self,data,status=200):
  raw=json.dumps(data,ensure_ascii=False).encode();self.send_response(status);self.send_header('Content-Type','application/json; charset=utf-8');self.send_header('Content-Length',str(len(raw)));self.end_headers();self.wfile.write(raw)
 def do_GET(self):
  if self.path=='/':
   raw=PAGE.encode();self.send_response(200);self.send_header('Content-Type','text/html; charset=utf-8');self.send_header('Content-Length',str(len(raw)));self.end_headers();self.wfile.write(raw)
  elif self.path=='/next':
   try:text=queue.get_nowait()
   except Empty:text=''
   self.reply({'text':text})
  else:self.reply({'error':'not found'},404)
 def do_POST(self):
  if self.path!='/command':return self.reply({'error':'not found'},404)
  if self.headers.get('Origin') not in (None,'http://localhost:8767','http://127.0.0.1:8767'):return self.reply({'error':'origin rejected'},403)
  try:
   size=int(self.headers.get('Content-Length','0'))
   if not 0<size<=1024:raise ValueError('invalid size')
   text=json.loads(self.rfile.read(size)).get('text')
   if text not in COMMANDS:raise ValueError('unsupported command; use an exact command')
   if text in {'stop','멈춰','정지'}:
    while True:
     try:queue.get_nowait()
     except Empty:break
   queue.put_nowait(text);self.reply({'queued':text,'target':'Unity virtual robot only'})
  except (ValueError,TypeError,Full) as e:self.reply({'error':str(e)},400)
if __name__=='__main__':
 print('Open http://localhost:8767');ThreadingHTTPServer(('127.0.0.1',8767),Handler).serve_forever()
