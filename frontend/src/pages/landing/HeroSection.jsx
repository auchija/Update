import { useEffect } from "react";
import Button from "../../components/Button.jsx";
import { LANDING_SECTION_IDS } from "../../constants/pageAnchors.js";
import styles from "./HeroSection.module.css";
import ProductPreview from "./ProductPreview.jsx";

export default function HeroSection() {
  useEffect(() => {
    const handleMouseMove = (e) => {
      const x = (e.clientX / window.innerWidth) * 100;
      const y = (e.clientY / window.innerHeight) * 100;
      document.documentElement.style.setProperty("--mouse-x", `${x}%`);
      document.documentElement.style.setProperty("--mouse-y", `${y}%`);
    };

    window.addEventListener("mousemove", handleMouseMove);
    return () => window.removeEventListener("mousemove", handleMouseMove);
  }, []);

  return (
    <section aria-labelledby="hero-titulo" className={styles.hero}>
      <div className={`container ${styles.inner}`}>
        <div className={styles.content}>
          <h1 id="hero-titulo" className={styles.title}>
            Construye tu proyecto con las personas correctas
          </h1>
          <p className={styles.lead}>
            Muestra lo que estás construyendo y conecta con mentores, socios e inversionistas que
            entienden tu etapa. Menos currículum, más proyecto.
          </p>
          <div className="cluster">
            <Button to="/registro" size="lg">
              Crear mi perfil
            </Button>
            <Button href={`#${LANDING_SECTION_IDS.howItWorks}`} variant="secondary" size="lg">
              Ver cómo funciona
            </Button>
          </div>
        </div>

        <ProductPreview />
      </div>
    </section>
  );
}