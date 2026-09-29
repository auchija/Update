// Página principal ("/"). Se lee de arriba abajo en el mismo orden en que se ve:
// cabecera, portada, cómo funciona, para quién es, llamado final y pie de página.
import {
  Briefcase,
  CalendarDays,
  Check,
  GraduationCap,
  Handshake,
  Lightbulb,
  Rocket,
  ShoppingBag,
  TrendingUp,
  Truck,
  UserRound,
  UsersRound,
} from "lucide-react";
import Avatar from "../components/Avatar.jsx";
import Badge from "../components/Badge.jsx";
import Button from "../components/Button.jsx";
import Card from "../components/Card.jsx";
import Footer from "../components/Footer.jsx";
import Header from "../components/Header.jsx";
import { SITE_NAME } from "../config.js";
import { PERSONAS } from "../data/personas.js";
import styles from "./Landing.module.css";

// Los 5 pasos del flujo del producto. Los eventos todavía son una propuesta.
const STEPS = [
  {
    icon: UserRound,
    title: "Crea tu perfil",
    text: "Elige cómo participas en la comunidad (puedes ser varias cosas a la vez) y qué te interesa.",
  },
  {
    icon: Rocket,
    title: "Publica tu proyecto",
    text: "Explica qué construyes, en qué etapa va y qué necesitas para avanzar.",
  },
  {
    icon: Handshake,
    title: "Conecta",
    text: "Encuentra personas que ya recorrieron el camino y conversa con ellas.",
  },
  {
    icon: Briefcase,
    title: "Encuentra oportunidades",
    text: "Socios, mentorías, convocatorias e inversión para tu etapa.",
  },
  {
    icon: CalendarDays,
    title: "Asiste a eventos",
    text: "Encuentros y comunidades de emprendedores cerca de ti.",
    isUpcoming: true,
  },
];

// Icono de cada persona. Los textos vienen de data/personas.js, la misma lista del registro.
const PERSONA_ICONS = {
  entrepreneur: Lightbulb,
  buyer: ShoppingBag,
  mentor: GraduationCap,
  investor: TrendingUp,
  supplier: Truck,
  collaborator: UsersRound,
};

export default function Landing() {
  return (
    <div className={styles.page}>
      <title>{SITE_NAME}</title>
      <Header showSectionLinks />

      <main id="contenido">
        {/* ===== Portada ===== */}
        <section aria-labelledby="hero-title" className={styles.hero}>
          <div className={`container ${styles.heroInner}`}>
            <div className={styles.heroContent}>
              <h1 id="hero-title" className={styles.heroTitle}>
                Construye tu proyecto con las personas correctas
              </h1>
              <p className={styles.heroLead}>
                Muestra lo que estás construyendo y conecta con mentores, socios e inversionistas
                que entienden tu etapa. Menos currículum, más proyecto.
              </p>
              <div className="cluster">
                <Button to="/registro" size="lg">
                  Crear mi perfil
                </Button>
                <Button href="#como-funciona" variant="secondary" size="lg">
                  Ver cómo funciona
                </Button>
              </div>
            </div>

            <ProductPreview />
          </div>
        </section>

        {/* ===== Cómo funciona ===== */}
        <section
          id="como-funciona"
          aria-labelledby="how-title"
          className={`${styles.section} ${styles.sectionSunken}`}
        >
          <div className={`container ${styles.sectionInner}`}>
            <div className={styles.sectionHeader}>
              <h2 id="how-title" className={styles.sectionTitle}>
                Del perfil a la oportunidad, en cinco pasos
              </h2>
              <p className="text-muted">
                En {SITE_NAME} tu proyecto es tu carta de presentación. A partir de él llegan las
                conexiones y las oportunidades.
              </p>
            </div>

            <ol className={styles.steps}>
              {STEPS.map((step, index) => (
                <li key={step.title} className={styles.step}>
                  <div className={styles.stepTop}>
                    <span className={styles.iconCircle}>
                      <step.icon aria-hidden="true" className="icon" />
                    </span>
                    <span className={styles.stepNumber}>{index + 1}</span>
                  </div>
                  <h3 className={styles.stepTitle}>{step.title}</h3>
                  <p className="text-sm text-muted">{step.text}</p>
                  {step.isUpcoming && <Badge variant="accent">Próximamente</Badge>}
                </li>
              ))}
            </ol>
          </div>
        </section>

        {/* ===== Para quién es ===== */}
        <section id="para-quien" aria-labelledby="audiences-title" className={styles.section}>
          <div className={`container ${styles.sectionInner}`}>
            <div className={styles.sectionHeader}>
              <h2 id="audiences-title" className={styles.sectionTitle}>
                Un lugar para cada lado del emprendimiento
              </h2>
              <p className="text-muted">
                Al crear tu cuenta eliges cómo participas, y puedes marcar varias opciones. Cada una
                ve {SITE_NAME} desde su necesidad.
              </p>
            </div>

            <div className={styles.audiences}>
              {PERSONAS.map((persona) => {
                const Icon = PERSONA_ICONS[persona.value];
                return (
                  <Card as="article" key={persona.value} className={styles.audience}>
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
              })}
            </div>
          </div>
        </section>

        {/* ===== Llamado final ===== */}
        <section aria-labelledby="cta-title" className={styles.cta}>
          <div className="container">
            <div className={styles.ctaBand}>
              <div className="stack stack--xs">
                <h2 id="cta-title" className={styles.ctaTitle}>
                  Tu próximo socio puede estar a una conexión
                </h2>
                <p>Crea tu perfil y empieza a conectar con quienes pueden impulsar tu proyecto.</p>
              </div>
              <div className="cluster">
                <Button to="/registro" variant="secondary" size="lg">
                  Crear mi perfil
                </Button>
              </div>
            </div>
          </div>
        </section>
      </main>

      <Footer />
    </div>
  );
}

// Ilustración de la portada: una tarjeta de proyecto y una solicitud de conexión encima.
// Es decorativa (aria-hidden), por eso sus "botones" son solo texto con aspecto de botón.
function ProductPreview() {
  return (
    <div aria-hidden="true" className={styles.preview}>
      <Card elevated className={styles.project}>
        <div className={styles.person}>
          <Avatar name="Valentina Ospina" decorative />
          <div className={styles.personText}>
            <span className="text-strong">Valentina Ospina</span>
            <span className="text-sm text-muted">Emprendedora en Cali</span>
          </div>
        </div>
        <div className="stack stack--xs">
          <Badge variant="accent">MVP en pruebas</Badge>
          <p className={styles.projectTitle}>Cosecha Directa</p>
          <p className="text-sm text-muted">
            Conecta a productores del Valle del Cauca con restaurantes de la ciudad, sin
            intermediarios. Busca un mentor en logística de última milla.
          </p>
        </div>
        <div className="cluster cluster--xs">
          <Badge>Agrotech</Badge>
          <Badge>Marketplace</Badge>
          <Badge variant="primary">Busca mentor</Badge>
        </div>
        <div className={styles.stats}>
          <span>
            <b>38</b> conexiones
          </span>
          <span>
            <b>12</b> restaurantes piloto
          </span>
        </div>
        <div className={styles.overlapSpace} />
      </Card>

      <Card elevated compact className={styles.request}>
        <div className={styles.person}>
          <Avatar name="Laura Restrepo" size="sm" decorative />
          <div className={`${styles.personText} text-sm`}>
            <span>
              <b>Laura Restrepo</b> quiere conectar contigo
            </span>
            <span className="text-muted">Mentora en logística, 12 años</span>
          </div>
        </div>
        <div className="cluster cluster--xs">
          <span className={`${styles.fakeButton} ${styles.fakeButtonPrimary}`}>Aceptar</span>
          <span className={styles.fakeButton}>Ver perfil</span>
        </div>
      </Card>
    </div>
  );
}
