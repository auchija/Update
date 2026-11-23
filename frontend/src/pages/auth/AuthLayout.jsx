import { useRef, useEffect } from "react";
import { Link } from "react-router-dom";
import { motion } from "motion/react";
import { ArrowLeft } from "lucide-react";
import { MAIN_CONTENT_ID } from "../../constants/pageAnchors.js";
import Logo from "../../components/Logo.jsx";
import { SITE_NAME } from "../../config.js";
import styles from "./Auth.module.css";

const TESTIMONIALS = [
  {
    id: 1,
    text: "Encontré un inversionista para mi emprendimiento en menos de dos semanas. La plataforma conecta a las personas indicadas.",
    avatar: "https://images.unsplash.com/photo-1534751516642-a1af1ef26a56?w=96&h=96&q=80&auto=format&fit=crop&crop=faces",
    name: "Valentina Torres",
    role: "Fundadora, GreenPack",
  },
  {
    id: 2,
    text: "Como mentor, encontré proyectos con visión real. Ahora acompaño a tres equipos y es la parte más satisfactoria de mi semana.",
    avatar: "https://images.unsplash.com/photo-1552358155-515e264cb8b8?w=96&h=96&q=80&auto=format&fit=crop&crop=faces",
    name: "Carlos Mendoza",
    role: "Mentor de producto",
  },
  {
    id: 3,
    text: "Me uní a un equipo como desarrolladora y el proyecto ya tiene sus primeros clientes. Todo empezó con un mensaje en UpDate.",
    avatar: "https://images.unsplash.com/photo-1604874156629-5c6c3b3bded7?w=96&h=96&q=80&auto=format&fit=crop&crop=faces",
    name: "Isabela Ruiz",
    role: "Desarrolladora fullstack",
  },
  {
    id: 4,
    text: "Publiqué mi emprendimiento un viernes. El lunes ya tenía conversaciones con dos posibles compradores.",
    avatar: "https://images.unsplash.com/photo-1667312147803-4b2437b5485e?w=96&h=96&q=80&auto=format&fit=crop&crop=faces",
    name: "Andrés Ospina",
    role: "Fundador, LogiQ",
  },
  {
    id: 5,
    text: "Como proveedor, antes tocaba puertas en frío. Ahora los emprendedores me encuentran a mí.",
    avatar: "https://images.unsplash.com/photo-1573497161161-c3e73707e25c?w=96&h=96&q=80&auto=format&fit=crop&crop=faces",
    name: "Luisa Herrera",
    role: "Proveedora de packaging",
  },
  {
    id: 6,
    text: "Invertí en dos proyectos que conocí aquí. El filtro por tipo de emprendimiento me ahorra tiempo valioso.",
    avatar: "https://images.unsplash.com/photo-1782323709687-43675cbddd46?w=96&h=96&q=80&auto=format&fit=crop&crop=faces",
    name: "Miguel Pardo",
    role: "Inversionista ángel",
  },
];

export default function AuthLayout({ pageTitle, children }) {
  return (
    <div className={styles.page}>
      <title>{`${pageTitle} | ${SITE_NAME}`}</title>

      <main id={MAIN_CONTENT_ID} className={styles.formSide}>
        <motion.div
          className={styles.formInner}
          initial={{ opacity: 0, y: 20 }}
          animate={{ opacity: 1, y: 0 }}
          transition={{ duration: 0.5 }}
        >
          <div className={styles.formTop}>
            <Logo />
          </div>

          <div className={styles.card}>{children}</div>

          <Link to="/" className={styles.backLink}>
            <ArrowLeft size={16} strokeWidth={1.75} aria-hidden="true" />
            Volver al inicio
          </Link>
        </motion.div>
      </main>

      <PanelSide />
    </div>
  );
}

const SPACING = 28;
const DOT_RADIUS = 1.5;
const WAVE_LENGTH = 35;
const WAVE_FALLOFF = 180;
const WAVE_AMP = 10;
const WAVE_SPEED = 3;

function PanelSide() {
  const canvasRef = useRef(null);
  const mouseRef = useRef({ x: -9999, y: -9999 });
  const timeRef = useRef(0);

  useEffect(() => {
    const canvas = canvasRef.current;
    if (!canvas) return;
    const ctx = canvas.getContext("2d");

    function resize() {
      canvas.width = canvas.offsetWidth;
      canvas.height = canvas.offsetHeight;
    }
    resize();
    const ro = new ResizeObserver(resize);
    ro.observe(canvas);

    let rafId;
    function animate() {
      timeRef.current += 0.016;
      ctx.clearRect(0, 0, canvas.width, canvas.height);

      const mx = mouseRef.current.x;
      const my = mouseRef.current.y;
      const cols = Math.ceil(canvas.width / SPACING) + 1;
      const rows = Math.ceil(canvas.height / SPACING) + 1;

      for (let row = 0; row < rows; row++) {
        for (let col = 0; col < cols; col++) {
          const gx = col * SPACING;
          const gy = row * SPACING;
          const dist = Math.sqrt((gx - mx) ** 2 + (gy - my) ** 2);
          const envelope = Math.exp(-dist / WAVE_FALLOFF);
          const ripple = Math.sin(dist / WAVE_LENGTH - timeRef.current * WAVE_SPEED) * envelope * WAVE_AMP;
          const intensity = Math.abs(ripple) / WAVE_AMP;

          ctx.globalAlpha = 0.18 + intensity * 0.82;
          ctx.fillStyle = "#facc15";
          ctx.beginPath();
          ctx.arc(gx, gy + ripple, DOT_RADIUS * (1 + intensity * 0.6), 0, Math.PI * 2);
          ctx.fill();
        }
      }
      ctx.globalAlpha = 1;
      rafId = requestAnimationFrame(animate);
    }
    animate();

    return () => {
      cancelAnimationFrame(rafId);
      ro.disconnect();
    };
  }, []);

  function handleMouseMove(e) {
    const canvas = canvasRef.current;
    if (!canvas) return;
    const rect = canvas.getBoundingClientRect();
    mouseRef.current = { x: e.clientX - rect.left, y: e.clientY - rect.top };
  }

  function handleMouseLeave() {
    mouseRef.current = { x: -9999, y: -9999 };
  }

  return (
    <aside
      className={styles.panelSide}
      aria-hidden="true"
      onMouseMove={handleMouseMove}
      onMouseLeave={handleMouseLeave}
    >
      <canvas ref={canvasRef} className={styles.flickerCanvas} />
      <div className={styles.testimonialTrack}>
        <div className={styles.marquee}>
          {[...TESTIMONIALS, ...TESTIMONIALS].map((testimonial, index) => (
            <TestimonialCard key={index} testimonial={testimonial} />
          ))}
        </div>
      </div>
      <div className={styles.fadeLeft} />
      <div className={styles.fadeRight} />
    </aside>
  );
}

function TestimonialCard({ testimonial }) {
  return (
    <figure className={styles.testimonialCard}>
      <blockquote className={styles.testimonialText}>
        &ldquo;{testimonial.text}&rdquo;
      </blockquote>
      <figcaption className={styles.testimonialAuthor}>
        <div className={styles.testimonialAvatar}>
          <img
            src={testimonial.avatar}
            alt={testimonial.name}
            width={40}
            height={40}
            loading="lazy"
            decoding="async"
          />
        </div>
        <div>
          <div className={styles.testimonialName}>{testimonial.name}</div>
          <div className={styles.testimonialRole}>{testimonial.role}</div>
        </div>
      </figcaption>
    </figure>
  );
}
