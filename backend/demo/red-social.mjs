// Llena la red social con cuentas de ejemplo: perfiles con foto, publicaciones con imágenes,
// "me gusta", comentarios y seguidores. Pasa por la API (respeta todas las validaciones)
// y se puede correr varias veces: lo que ya existe no se duplica.
//
// Las fotos se descargan de picsum.photos (catálogo libre de fotos de Unsplash) por su id.
//
// Uso:
//   UPDATE_DEMO_CLAVE=<clave de las cuentas demo> node backend/demo/red-social.mjs
// Variables:
//   UPDATE_API          por defecto http://localhost:5195
//   UPDATE_DEMO_CLAVE   clave de todas las cuentas demo (mínimo 8 caracteres, letra y número)
//   UPDATE_DEMO_CONFIRMAR=si   obligatorio si la API no es local (para no llenar otra base por error)

const base = (process.env.UPDATE_API ?? "http://localhost:5195").replace(/\/$/, "");
const clave = process.env.UPDATE_DEMO_CLAVE;
const esLocal = ["localhost", "127.0.0.1", "[::1]"].includes(new URL(base).hostname);

if (!clave || !/^(?=.*[a-zA-Z])(?=.*[0-9]).{8,}$/.test(clave)) {
  console.error("Define UPDATE_DEMO_CLAVE (mínimo 8 caracteres, con una letra y un número).");
  process.exit(1);
}
if (!esLocal && process.env.UPDATE_DEMO_CONFIRMAR !== "si") {
  console.error(`La API ${base} no es local. Si de verdad quieres llenarla, define UPDATE_DEMO_CONFIRMAR=si.`);
  process.exit(1);
}

// ---------- Datos ----------
// foto: id de picsum. Las publicaciones sin foto son de solo texto.

const CUENTAS = [
  {
    handle: "cafe.origen",
    nombre: "Café Origen",
    personas: ["entrepreneur", "supplier"],
    titular: "Café de especialidad del Valle del Cauca",
    bio: "Tostamos en pequeños lotes y le compramos directo a 14 familias caficultoras. ☕\nPedidos para oficinas y eventos por mensaje.",
    foto: 766,
    publicaciones: [
      { foto: 1060, texto: "Así empieza cada mañana en el laboratorio: método V60, agua a 93 °C y un lote nuevo de Trujillo que nos tiene enamorados. ¿Quién quiere la cata del sábado?" },
      { foto: 431, texto: "Primer pedido corporativo cerrado gracias a una conexión que hicimos aquí en UpDate 🙌 30 oficinas van a tomar café del Valle cada semana." },
      { foto: 425, texto: "Lote 07 · Honey · Notas a panela, mandarina y cacao. Tostión media. Quedan 40 bolsas." },
      { foto: 635, texto: "Cata a ciegas con baristas de la ciudad. Lo que más aprendimos: la gente prefiere el café más dulce, no el más fuerte." },
    ],
  },
  {
    handle: "valentina.rios",
    nombre: "Valentina Ríos",
    personas: ["entrepreneur", "collaborator"],
    titular: "Diseñadora UX/UI · Fundadora de Pixel Lab",
    bio: "Ayudo a startups a que su producto se entienda en 5 segundos.\nBranding, prototipos y pruebas con usuarios.",
    foto: 64,
    publicaciones: [
      { foto: 526, texto: "Entregamos la identidad visual de una marca de cosmética natural de Cali. Paleta cálida, tipografía con personalidad y un sistema que se puede usar en redes sin diseñador." },
      { foto: 252, texto: "Antes de abrir Figma, papel. Siempre papel. Estas son las primeras ideas para el onboarding de una app de finanzas personales." },
      { texto: "Consejo para quien está lanzando su primer producto: haz 5 pruebas con usuarios reales antes de escribir una línea de código. Te vas a ahorrar meses. 5 personas bastan para encontrar el 80 % de los problemas." },
      { foto: 528, texto: "Nuevo espacio de trabajo 🪴 Si eres emprendedor y necesitas una revisión rápida de tu landing, escríbeme: esta semana tengo 3 cupos gratis." },
    ],
  },
  {
    handle: "andres.mejia",
    nombre: "Andrés Mejía",
    personas: ["entrepreneur"],
    titular: "Full-stack · Construyendo Rutalia, logística para tenderos",
    bio: "Ingeniero de sistemas. Ayudamos a las tiendas de barrio a pedir a sus proveedores desde el celular.",
    foto: 91,
    publicaciones: [
      { foto: 1, texto: "Semana 12 construyendo Rutalia. Hoy salió la primera versión del panel para proveedores: ya pueden ver los pedidos de 37 tiendas en tiempo real." },
      { foto: 180, texto: "Planeación del trimestre: menos funcionalidades, más conversaciones con tenderos. Si tienes una tienda o conoces a alguien, me encantaría escucharte." },
      { texto: "Buscamos cofundador(a) comercial que conozca el canal tradicional en el Valle. Tenemos producto, 37 tiendas usándolo y ganas. ¿Alguien que me recomienden?" },
      { foto: 445, texto: "El setup que sostiene todo: un portátil viejo, un monitor prestado y mucho café de @cafe.origen 😄" },
    ],
  },
  {
    handle: "huerta.viva",
    nombre: "Huerta Viva",
    personas: ["entrepreneur", "supplier"],
    titular: "Agricultura urbana y alimentos orgánicos",
    bio: "Cultivamos en terrazas de Cali y llevamos canastas orgánicas a domicilio. 🌱\nTambién enseñamos a montar tu propia huerta.",
    foto: 785,
    publicaciones: [
      { foto: 674, texto: "Cosecha de esta semana. Uvas Isabella de un productor aliado en La Unión, sin químicos y recogidas ayer." },
      { foto: 292, texto: "La canasta de esta semana: cebolla roja, ajo, pimienta y hierbas frescas. 120 familias ya reciben la suya cada sábado." },
      { foto: 627, texto: "Fríjol verde de nuestra huerta en el tercer piso de un edificio en San Fernando. Sí, en plena ciudad se puede." },
      { foto: 1080, texto: "Fresas para el taller de mermeladas del domingo. Quedan 6 cupos 🍓" },
    ],
  },
  {
    handle: "cuero.artesanal",
    nombre: "Taller Cuero & Co.",
    personas: ["entrepreneur"],
    titular: "Marroquinería hecha a mano en Cali",
    bio: "Billeteras, morrales y libretas en cuero colombiano. Cada pieza lleva el nombre de quien la hizo.",
    foto: 527,
    publicaciones: [
      { foto: 491, texto: "Herramientas que heredamos del abuelo. Algunas tienen más de 40 años y siguen marcando el cuero mejor que cualquier máquina." },
      { foto: 464, texto: "Nueva colección: libreta + billetera en cuero café. La pueden personalizar con sus iniciales." },
      { foto: 26, texto: "Todo lo que cabe en nuestro morral de diario. Lo diseñamos con 20 clientes que lo usaron dos meses antes de lanzarlo." },
    ],
  },
  {
    handle: "laura.gomez",
    nombre: "Laura Gómez",
    personas: ["investor", "mentor"],
    titular: "Inversionista ángel · Mentora en fintech",
    bio: "He invertido en 9 startups de LatAm. Me interesan fintech, agro y logística.\nMentorías los jueves.",
    foto: 1027,
    publicaciones: [
      { texto: "Lo que más miro en una primera reunión no es la idea, es qué tan rápido aprende el equipo. Muéstrame qué cambiaron en el último mes y por qué." },
      { foto: 3, texto: "Hoy estuve escuchando pitches de emprendedores de la región. El nivel sube cada año. Tres equipos pasan a la siguiente ronda 👏" },
      { texto: "Abro 4 cupos de mentoría para equipos en etapa de validación (fintech, agro o logística). Déjame un comentario con lo que están construyendo." },
    ],
  },
  {
    handle: "dulce.antojo",
    nombre: "Dulce Antojo",
    personas: ["entrepreneur"],
    titular: "Repostería artesanal por encargo",
    bio: "Postres sin conservantes, hechos el mismo día. 🍰 Pedidos con 48 horas de anticipación.",
    foto: 835,
    publicaciones: [
      { foto: 493, texto: "Parfait de yogur griego, granola de la casa y fresas. El más pedido de la semana." },
      { foto: 429, texto: "Probando una nueva tarta de frambuesa para la carta de diciembre. ¿La lanzamos?" },
      { foto: 488, texto: "Así preparamos los toppings para 50 postres de un evento corporativo. Gracias @valentina.rios por recomendarnos 💜" },
    ],
  },
  {
    handle: "sofia.cardenas",
    nombre: "Sofía Cárdenas",
    personas: ["collaborator", "supplier"],
    titular: "Fotógrafa de producto para marcas locales",
    bio: "Hago que tu producto se vea como se siente. 📷\nSesiones en estudio o en tu taller.",
    foto: 823,
    publicaciones: [
      { foto: 250, texto: "La cámara con la que empecé hace 8 años. Hoy hago fotos de producto para 20 marcas del Valle." },
      { foto: 531, texto: "Tip: para fotos de producto con el celular, busca luz de ventana lateral y pon una cartulina blanca al otro lado. Cero gastos, mucha diferencia." },
      { foto: 454, texto: "Sesión de hoy con @cuero.artesanal. No puedo esperar a mostrarles las fotos de la nueva colección." },
    ],
  },
  {
    handle: "mateo.londono",
    nombre: "Mateo Londoño",
    personas: ["mentor"],
    titular: "Mentor en ventas B2B · 10 años en startups",
    bio: "Ayudo a equipos técnicos a vender sin sentirse vendedores.",
    foto: 1012,
    publicaciones: [
      { foto: 513, texto: "Café, libreta y tres llamadas con fundadores. Hoy hablamos de cómo poner el primer precio." },
      { texto: "Si tu cliente no entiende tu propuesta en una frase, no es su culpa. Escríbela, léesela a alguien de otra industria y reescríbela hasta que la repita sin ayuda." },
      { foto: 192, texto: "Así se veía el coworking esta tarde: 15 emprendedores trabajando, compartiendo contactos y celebrando la primera venta de uno de ellos." },
    ],
  },
];

// Quién sigue a quién (además, todos siguen a las cuentas de mentoría).
const SEGUIMIENTOS = {
  "cafe.origen": ["huerta.viva", "dulce.antojo", "andres.mejia", "sofia.cardenas"],
  "valentina.rios": ["dulce.antojo", "cuero.artesanal", "sofia.cardenas", "cafe.origen"],
  "andres.mejia": ["cafe.origen", "valentina.rios", "huerta.viva"],
  "huerta.viva": ["cafe.origen", "dulce.antojo", "andres.mejia"],
  "cuero.artesanal": ["sofia.cardenas", "valentina.rios", "cafe.origen"],
  "laura.gomez": ["andres.mejia", "huerta.viva", "cafe.origen", "valentina.rios"],
  "dulce.antojo": ["valentina.rios", "huerta.viva", "cafe.origen", "sofia.cardenas"],
  "sofia.cardenas": ["cuero.artesanal", "dulce.antojo", "valentina.rios", "cafe.origen"],
  "mateo.londono": ["andres.mejia", "valentina.rios", "cafe.origen"],
};
const MENTORES = ["laura.gomez", "mateo.londono"];

// Comentarios: [cuenta dueña, índice de su publicación, quién comenta, texto]
const COMENTARIOS = [
  ["cafe.origen", 0, "dulce.antojo", "¡Yo voy a la cata! ¿Puedo llevar postres para maridar? 🍰"],
  ["cafe.origen", 0, "andres.mejia", "Ese lote de Trujillo es otra cosa."],
  ["cafe.origen", 1, "laura.gomez", "Así se valida un negocio: clientes que pagan cada semana. ¡Felicitaciones!"],
  ["cafe.origen", 1, "valentina.rios", "¡Qué orgullo verlos crecer! 💜"],
  ["valentina.rios", 0, "sofia.cardenas", "Esa paleta quedó preciosa. Si necesitan fotos de producto, cuenten conmigo."],
  ["valentina.rios", 2, "mateo.londono", "Totalmente. Y lo mismo aplica para ventas: 5 conversaciones antes del primer pitch."],
  ["valentina.rios", 2, "andres.mejia", "Lo aprendí a las malas 😅 Gran consejo."],
  ["valentina.rios", 3, "huerta.viva", "¡Nos interesa! Te escribo por mensaje."],
  ["andres.mejia", 0, "laura.gomez", "Me gustaría conocer las métricas de retención de esas 37 tiendas. ¿Hablamos el jueves?"],
  ["andres.mejia", 0, "mateo.londono", "Gran avance. ¿Cómo están cobrando a los proveedores?"],
  ["andres.mejia", 2, "valentina.rios", "Compartido con un par de amigos que vienen de consumo masivo 🙌"],
  ["andres.mejia", 3, "cafe.origen", "¡Gracias por el apoyo! La próxima bolsa va por nuestra cuenta ☕"],
  ["huerta.viva", 1, "dulce.antojo", "Necesito sus hierbas para la nueva línea de postres 🌿"],
  ["huerta.viva", 2, "laura.gomez", "Agricultura urbana con 120 familias recurrentes. Me encanta este modelo."],
  ["huerta.viva", 3, "valentina.rios", "¡Aparten un cupo para mí!"],
  ["cuero.artesanal", 0, "sofia.cardenas", "Estas herramientas cuentan una historia. Hay que fotografiarlas bien."],
  ["cuero.artesanal", 1, "cafe.origen", "Necesitamos 12 libretas para el equipo. ¿Hacen pedidos al por mayor?"],
  ["laura.gomez", 0, "andres.mejia", "Esto me cambió la forma de preparar las reuniones con inversionistas. Gracias."],
  ["laura.gomez", 2, "huerta.viva", "Huerta Viva: canastas orgánicas con cultivo urbano en Cali. ¡Estamos validando la suscripción mensual!"],
  ["laura.gomez", 2, "andres.mejia", "Rutalia: pedidos para tiendas de barrio desde el celular. 37 tiendas activas 🚚"],
  ["dulce.antojo", 1, "cafe.origen", "¡Lánzala! Y la acompañamos con un filtrado 😋"],
  ["dulce.antojo", 1, "sofia.cardenas", "Se ve deliciosa. ¿Hacemos fotos para la carta?"],
  ["sofia.cardenas", 1, "cuero.artesanal", "Lo probamos y funciona increíble. ¡Gracias por el tip!"],
  ["mateo.londono", 1, "valentina.rios", "Esto es oro. Lo voy a usar en el próximo taller."],
  ["mateo.londono", 2, "andres.mejia", "¡Gran tarde! Gracias por el espacio."],
];

// ---------- API ----------

async function api(ruta, { metodo = "GET", json, form, token } = {}) {
  const headers = {};
  if (token) headers.Authorization = `Bearer ${token}`;
  let body;
  if (json !== undefined) {
    headers["Content-Type"] = "application/json";
    body = JSON.stringify(json);
  } else if (form) {
    body = form;
  }
  for (let intento = 1; ; intento++) {
    const respuesta = await fetch(`${base}/api${ruta}`, { method: metodo, headers, body });
    // Render (plan gratis) puede tardar en despertar: se reintenta un par de veces.
    if (respuesta.status >= 502 && intento < 4) {
      await new Promise((r) => setTimeout(r, 5000 * intento));
      continue;
    }
    const texto = await respuesta.text();
    const datos = texto ? JSON.parse(texto) : null;
    if (!respuesta.ok) {
      const error = new Error(`HTTP ${respuesta.status} en ${metodo} ${ruta}: ${texto.slice(0, 300)}`);
      error.status = respuesta.status;
      throw error;
    }
    return datos;
  }
}

async function descargarFoto(id, ancho, alto) {
  const respuesta = await fetch(`https://picsum.photos/id/${id}/${ancho}/${alto}.jpg`);
  if (!respuesta.ok) throw new Error(`No se pudo descargar la foto ${id} de picsum (HTTP ${respuesta.status}).`);
  return new Blob([await respuesta.arrayBuffer()], { type: "image/jpeg" });
}

async function subirFoto(token, id, proposito, ancho, alto) {
  const form = new FormData();
  form.append("proposito", proposito);
  form.append("archivo", await descargarFoto(id, ancho, alto), `foto-${id}.jpg`);
  return api("/archivos/subir", { metodo: "POST", form, token });
}

async function sesion(cuenta) {
  try {
    const r = await api("/auth/login", { metodo: "POST", json: { identifier: cuenta.handle, password: clave } });
    return r.accessToken;
  } catch (error) {
    if (error.status !== 401) throw error;
  }
  const r = await api("/auth/register", {
    metodo: "POST",
    json: {
      displayName: cuenta.nombre,
      handle: cuenta.handle,
      email: `${cuenta.handle}@example.com`,
      password: clave,
      personas: cuenta.personas,
      termsAccepted: true,
    },
  });
  console.log(`  cuenta creada: @${cuenta.handle}`);
  return r.accessToken;
}

// ---------- Sembrar ----------

const tokens = {};
const publicaciones = {};

console.log(`Llenando ${base} con ${CUENTAS.length} cuentas de ejemplo…`);

for (const cuenta of CUENTAS) {
  console.log(`@${cuenta.handle}`);
  const token = (tokens[cuenta.handle] = await sesion(cuenta));

  const perfil = await api("/perfil/yo", { token });
  let fotoArchivoId = perfil.fotoArchivoId;
  if (!fotoArchivoId) fotoArchivoId = (await subirFoto(token, cuenta.foto, "foto_perfil", 400, 400)).id;
  await api("/perfil/yo", {
    metodo: "PUT",
    token,
    json: {
      nombreUsuario: cuenta.handle,
      nombreVisible: cuenta.nombre,
      titular: cuenta.titular,
      biografia: cuenta.bio,
      sitioWeb: null,
      fotoArchivoId,
    },
  });

  const existentes = await api("/feed/mias?pagina=1&tamanoPagina=100", { token });
  publicaciones[cuenta.handle] = [];
  for (const pub of cuenta.publicaciones) {
    let creada = existentes.find((p) => p.contenido === pub.texto);
    if (!creada) {
      const imagenArchivoId = pub.foto ? (await subirFoto(token, pub.foto, "publicacion", 1080, 1350)).id : null;
      creada = await api("/feed", { metodo: "POST", token, json: { contenido: pub.texto, imagenArchivoId } });
      console.log(`  publicación: ${pub.texto.slice(0, 50)}…`);
    }
    publicaciones[cuenta.handle].push(creada.id);
  }
}

console.log("Seguidores…");
for (const [quien, aQuienes] of Object.entries(SEGUIMIENTOS)) {
  const todos = new Set([...aQuienes, ...MENTORES].filter((h) => h !== quien));
  for (const handle of todos) {
    await api(`/comunidad/perfiles/${handle}/seguir`, { metodo: "POST", token: tokens[quien] });
  }
}

console.log("Me gusta…");
// Cada publicación recibe "me gusta" de una parte de las cuentas (siempre las mismas, para que
// volver a correr el script no cambie nada).
const handles = CUENTAS.map((c) => c.handle);
for (const [dueno, ids] of Object.entries(publicaciones)) {
  for (const [i, id] of ids.entries()) {
    const cantidad = 3 + ((dueno.length + i * 3) % (handles.length - 2));
    const fans = handles.filter((h) => h !== dueno).slice(0, cantidad);
    for (const fan of fans) await api(`/feed/${id}/me-gusta`, { metodo: "POST", token: tokens[fan] });
  }
}

console.log("Comentarios…");
for (const [dueno, indice, autor, texto] of COMENTARIOS) {
  const id = publicaciones[dueno][indice];
  const actuales = await api(`/feed/${id}/comentarios?pagina=1&tamanoPagina=100`, { token: tokens[autor] });
  if (!actuales.some((c) => c.autor.nombreUsuario === autor && c.contenido === texto)) {
    await api(`/feed/${id}/comentarios`, { metodo: "POST", token: tokens[autor], json: { contenido: texto } });
  }
}

const total = Object.values(publicaciones).flat().length;
console.log(`Listo: ${CUENTAS.length} cuentas, ${total} publicaciones, ${COMENTARIOS.length} comentarios.`);
