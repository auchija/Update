import { useId } from "react";

export function useFieldIds({ hint, error }) {
  const id = useId();
  const hintId = `${id}-hint`;
  const errorId = `${id}-error`;
  const describedBy = [error && errorId, hint && hintId].filter(Boolean).join(" ") || undefined;

  return { id, hintId, errorId, describedBy };
}
