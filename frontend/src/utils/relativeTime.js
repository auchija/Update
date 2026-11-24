const relative = new Intl.RelativeTimeFormat("es", { numeric: "auto" });
const fullDate = new Intl.DateTimeFormat("es-CO", { dateStyle: "long", timeStyle: "short" });
const shortDate = new Intl.DateTimeFormat("es-CO", { day: "numeric", month: "short" });

const UNITS = [
  ["year", 60 * 60 * 24 * 365],
  ["month", 60 * 60 * 24 * 30],
  ["week", 60 * 60 * 24 * 7],
  ["day", 60 * 60 * 24],
  ["hour", 60 * 60],
  ["minute", 60],
];

export function timeAgo(date, now = Date.now()) {
  const value = new Date(date);
  const seconds = Math.round((value.getTime() - now) / 1000);
  if (Math.abs(seconds) < 60) return "justo ahora";
  if (Math.abs(seconds) >= 60 * 60 * 24 * 30) return shortDate.format(value);

  const [unit, size] = UNITS.find(([, secondsInUnit]) => Math.abs(seconds) >= secondsInUnit);
  return relative.format(Math.round(seconds / size), unit);
}

export function formatFullDate(date) {
  return fullDate.format(new Date(date));
}

export function formatCount(value) {
  return new Intl.NumberFormat("es-CO", { notation: "compact", maximumFractionDigits: 1 }).format(
    value ?? 0,
  );
}
