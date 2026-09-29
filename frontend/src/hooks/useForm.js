import { useState } from "react";

export function useForm({ initialValues, validate, onSubmit }) {
  const [values, setValues] = useState(initialValues);
  const [errors, setErrors] = useState({});
  const [formError, setFormError] = useState("");
  const [isSubmitting, setIsSubmitting] = useState(false);

  function setFieldValue(name, value) {
    setValues((current) => ({ ...current, [name]: value }));
    setErrors((current) => ({ ...current, [name]: undefined }));
  }

  function handleChange(event) {
    const { name, type, value, checked } = event.target;
    setFieldValue(name, type === "checkbox" ? checked : value);
  }

  async function handleSubmit(event) {
    event.preventDefault();
    setFormError("");

    const validationErrors = validate(values);
    setErrors(validationErrors);

    const firstInvalidField = Object.keys(validationErrors)[0];
    if (firstInvalidField) {
      focusField(event.target, firstInvalidField);
      return;
    }

    setIsSubmitting(true);
    try {
      await onSubmit(values);
    } catch (error) {
      setErrors(error.fieldErrors ?? {});
      setFormError(error.message);
    } finally {
      setIsSubmitting(false);
    }
  }

  function reset() {
    setValues(initialValues);
    setErrors({});
    setFormError("");
  }

  return {
    values,
    errors,
    formError,
    isSubmitting,
    setFieldValue,
    handleChange,
    handleSubmit,
    reset,
  };
}

function focusField(form, fieldName) {
  const field = form.elements[fieldName];
  const firstControl = field instanceof RadioNodeList ? field[0] : field;
  firstControl.focus();
}
