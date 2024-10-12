export function formatTime(time: string): string {
  if (!time || typeof time !== "string") {
    throw new Error(
      "Invalid input: Time must be a string in the format 'HH:MM'."
    );
  }

  const [hours, minutes] = time.split(":");
  const formattedHours = hours !== "00" ? `${hours}h ` : "";
  return `${formattedHours} ${minutes}m`;
}
