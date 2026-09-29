import { Briefcase, CalendarDays, Handshake, Rocket, UserRound } from "lucide-react";
import Badge from "../../components/Badge.jsx";
import { SITE_NAME } from "../../config.js";
import { LANDING_SECTION_IDS } from "../../constants/pageAnchors.js";
import styles from "./HowItWorksSection.module.css";
import LandingSection from "./LandingSection.jsx";

const STEPS = [
  {
    Icon: UserRound,
    title: "Crea tu perfil",
    text: "Elige cómo participas en la comunidad (puedes ser varias cosas a la vez) y qué te interesa.",
  },
  {
    Icon: Rocket,
    title: "Publica tu proyecto",
    text: "Explica qué construyes, en qué etapa va y qué necesitas para avanzar.",
  },
  {
    Icon: Handshake,
    title: "Conecta",
    text: "Encuentra personas que ya recorrieron el camino y conversa con ellas.",
  },
  {
    Icon: Briefcase,
    title: "Encuentra oportunidades",
    text: "Socios, mentorías, convocatorias e inversión para tu etapa.",
  },
  {
    Icon: CalendarDays,
    title: "Asiste a eventos",
    text: "Encuentros y comunidades de emprendedores cerca de ti.",
    isUpcoming: true,
  },
];

export default function HowItWorksSection() {
  return (
    <LandingSection
      id={LANDING_SECTION_IDS.howItWorks}
      sunken
      title="Del perfil a la oportunidad, en cinco pasos"
      description={`En ${SITE_NAME} tu proyecto es tu carta de presentación. A partir de él llegan las conexiones y las oportunidades.`}
    >
      <ol className={styles.steps}>
        {STEPS.map((step, index) => (
          <Step key={step.title} number={index + 1} {...step} />
        ))}
      </ol>
    </LandingSection>
  );
}

function Step({ number, Icon, title, text, isUpcoming = false }) {
  return (
    <li className={styles.step}>
      <div className={styles.stepTop}>
        <span className={styles.iconCircle}>
          <Icon aria-hidden="true" className="icon" />
        </span>
        <span className={styles.stepNumber}>{number}</span>
      </div>
      <h3 className={styles.stepTitle}>{title}</h3>
      <p className="text-sm text-muted">{text}</p>
      {isUpcoming && <Badge variant="accent">Próximamente</Badge>}
    </li>
  );
}
