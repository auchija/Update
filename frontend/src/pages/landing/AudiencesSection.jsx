import { motion, useReducedMotion } from "motion/react";
import {
  ArrowRight,
  GraduationCap,
  Lightbulb,
  ShoppingBag,
  TrendingUp,
  Truck,
  UsersRound,
} from "lucide-react";
import { SITE_NAME } from "../../config.js";
import { LANDING_SECTION_IDS } from "../../constants/pageAnchors.js";
import { PERSONAS } from "../../data/personas.js";
import LandingSection from "./LandingSection.jsx";
import styles from "./AudiencesSection.module.css";

const PERSONA_ICONS = {
  entrepreneur: Lightbulb,
  buyer: ShoppingBag,
  mentor: GraduationCap,
  investor: TrendingUp,
  supplier: Truck,
  collaborator: UsersRound,
};

const PERSONA_LINKS = {
  entrepreneur: "Publica tu emprendimiento",
  buyer: "Descubre emprendimientos",
  mentor: "Ofrece mentoría",
  investor: "Encuentra proyectos",
  supplier: "Conecta con clientes",
  collaborator: "Únete a un equipo",
};

const container = {
  hidden: {},
  show: { transition: { staggerChildren: 0.06, delayChildren: 0.05 } },
};

const fadeUp = {
  hidden: { opacity: 0, y: 16 },
  show: { opacity: 1, y: 0, transition: { duration: 0.6, ease: [0.22, 1, 0.36, 1] } },
};

const fade = {
  hidden: { opacity: 0 },
  show: { opacity: 1, transition: { duration: 0.6, ease: [0.22, 1, 0.36, 1] } },
};

export default function AudiencesSection() {
  const reduce = useReducedMotion();
  const rise = reduce ? fade : fadeUp;

  return (
    <LandingSection
      id={LANDING_SECTION_IDS.audiences}
      title={
        <>
          Un lugar para cada lado del{" "}
          <em className={styles.accentWord}>emprendimiento</em>
        </>
      }
      description={`Al crear tu cuenta eliges cómo participas, y puedes marcar varias opciones. Cada una ve ${SITE_NAME} desde su necesidad.`}
    >
      <motion.div
        className={styles.audiences}
        variants={container}
        initial={reduce ? false : "hidden"}
        whileInView="show"
        viewport={{ once: true, margin: "-60px" }}
      >
        {PERSONAS.map((persona) => (
          <AudienceCard key={persona.value} persona={persona} rise={rise} />
        ))}
      </motion.div>
    </LandingSection>
  );
}

function AudienceCard({ persona, rise }) {
  const Icon = PERSONA_ICONS[persona.value];
  const link = PERSONA_LINKS[persona.value];

  return (
    <motion.article variants={rise} className={styles.card}>
      <span className={styles.iconWrap}>
        <Icon size={20} strokeWidth={1.75} aria-hidden="true" className={styles.icon} />
      </span>

      <h3 className={styles.cardTitle}>{persona.title}</h3>
      <p className={styles.cardText}>{persona.text}</p>

      <div className={styles.cardBottom}>
        <a href="#" className={styles.cardLink}>
          {link}
          <ArrowRight size={16} strokeWidth={1.75} aria-hidden="true" className={styles.arrowIcon} />
        </a>
      </div>
    </motion.article>
  );
}
