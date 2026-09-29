import { useState } from "react";
import { Eye, EyeOff } from "lucide-react";
import Input from "./Input.jsx";
import styles from "./PasswordInput.module.css";

/** Campo de contraseña con un botón (ojo) para mostrarla u ocultarla. */
export default function PasswordInput(props) {
  const [isVisible, setIsVisible] = useState(false);
  const Icon = isVisible ? EyeOff : Eye;

  return (
    <Input
      {...props}
      type={isVisible ? "text" : "password"}
      endSlot={
        <button
          type="button"
          onClick={() => setIsVisible(!isVisible)}
          aria-pressed={isVisible}
          aria-label="Mostrar contraseña"
          className={styles.toggle}
        >
          <Icon aria-hidden="true" className="icon" />
        </button>
      }
    />
  );
}
