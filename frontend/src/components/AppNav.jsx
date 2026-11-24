import { useState } from "react";
import { NavLink, useNavigate } from "react-router-dom";
import { Compass, FolderOpen, House, LogOut, Newspaper, SquarePlus, UserPlus } from "lucide-react";
import { ROUTES, userProfilePath } from "../constants/routes.js";
import { useAuth } from "../hooks/useAuth.js";
import { classNames } from "../utils/classNames.js";
import Avatar from "./Avatar.jsx";
import Logo from "./Logo.jsx";
import ThemeToggle from "./ThemeToggle.jsx";
import styles from "./AppNav.module.css";

export default function AppNav({ onCreate, requestsCount = 0 }) {
  const { user, logout } = useAuth();
  const navigate = useNavigate();
  const [isLoggingOut, setIsLoggingOut] = useState(false);

  async function handleLogout() {
    setIsLoggingOut(true);
    try {
      await logout();
    } finally {
      navigate(ROUTES.home, { replace: true });
    }
  }

  const profileItem = user && (
    <NavItem
      to={userProfilePath(user.handle)}
      label="Perfil"
      icon={<Avatar name={user.displayName} src={user.avatarUrl} size="xs" decorative />}
    />
  );

  return (
    <>
      <header className={styles.topBar}>
        <Logo />
        <div className={styles.topActions}>
          <ThemeToggle />
          <button
            type="button"
            onClick={handleLogout}
            disabled={isLoggingOut}
            className={styles.iconButton}
            title="Cerrar sesión"
          >
            <LogOut aria-hidden="true" className="icon" />
            <span className="sr-only">Cerrar sesión</span>
          </button>
        </div>
      </header>

      <nav aria-label="Principal" className={styles.sidebar}>
        <div className={styles.brand}>
          <Logo />
        </div>

        <ul className={styles.list}>
          <NavItem to={ROUTES.feed} label="Inicio" Icon={House} />
          <NavItem to={ROUTES.explore} label="Explorar" Icon={Compass} />
          <li>
            <button type="button" onClick={onCreate} className={styles.link}>
              <SquarePlus aria-hidden="true" className={styles.icon} />
              <span className={styles.label}>Crear</span>
            </button>
          </li>
          <NavItem to={ROUTES.requests} label="Solicitudes" Icon={UserPlus} badge={requestsCount} />
          <NavItem to={ROUTES.myPosts} label="Mis publicaciones" Icon={Newspaper} />
          <NavItem to={ROUTES.myFiles} label="Mis archivos" Icon={FolderOpen} />
          {profileItem}
        </ul>

        <div className={styles.footer}>
          <ThemeToggle />
          <button
            type="button"
            onClick={handleLogout}
            disabled={isLoggingOut}
            className={styles.link}
            title="Cerrar sesión"
          >
            <LogOut aria-hidden="true" className={styles.icon} />
            <span className={styles.label}>{isLoggingOut ? "Cerrando…" : "Cerrar sesión"}</span>
          </button>
        </div>
      </nav>

      <nav aria-label="Secciones" className={styles.bottomBar}>
        <ul className={styles.bottomList}>
          <NavItem to={ROUTES.feed} label="Inicio" Icon={House} compact />
          <NavItem to={ROUTES.explore} label="Explorar" Icon={Compass} compact />
          <li>
            <button type="button" onClick={onCreate} className={styles.bottomLink} title="Crear">
              <SquarePlus aria-hidden="true" className={styles.icon} />
              <span className="sr-only">Crear</span>
            </button>
          </li>
          <NavItem
            to={ROUTES.requests}
            label="Solicitudes"
            Icon={UserPlus}
            badge={requestsCount}
            compact
          />
          {user && (
            <NavItem
              to={userProfilePath(user.handle)}
              label="Perfil"
              compact
              icon={<Avatar name={user.displayName} src={user.avatarUrl} size="xs" decorative />}
            />
          )}
        </ul>
      </nav>
    </>
  );
}

function NavItem({ to, label, Icon, icon, badge = 0, compact = false }) {
  return (
    <li>
      <NavLink
        to={to}
        title={label}
        className={({ isActive }) =>
          classNames(compact ? styles.bottomLink : styles.link, isActive && styles.active)
        }
      >
        <span className={styles.iconWrap}>
          {icon ?? <Icon aria-hidden="true" className={styles.icon} />}
          {badge > 0 && (
            <b className={styles.badge} aria-hidden="true">
              {badge > 9 ? "9+" : badge}
            </b>
          )}
        </span>
        <span className={compact ? "sr-only" : styles.label}>
          {label}
          {badge > 0 && <span className="sr-only"> ({badge} pendientes)</span>}
        </span>
      </NavLink>
    </li>
  );
}
