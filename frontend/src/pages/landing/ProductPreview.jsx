import Avatar from "../../components/Avatar.jsx";
import Badge from "../../components/Badge.jsx";
import Card from "../../components/Card.jsx";
import { classNames } from "../../utils/classNames.js";
import styles from "./ProductPreview.module.css";

export default function ProductPreview() {
  return (
    <div aria-hidden="true" className={styles.preview}>
      <ProjectCard />
      <ConnectionRequestCard />
    </div>
  );
}

function ProjectCard() {
  return (
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
  );
}

function ConnectionRequestCard() {
  return (
    <Card elevated compact className={styles.request}>
      <div className={styles.person}>
        <Avatar name="Laura Restrepo" size="sm" decorative />
        <div className={classNames(styles.personText, "text-sm")}>
          <span>
            <b>Laura Restrepo</b> quiere conectar contigo
          </span>
          <span className="text-muted">Mentora en logística, 12 años</span>
        </div>
      </div>

      <div className="cluster cluster--xs">
        <span className={classNames(styles.fakeButton, styles.fakeButtonPrimary)}>Aceptar</span>
        <span className={styles.fakeButton}>Ver perfil</span>
      </div>
    </Card>
  );
}
