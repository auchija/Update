import { useState, useEffect, useRef } from "react";
import { Link } from "react-router-dom";
import { ArrowRight } from "lucide-react";
import { motion, useReducedMotion, useInView } from "motion/react";
import { LANDING_SECTION_IDS } from "../../constants/pageAnchors.js";
import styles from "./HeroSection.module.css";

const INDUSTRIES = [
  { name: "Fintech", className: `${styles.brand} ${styles.brandSerif}` },
  { name: "EDTECH", className: `${styles.brand} ${styles.brandTracking}` },
  { name: "greentech", className: `${styles.brand} ${styles.brandMedium}` },
  { name: "Salud", className: `${styles.brand} ${styles.brandTight}` },
  { name: "LOGÍSTICA", className: `${styles.brand} ${styles.brandWide}` },
  { name: "agrotech", className: `${styles.brand} ${styles.brandMono}` },
];

const METRICS = [
  { value: "500+", label: "Emprendedores registrados" },
  { value: "150+", label: "Conexiones realizadas" },
  { value: "$2.5M", label: "En inversión conectada" },
  { value: "4.8★", label: "Calificación promedio" },
];

const container = {
  hidden: {},
  show: { transition: { staggerChildren: 0.1, delayChildren: 0.05 } },
};

const item = {
  hidden: { opacity: 0, y: 20 },
  show: {
    opacity: 1,
    y: 0,
    transition: { duration: 0.6, ease: [0.22, 1, 0.36, 1] },
  },
};

const headline = {
  hidden: { opacity: 0, y: 28, filter: "blur(10px)" },
  show: {
    opacity: 1,
    y: 0,
    filter: "blur(0px)",
    transition: { duration: 0.8, ease: [0.22, 1, 0.36, 1] },
  },
};

const group = {
  hidden: {},
  show: { transition: { staggerChildren: 0.05 } },
};

const groupItem = {
  hidden: { opacity: 0, y: 12 },
  show: {
    opacity: 1,
    y: 0,
    transition: { duration: 0.5, ease: [0.22, 1, 0.36, 1] },
  },
};

function MetricCard({ value, label }) {
  const ref = useRef(null);
  const isInView = useInView(ref, { once: true, margin: "-40px" });

  const prefix = value.match(/^[^0-9]*/)?.[0] ?? "";
  const numStr = value.match(/[0-9.]+/)?.[0] ?? "0";
  const suffix = value.match(/[^0-9.]+$/)?.[0] ?? "";
  const target = parseFloat(numStr);
  const decimals = numStr.includes(".") ? 1 : 0;

  const [display, setDisplay] = useState(`${prefix}0${suffix}`);

  useEffect(() => {
    if (!isInView) return undefined;
    const duration = 1400;
    const startTime = performance.now();
    let frame;

    function tick(now) {
      const progress = Math.min((now - startTime) / duration, 1);
      const eased = 1 - Math.pow(1 - progress, 3); // easeOutCubic
      setDisplay(`${prefix}${(target * eased).toFixed(decimals)}${suffix}`);
      if (progress < 1) frame = requestAnimationFrame(tick);
    }

    frame = requestAnimationFrame(tick);
    // Si la tarjeta se desmonta a mitad de la animación, no seguir actualizando el estado.
    return () => cancelAnimationFrame(frame);
  }, [isInView, prefix, target, decimals, suffix]);

  return (
    <motion.div ref={ref} variants={groupItem} className={styles.metricCard}>
      <dt className={styles.metricLabel}>{label}</dt>
      <dd className={styles.metricValue}>{display}</dd>
    </motion.div>
  );
}

export default function HeroSection() {
  const reduceMotion = useReducedMotion();
  const [arrowHovered, setArrowHovered] = useState(false);

  return (
    <section aria-labelledby="hero-titulo" className={styles.hero}>
      <motion.div
        variants={container}
        initial={reduceMotion ? false : "hidden"}
        animate="show"
        className={styles.inner}
      >
        <motion.h1 id="hero-titulo" variants={headline} className={styles.title}>
          Muestra lo que construyes
          <br />
          Conecta con quien <span className={styles.titleAccent}>importa</span>
        </motion.h1>

        <motion.p variants={item} className={styles.subtitle}>
          UpDate te conecta con mentores, socios e inversionistas que entienden tu etapa. Menos
          currículum, más proyecto.
        </motion.p>

        <motion.div variants={item} className={styles.actions}>
          <Link
            to="/registro"
            className={styles.ctaPrimary}
            onMouseEnter={() => setArrowHovered(true)}
            onMouseLeave={() => setArrowHovered(false)}
          >
            Crear mi perfil
            <ArrowRight
              size={16}
              strokeWidth={1.75}
              className={styles.arrowIcon}
              style={{ transform: arrowHovered ? "translateX(3px)" : "translateX(0)" }}
            />
          </Link>
          <a href={`#${LANDING_SECTION_IDS.howItWorks}`} className={styles.ctaSecondary}>
            Ver cómo funciona
          </a>
        </motion.div>

        <motion.div variants={group} className={styles.trustSection}>
          <motion.p variants={groupItem} className={styles.trustLabel}>
            Proyectos activos en industrias como
          </motion.p>
          <div className={styles.brandsList}>
            <div className={styles.brandsTrack} aria-hidden="true">
              {[...INDUSTRIES, ...INDUSTRIES, ...INDUSTRIES, ...INDUSTRIES].map((ind, i) => (
                <span key={`${ind.name}-${i}`} className={ind.className}>
                  {ind.name}
                </span>
              ))}
            </div>
          </div>
        </motion.div>

        <motion.dl variants={group} className={styles.metricsGrid}>
          {METRICS.map((metric) => (
            <MetricCard key={metric.label} value={metric.value} label={metric.label} />
          ))}
        </motion.dl>
      </motion.div>
    </section>
  );
}
