import Button from "../../components/Button.jsx";
import styles from "./CallToActionSection.module.css";

export default function CallToActionSection() {
  return (
    <section aria-labelledby="llamado-titulo" className={styles.cta}>
      <div className="container">
        <div className={styles.band}>
          <div className="stack stack--xs">
            <h2 id="llamado-titulo" className={styles.title}>
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
  );
}
