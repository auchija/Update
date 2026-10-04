import { useState } from "react";
import { Eye, EyeOff } from "lucide-react";
import Input from "./Input.jsx";
import styles from "./PasswordInput.module.css";

export default function PasswordInput({ validator, ...props }) {
  const [isVisible, setIsVisible] = useState(false);
  const Icon = isVisible ? EyeOff : Eye;

  const visibilityToggle = (
    <button
      type="button"
      onClick={() => setIsVisible((visible) => !visible)}
      aria-pressed={isVisible}
      aria-label="Mostrar contraseña"
      className={styles.toggle}
    >
      <Icon aria-hidden="true" className="icon" />
    </button>
  );

  return <Input {...props} type={isVisible ? "text" : "password"} endSlot={visibilityToggle} validator={validator} />;
}
