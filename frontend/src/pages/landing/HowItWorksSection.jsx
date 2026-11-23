import { useState, useEffect, useRef } from "react";
import { motion, useReducedMotion, useScroll, useTransform } from "motion/react";
import Badge from "../../components/Badge.jsx";
import { SITE_NAME } from "../../config.js";
import { LANDING_SECTION_IDS } from "../../constants/pageAnchors.js";
import styles from "./HowItWorksSection.module.css";

const STEPS = [
  {
    title: "Crea tu perfil",
    text: "Elige cómo participas en la comunidad (puedes ser varias cosas a la vez) y qué te interesa.",
    image: "https://images.unsplash.com/photo-1448932223592-d1fc686e76ea?w=600&q=80&fit=crop&auto=format",
  },
  {
    title: "Publica tu proyecto",
    text: "Explica qué construyes, en qué etapa va y qué necesitas para avanzar.",
    image: "https://images.unsplash.com/photo-1556761175-5973dc0f32e7?w=600&q=80&fit=crop&auto=format",
  },
  {
    title: "Conecta",
    text: "Encuentra personas que ya recorrieron el camino y conversa con ellas.",
    image: "https://images.unsplash.com/photo-1523908511403-7fc7b25592f4?w=600&q=80&fit=crop&auto=format",
  },
  {
    title: "Encuentra oportunidades",
    text: "Socios, mentorías, convocatorias e inversión para tu etapa.",
    image: "https://images.unsplash.com/photo-1557804506-669a67965ba0?w=600&q=80&fit=crop&auto=format",
  },
  {
    title: "Asiste a eventos",
    text: "Encuentros y comunidades de emprendedores cerca de ti.",
    image: "https://images.unsplash.com/photo-1790725673673-547b23f1ee61?w=600&q=80&fit=crop&auto=format",
    isUpcoming: true,
  },
];

const fadeUp = {
  hidden: { opacity: 0, y: 20 },
  show: { opacity: 1, y: 0, transition: { duration: 0.7, ease: [0.22, 1, 0.36, 1] } },
};

const cardVariants = {
  hidden: { opacity: 0, y: 32 },
  show: { opacity: 1, y: 0, transition: { duration: 0.6, ease: [0.22, 1, 0.36, 1] } },
};

export default function HowItWorksSection() {
  const outerRef = useRef(null);
  const trackRef = useRef(null);
  const containerRef = useRef(null);
  const scrollDistanceRef = useRef(0);
  const reduceMotion = useReducedMotion();
  const [scrollDistance, setScrollDistance] = useState(0);
  const [isMobile, setIsMobile] = useState(() => window.innerWidth < 640);

  useEffect(() => {
    function checkMobile() {
      setIsMobile(window.innerWidth < 640);
    }
    window.addEventListener("resize", checkMobile);
    return () => window.removeEventListener("resize", checkMobile);
  }, []);

  const { scrollYProgress } = useScroll({
    target: outerRef,
    offset: ["start start", "end end"],
  });

  // useTransform with function form reads from ref each tick — no stale closure
  const x = useTransform(scrollYProgress, (p) => -p * scrollDistanceRef.current);

  useEffect(() => {
    scrollDistanceRef.current = scrollDistance;
  }, [scrollDistance]);

  useEffect(() => {
    function measure() {
      if (!trackRef.current || !containerRef.current) return;
      const dist = Math.max(0, trackRef.current.scrollWidth - containerRef.current.clientWidth);
      setScrollDistance(dist);
    }
    measure();
    window.addEventListener("resize", measure);
    return () => window.removeEventListener("resize", measure);
  }, []);

  return (
    <div
      ref={outerRef}
      className={styles.outerScroll}
      style={reduceMotion || isMobile ? {} : { height: `calc(100svh + ${scrollDistance}px)` }}
    >
      <div
        ref={containerRef}
        className={reduceMotion || isMobile ? styles.staticWrapper : styles.sticky}
      >
        <svg
          className={styles.wave}
          viewBox="0 0 1440 120"
          preserveAspectRatio="none"
          aria-hidden="true"
        >
          <path d="M0 70 C 240 130 480 10 720 45 C 960 80 1200 125 1440 50 V120 H0 Z" />
        </svg>

        <div className={styles.background}>
          <section
            id={LANDING_SECTION_IDS.howItWorks}
            aria-labelledby="como-funciona-titulo"
            className={styles.section}
          >
            <div className="container">
              <motion.div
                className={styles.header}
                variants={{ hidden: {}, show: { transition: { staggerChildren: 0.12 } } }}
                initial={reduceMotion ? false : "hidden"}
                whileInView="show"
                viewport={{ once: true, margin: "-80px" }}
              >
                <motion.h2
                  id="como-funciona-titulo"
                  className={styles.sectionTitle}
                  variants={reduceMotion ? {} : fadeUp}
                >
                  Del perfil a la oportunidad, en{" "}
                  <em className={styles.accentWord}>cinco pasos</em>
                </motion.h2>
                <motion.p className="text-muted" variants={reduceMotion ? {} : fadeUp}>
                  En {SITE_NAME} tu proyecto es tu carta de presentación. A partir de él llegan
                  las conexiones y las oportunidades.
                </motion.p>
              </motion.div>
            </div>

            <div className={styles.trackOuter}>
              <motion.div
                ref={trackRef}
                style={reduceMotion || isMobile ? {} : { x }}
                className={styles.track}
              >
                {STEPS.map((s, i) => (
                  <StepCard key={s.title} step={s} number={i + 1} reduceMotion={reduceMotion} />
                ))}
              </motion.div>
            </div>
          </section>
        </div>
      </div>
    </div>
  );
}

function StepCard({ step, number, reduceMotion }) {
  return (
    <motion.article
      variants={reduceMotion ? {} : cardVariants}
      whileHover="hover"
      className={styles.card}
    >
      <div className={styles.imageContainer}>
        <img
          src={step.image}
          alt=""
          loading="lazy"
          decoding="async"
          draggable={false}
          className={styles.image}
        />
        <span className={styles.stepBadge}>{number}</span>
      </div>
      <div className={styles.cardMeta}>
        <div className={styles.cardInfo}>
          <div className={styles.cardTop}>
            <h3 className={styles.cardTitle}>{step.title}</h3>
            {step.isUpcoming && <Badge variant="accent">Próximamente</Badge>}
          </div>
          <p className={styles.cardText}>{step.text}</p>
        </div>
      </div>
    </motion.article>
  );
}
