export function formatDate(dateStr: string) {
  const date = new Date(dateStr);
  return date.toLocaleDateString('uk-UA', {
    year: 'numeric',
    month: 'long',
    day: 'numeric',
  });
} 