import {
  Check,
  GraduationCap,
  Lightbulb,
  ShoppingBag,
  TrendingUp,
  Truck,
  UsersRound,
} from "lucide-react";
import Card from "../../components/Card.jsx";
import { SITE_NAME } from "../../config.js";
import { LANDING_SECTION_IDS } from "../../constants/pageAnchors.js";
import { PERSONAS } from "../../data/personas.js";
import styles from "./AudiencesSection.module.css";
import LandingSection from "./LandingSection.jsx";

const PERSONA_ICONS = {
  entrepreneur: Lightbulb,
  buyer: ShoppingBag,
  mentor: GraduationCap,
  investor: TrendingUp,
  supplier: Truck,
  collaborator: UsersRound,
};

export default function AudiencesSection() {
  return (
    <LandingSection
      id={LANDING_SECTION_IDS.audiences}
      title="Un lugar para cada lado del emprendimiento"
      description={`Al crear tu cuenta eliges cómo participas, y puedes marcar varias opciones. Cada una ve ${SITE_NAME} desde su necesidad.`}
    >
      <div className={styles.audiences}>
        {PERSONAS.map((persona) => (
          <AudienceCard key={persona.value} persona={persona} />
        ))}
      </div>
    </LandingSection>
  );
}

function AudienceCard({ persona }) {
  const Icon = PERSONA_ICONS[persona.value];

  return (
    <Card as="article" className={styles.audience}>
      <span className={styles.iconSquare}>
        <Icon aria-hidden="true" className="icon icon--lg" />
      </span>
      <div className="stack stack--xs">
        <h3 className={styles.audienceTitle}>{persona.title}</h3>
        <p className="text-muted">{persona.text}</p>
      </div>
      <ul className={styles.benefits}>
        {persona.benefits.map((benefit) => (
          <li key={benefit} className={styles.benefit}>
            <Check aria-hidden="true" className={`icon icon--sm ${styles.check}`} />
            {benefit}
          </li>
        ))}
      </ul>
    </Card>
  );
}
