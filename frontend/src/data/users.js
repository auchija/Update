// Usuarios de prueba. Se usan mientras no exista el backend (VITE_USE_MOCKS=true).
// Tienen la misma forma que el usuario que devuelve la API (docs/contrato-api.md),
// más `password`, que el backend real nunca devuelve (solo guarda su hash).
//
// Cuenta para probar el inicio de sesión (con el correo o con el usuario):
//   demo@correo.com  o  @valentina   ·   contraseña: Demo1234

export const TEST_USERS = [
  {
    id: "5f1d2c3a-7b8e-4c21-9a10-000000000001",
    handle: "valentina",
    displayName: "Valentina Ospina",
    email: "demo@correo.com",
    password: "Demo1234",
    avatarUrl: null,
    personas: ["entrepreneur"],
  },
  {
    id: "5f1d2c3a-7b8e-4c21-9a10-000000000002",
    handle: "laura.restrepo",
    displayName: "Laura Restrepo",
    email: "laura@correo.com",
    password: "Mentora1234",
    avatarUrl: null,
    personas: ["mentor", "investor"],
  },
];
