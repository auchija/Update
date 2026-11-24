import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import Avatar from "../../components/Avatar.jsx";
import { userProfilePath } from "../../constants/routes.js";
import { getRelacionados } from "../../services/comunidadService.js";
import FollowButton from "./FollowButton.jsx";
import styles from "./RelatedProfiles.module.css";

export default function RelatedProfiles({ handle, onError }) {
  const [people, setPeople] = useState([]);

  useEffect(() => {
    let cancelled = false;
    getRelacionados(handle)
      .then((data) => {
        if (!cancelled) setPeople(data);
      })
      .catch(() => {});
    return () => {
      cancelled = true;
    };
  }, [handle]);

  if (people.length === 0) return null;

  function update(personHandle, changes) {
    setPeople((current) =>
      current.map((p) => (p.nombreUsuario === personHandle ? { ...p, ...changes } : p)),
    );
  }

  return (
    <section aria-labelledby="relacionados-titulo" className={styles.section}>
      <h2 id="relacionados-titulo" className={styles.title}>
        Perfiles relacionados
      </h2>
      <ul className={styles.list}>
        {people.map((person) => (
          <li key={person.nombreUsuario} className={styles.card}>
            <Link to={userProfilePath(person.nombreUsuario)} className={styles.person}>
              <Avatar name={person.nombreVisible} src={person.fotoUrl} size="lg" ring decorative />
              <span className={styles.handle}>{person.nombreUsuario}</span>
              <span className={styles.name}>{person.titular || person.nombreVisible}</span>
            </Link>
            <FollowButton
              person={person}
              onChange={(changes) => update(person.nombreUsuario, changes)}
              onError={onError}
            />
          </li>
        ))}
      </ul>
    </section>
  );
}
