import { createHmac } from 'node:crypto';

const clave = process.env.Jwt__Clave;
if (!clave || Buffer.byteLength(clave, 'utf8') < 32)
  throw new Error('Configura Jwt__Clave con al menos 32 bytes; debe coincidir con la API local.');
const ahora = Math.floor(Date.now() / 1000);
const codificar = objeto => Buffer.from(JSON.stringify(objeto)).toString('base64url');
const contenido = codificar({ alg: 'HS256', typ: 'JWT' }) + '.' + codificar({
  iss: process.env.Jwt__Emisor ?? 'UpDate',
  aud: process.env.Jwt__Audiencia ?? 'UpDate.API',
  sub: 'administrador-desarrollo', rol: 'administrador',
  iat: ahora, nbf: ahora, exp: ahora + 1800
});
console.log(contenido + '.' + createHmac('sha256', clave).update(contenido).digest('base64url'));
