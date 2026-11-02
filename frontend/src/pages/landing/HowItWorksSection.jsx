import Badge from "../../components/Badge.jsx";
import Card from "../../components/Card.jsx";
import { SITE_NAME } from "../../config.js";
import { LANDING_SECTION_IDS } from "../../constants/pageAnchors.js";
import styles from "./HowItWorksSection.module.css";
import LandingSection from "./LandingSection.jsx";

const STEPS = [
  {
    title: "Crea tu perfil",
    text: "Elige cómo participas en la comunidad (puedes ser varias cosas a la vez) y qué te interesa.",
  },
  {
    title: "Publica tu proyecto",
    text: "Explica qué construyes, en qué etapa va y qué necesitas para avanzar.",
  },
  {
    title: "Conecta",
    text: "Encuentra personas que ya recorrieron el camino y conversa con ellas.",
  },
  {
    title: "Encuentra oportunidades",
    text: "Socios, mentorías, convocatorias e inversión para tu etapa.",
  },
  {
    title: "Asiste a eventos",
    text: "Encuentros y comunidades de emprendedores cerca de ti.",
    isUpcoming: true,
  },
];

export default function HowItWorksSection() {
  return (
    <div>
      <svg
        className={styles.wave}
        viewBox="0 0 1440 120"
        preserveAspectRatio="none"
        aria-hidden="true"
      >
        <path d="M0 70 C 240 130 480 10 720 45 C 960 80 1200 125 1440 50 V120 H0 Z" />
      </svg>
      <div className={styles.background}>
        <LandingSection
          id={LANDING_SECTION_IDS.howItWorks}
          title="Del perfil a la oportunidad, en cinco pasos"
          description={`En ${SITE_NAME} tu proyecto es tu carta de presentación. A partir de él llegan las conexiones y las oportunidades.`}
        >
          <ol className={styles.steps}>
            {STEPS.map((step, index) => (
              <Step key={step.title} number={index + 1} {...step} />
            ))}
          </ol>
        </LandingSection>
      </div>
    </div>
  );
}

function Step({ number, title, text, isUpcoming = false }) {
  return (
    <Card as="li" className={styles.step}>
      <div className={styles.stepTop}>
        <span className={styles.stepNumber} aria-hidden="true">
          {number}
        </span>
        {isUpcoming && <Badge variant="accent">Próximamente</Badge>}
      </div>
      <div className="stack stack--xs">
        <h3 className={styles.stepTitle}>{title}</h3>
        <p className="text-muted">{text}</p>
      </div>
    </Card>
  );
}
