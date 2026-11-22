import os, json, uuid, getpass, base64
from urllib.request import Request,urlopen
from urllib.error import HTTPError
from urllib.parse import urlparse
from types import SimpleNamespace
base=os.environ.get('UPDATE_API','http://localhost:5080').rstrip('/')
if urlparse(base).hostname not in {'localhost','127.0.0.1','::1'}:
    raise SystemExit('Usa una API local de pruebas.')
def r(m,p,t=None,**kw):
    url=p if p.startswith(base+'/') else base+p
    headers={'Authorization':'Bearer '+t} if t else {}
    body=None
    if 'json' in kw:
        body=json.dumps(kw['json']).encode()
        headers['Content-Type']='application/json'
    if 'files' in kw:
        limite=uuid.uuid4().hex
        nombre,datos=kw['files']['archivo']
        cabecera=f'--{limite}\r\nContent-Disposition: form-data; name="proposito"\r\n\r\n{kw["data"]["proposito"]}\r\n--{limite}\r\nContent-Disposition: form-data; name="archivo"; filename="{nombre}"\r\nContent-Type: application/octet-stream\r\n\r\n'
        body=cabecera.encode()+datos+f'\r\n--{limite}--\r\n'.encode()
        headers['Content-Type']='multipart/form-data; boundary='+limite
    try:
        response=urlopen(Request(url,body,headers,method=m),timeout=30)
    except HTTPError as error:
        response=error
    with response:
        raw=response.read()
        return SimpleNamespace(status=response.status,content=raw,json=lambda:json.loads(raw),url=url)
def ok(res,n):
    if res.status!=n:
        raise RuntimeError(f'{res.url}: esperaba {n}, recibi {res.status}: {res.content[:300]!r}')
    return res
def login(i):
 return ok(r('POST','/api/auth/login',json={'identifier':f'demo_backend2_{i}','password':os.getenv(f'UPDATE_DEMO_CLAVE_{i}') or getpass.getpass(f'Clave demo {i}: ')}),200).json()['accessToken']
a,b=login(1),login(2)
categorias=ok(r('GET','/api/mis-emprendimientos/categorias',a),200).json()
d={'nombre':'Prueba temporal','categoriaId':categorias[0]['id'],'etapa':'validacion'}
e=ok(r('POST','/api/mis-emprendimientos',a,json=d),201).json()
ep='/api/mis-emprendimientos/'+e['id']
d['nombre']='Editado'
ok(r('GET',ep,b),404);ok(r('PUT',ep,b,json=d),404);ok(r('PUT',ep,a,json=d),200)
ok(r('DELETE',ep,a),204);ok(r('GET',ep,a),404)
for t,otro in [(a,'_2'),(b,'_1')]:
 posts=ok(r('GET','/api/feed/mias',t),200).json(); assert posts and all(p['esMia'] for p in posts) and not any('demo_backend2'+otro in p['contenido'] for p in posts)
assert ok(r('GET','/api/feed',a),200).json()
ok(r('GET','/api/feed/mias'),401)
p=b'%PDF-1.4\n%%EOF'
def subir(name,purpose,data=p):
 return r('POST','/api/archivos/subir',a,files={'archivo':(name,data)},data={'proposito':purpose})
ok(subir('x.jpg','publicacion'),400);ok(subir('x.pdf','foto_perfil'),400);ok(subir('x.pdf','publicacion',b'x'*(5*1024*1024+1)),400)
f=ok(subir('demo.pdf','publicacion'),201).json()
assert r('GET',f['url']).content==p
assert any(x['id']==f['id'] for x in ok(r('GET','/api/archivos/mios',a),200).json())
assert not any(x['id']==f['id'] for x in ok(r('GET','/api/archivos/mios',b),200).json())
entrada={'contenido':'Con adjunto','imagenArchivoId':f['id']}
pub=ok(r('POST','/api/feed',a,json=entrada),201).json();assert pub['adjuntoTipoMime']=='application/pdf'
ruta='/api/feed/'+pub['id']; borrar='/api/archivos/mios/'+f['id']
ok(r('DELETE',borrar,b),403);ok(r('DELETE',borrar,a),409)
ok(r('PUT',ruta,b,json=entrada),403);ok(r('DELETE',ruta,b),403)
ok(r('PUT',ruta,a,json={'contenido':'Sin adjunto'}),200)
ok(r('DELETE',borrar,a),204);ok(r('GET',f['url']),404)
ok(r('DELETE',ruta,a),204);ok(r('GET',ruta,a),404)
print('OK: CRUD, seeds idempotentes, dos usuarios, historiales, limite, extensiones y ciclo PDF.')

png=base64.b64decode("iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAQAAAC1HAwCAAAAC0lEQVR42mP8/x8AAusB9Wl2YgAAAABJRU5ErkJggg==")
def imagen(proposito):
    return ok(subir('demo.png',proposito,png),201).json()
foto=imagen('foto_perfil')
perfil=ok(r('GET','/api/perfil/yo',a),200).json()
cambio={k:perfil.get(k) for k in ['nombreVisible','nombreUsuario','titular','biografia','sitioWeb','fotoArchivoId']}
anterior=dict(cambio)
cambio['fotoArchivoId']=foto['id']
ok(r('PUT','/api/perfil/yo',a,json=cambio),200)
assert ok(r('GET','/api/perfil/yo',a),200).json()['fotoArchivoId']==foto['id']
ok(r('PUT','/api/perfil/yo',a,json=anterior),200)
ok(r('DELETE','/api/archivos/mios/'+foto['id'],a),204)
portada=imagen('emprendimiento')
d['portadaArchivoId']=portada['id']
propio=ok(r('POST','/api/mis-emprendimientos',a,json=d),201).json()
assert ok(r('GET',propio['portadaUrl'],a),200).content==png
ok(r('DELETE','/api/mis-emprendimientos/'+propio['id'],a),204)
ok(r('DELETE','/api/archivos/mios/'+portada['id'],a),204)
print('OK: foto de perfil y portada de emprendimiento.')
producto=os.getenv('UPDATE_DEMO_PRODUCTO_ID')
if producto:
    adjunto=imagen('producto')
    enlace='/api/mis-productos/'+producto+'/archivos/'+adjunto['id']
    ok(r('PUT',enlace,b),403)
    ok(r('PUT',enlace,a),204)
    ok(r('DELETE','/api/archivos/mios/'+adjunto['id'],a),409)
    ok(r('DELETE',enlace,a),204)
    ok(r('DELETE','/api/archivos/mios/'+adjunto['id'],a),204)
    print('OK: archivo de producto.')
else:
    print('Producto sin probar. Ejecuta producto.sql y configura UPDATE_DEMO_PRODUCTO_ID.')
