import { useState } from "react";
import { NavLink, useNavigate } from "react-router-dom";
import { House, LogOut, UserRound } from "lucide-react";
import { ROUTES } from "../constants/routes.js";
import { useAuth } from "../hooks/useAuth.js";
import { classNames } from "../utils/classNames.js";
import Avatar from "./Avatar.jsx";
import Button from "./Button.jsx";
import Logo from "./Logo.jsx";
import ThemeToggle from "./ThemeToggle.jsx";
import styles from "./AppHeader.module.css";

const APP_LINKS = [
  { to: ROUTES.feed, label: "Inicio", Icon: House },
  { to: ROUTES.profile, label: "Mi perfil", Icon: UserRound },
];

export default function AppHeader() {
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

  return (
    <header className={styles.header}>
      <div className={classNames("container", styles.inner)}>
        <Logo />

        <nav aria-label="Principal" className={styles.nav}>
          <ul className={styles.navList}>
            {APP_LINKS.map(({ to, label, Icon }) => (
              <li key={to}>
                <NavLink
                  to={to}
                  title={label}
                  className={({ isActive }) =>
                    classNames(styles.navLink, isActive && styles.navLinkActive)
                  }
                >
                  <Icon aria-hidden="true" className="icon" />
                  <span className={styles.navLabel}>{label}</span>
                </NavLink>
              </li>
            ))}
          </ul>
        </nav>

        <div className={styles.actions}>
          <ThemeToggle />
          {user && (
            <div className={styles.user}>
              <Avatar name={user.displayName} src={user.avatarUrl} size="sm" decorative />
              <span className={styles.userName}>{user.displayName}</span>
            </div>
          )}
          <Button
            variant="ghost"
            size="sm"
            onClick={handleLogout}
            isLoading={isLoggingOut}
            title="Cerrar sesión"
          >
            {!isLoggingOut && <LogOut aria-hidden="true" className="icon icon--sm" />}
            <span className={styles.logoutLabel}>Cerrar sesión</span>
          </Button>
        </div>
      </div>
    </header>
  );
}
