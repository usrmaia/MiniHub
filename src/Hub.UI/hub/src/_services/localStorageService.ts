export const getKey = (key: string): string => key = `@minihub:${key}`;

export const getStorageValue = (key: string, defaultValue?: unknown) => {
  if (typeof window === 'undefined') return;

  const item = localStorage.getItem(getKey(key));

  return item ? JSON.parse(item) : defaultValue || '';
};