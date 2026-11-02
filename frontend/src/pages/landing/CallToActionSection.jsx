import { Link } from "react-router-dom";
import styles from "./CallToActionSection.module.css";

const IMAGES = [
  "https://images.unsplash.com/photo-1552664730-d307ca884978?w=400&h=600&fit=crop",
  "https://images.unsplash.com/photo-1600880292203-757bb62b4baf?w=400&h=600&fit=crop",
  "https://images.unsplash.com/photo-1556761175-5973dc0f32e7?w=400&h=600&fit=crop",
  "https://images.unsplash.com/photo-1573497019940-1c28c88b4f3e?w=400&h=600&fit=crop",
  "https://images.unsplash.com/photo-1542744173-8e7e53415bb0?w=400&h=600&fit=crop",
  "https://images.unsplash.com/photo-1557804506-669a67965ba0?w=400&h=600&fit=crop",
  "https://images.unsplash.com/photo-1551434678-e076c223a692?w=400&h=600&fit=crop",
  "https://images.unsplash.com/photo-1600880292089-90a7e086ee0c?w=400&h=600&fit=crop",
];

const CAROUSEL_IMAGES = [...IMAGES, ...IMAGES];

export default function CallToActionSection() {
  return (
    <section aria-labelledby="llamado-titulo" className={styles.cta}>
      <svg
        className={styles.wave}
        viewBox="0 0 1440 120"
        preserveAspectRatio="none"
        aria-hidden="true"
      >
        <path d="M0 70 C 240 130 480 10 720 45 C 960 80 1200 125 1440 50 V120 H0 Z" />
      </svg>
      <div className={styles.body}>
        <div className={styles.content}>
          <h2 id="llamado-titulo" className={styles.title}>
            Tu próximo socio puede estar a <em className={styles.emphasis}>una conexión</em>
          </h2>
          <p className={styles.lead}>
            Crea tu perfil y empieza a conectar con quienes pueden impulsar tu proyecto.
          </p>
          <Link to="/registro" className={styles.button}>
            Crear mi perfil
          </Link>
        </div>

        <div className={styles.carousel} aria-hidden="true">
          <div className={styles.track}>
            {CAROUSEL_IMAGES.map((url, index) => (
              <div key={`${url}-${index}`} className={styles.imageCard}>
                <img src={url} alt="" loading="lazy" className={styles.image} />
              </div>
            ))}
          </div>
        </div>
      </div>
    </section>
  );
}
