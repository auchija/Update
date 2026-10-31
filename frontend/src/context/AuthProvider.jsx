import { useCallback, useEffect, useMemo, useState } from "react";
import * as authService from "../services/authService.js";
import { AuthContext } from "./authContext.js";

// Se guarda la promesa para que la sesión se restaure una sola vez,
// aunque React (en modo desarrollo) ejecute el efecto dos veces.
let restoringSession = null;

function restoreSessionOnce() {
  restoringSession ??= authService.restoreSession();
  return restoringSession;
}

export default function AuthProvider({ children }) {
  const [user, setUser] = useState(null);
  // "checking" mientras se verifica la sesión; luego "authenticated" o "anonymous".
  const [status, setStatus] = useState("checking");

  useEffect(() => {
    let cancelled = false;

    restoreSessionOnce().then((restoredUser) => {
      if (cancelled) return;
      setUser(restoredUser);
      setStatus(restoredUser ? "authenticated" : "anonymous");
    });

    return () => {
      cancelled = true;
    };
  }, []);

  const login = useCallback(async (identifier, password) => {
    const loggedUser = await authService.login(identifier, password);
    setUser(loggedUser);
    setStatus("authenticated");
    return loggedUser;
  }, []);

  const register = useCallback(async (form) => {
    const newUser = await authService.register(form);
    setUser(newUser);
    setStatus("authenticated");
    return newUser;
  }, []);

  const logout = useCallback(async () => {
    await authService.logout();
    setUser(null);
    setStatus("anonymous");
  }, []);

  const value = useMemo(
    () => ({ user, status, login, register, logout }),
    [user, status, login, register, logout],
  );

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
}
