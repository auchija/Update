import { useEffect, useState } from "react";

/**
 * Carga una lista paginada de la API con estados de carga, error y "Ver más".
 * El backend devuelve arreglos sin total: si llega una página completa, puede haber más.
 * `fetchPage` debe ser estable (una función del servicio, no una flecha nueva en cada render).
 */
export function usePaginatedList(fetchPage, pageSize) {
  const [items, setItems] = useState(null);
  const [loadError, setLoadError] = useState("");
  const [attempt, setAttempt] = useState(0);
  const [page, setPage] = useState(1);
  const [hasMore, setHasMore] = useState(false);
  const [isLoadingMore, setIsLoadingMore] = useState(false);

  useEffect(() => {
    let cancelled = false;

    fetchPage(1)
      .then((data) => {
        if (cancelled) return;
        setItems(data);
        setPage(1);
        setHasMore(data.length === pageSize);
      })
      .catch((error) => {
        if (!cancelled) setLoadError(error.message);
      });

    return () => {
      cancelled = true;
    };
  }, [fetchPage, pageSize, attempt]);

  function retry() {
    setLoadError("");
    setAttempt((current) => current + 1);
  }

  /** Lanza el error para que la página decida cómo mostrarlo. */
  async function loadMore() {
    setIsLoadingMore(true);
    try {
      const data = await fetchPage(page + 1);
      // Si se creó algo mientras tanto, la página siguiente puede repetir elementos.
      setItems((current) => [
        ...current,
        ...data.filter((item) => !current.some((existing) => existing.id === item.id)),
      ]);
      setPage((current) => current + 1);
      setHasMore(data.length === pageSize);
    } finally {
      setIsLoadingMore(false);
    }
  }

  function addItem(item) {
    setItems((current) => [item, ...(current ?? [])]);
  }

  function replaceItem(item) {
    setItems((current) => current.map((existing) => (existing.id === item.id ? item : existing)));
  }

  function removeItem(id) {
    setItems((current) => current.filter((existing) => existing.id !== id));
  }

  return {
    items,
    loadError,
    retry,
    hasMore,
    isLoadingMore,
    loadMore,
    addItem,
    replaceItem,
    removeItem,
  };
}
