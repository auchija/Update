import Card from "../../components/Card.jsx";
import PageLayout from "../../components/PageLayout.jsx";
import { SITE_NAME } from "../../config.js";
import styles from "./Auth.module.css";

export default function AuthLayout({ pageTitle, children }) {
  return (
    <PageLayout title={`${pageTitle} | ${SITE_NAME}`} mainClassName={`container ${styles.main}`}>
      <Card className={styles.card}>{children}</Card>
    </PageLayout>
  );
}
