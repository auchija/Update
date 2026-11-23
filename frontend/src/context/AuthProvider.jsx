import { useCallback, useEffect, useMemo, useState } from "react";
import * as authService from "../services/authService.js";
import { onSessionExpired } from "../services/session.js";
import { AuthContext } from "./authContext.js";

let restoringSession = null;

function restoreSessionOnce() {
  restoringSession ??= authService.restoreSession();
  return restoringSession;
}

export default function AuthProvider({ children }) {
  const [user, setUser] = useState(null);
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

  useEffect(() => {
    onSessionExpired(() => {
      setUser(null);
      setStatus("anonymous");
    });
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
    try {
      await authService.logout();
    } finally {
      // Aunque falle la red, la sesión local se cierra igual.
      setUser(null);
      setStatus("anonymous");
    }
  }, []);

  const updateUser = useCallback((changes) => {
    setUser((current) => ({ ...current, ...changes }));
  }, []);

  const value = useMemo(
    () => ({ user, status, login, register, logout, updateUser }),
    [user, status, login, register, logout, updateUser],
  );

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
}
