import { useEffect, useState } from "react";

export function usePaginatedList(fetchPage, pageSize, refreshKey = 0) {
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
  }, [fetchPage, pageSize, attempt, refreshKey]);

  function retry() {
    setLoadError("");
    setAttempt((current) => current + 1);
  }

  async function loadMore() {
    setIsLoadingMore(true);
    try {
      const data = await fetchPage(page + 1);
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
