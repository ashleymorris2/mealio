export function formatTime(time: string): string {
  const [hours, minutes] = time.split(":");
  const formattedHours = hours !== "00" ? `${hours}h ` : "";
  return `${formattedHours} ${minutes}m`;
}
