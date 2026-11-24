const MAX_DIMENSION = 1600;
const MIN_BYTES_TO_COMPRESS = 400 * 1024;
const WEBP_QUALITY = 0.85;
const COMPRESSIBLE_TYPES = ["image/jpeg", "image/png", "image/webp"];

export async function optimizeImage(file) {
  if (!COMPRESSIBLE_TYPES.includes(file.type)) return file;

  let bitmap;
  try {
    bitmap = await createImageBitmap(file);
    const scale = Math.min(1, MAX_DIMENSION / Math.max(bitmap.width, bitmap.height));
    if (scale === 1 && file.size < MIN_BYTES_TO_COMPRESS) return file;

    const canvas = document.createElement("canvas");
    canvas.width = Math.round(bitmap.width * scale);
    canvas.height = Math.round(bitmap.height * scale);
    canvas.getContext("2d").drawImage(bitmap, 0, 0, canvas.width, canvas.height);

    const blob = await new Promise((resolve) => canvas.toBlob(resolve, "image/webp", WEBP_QUALITY));
    if (!blob || blob.type !== "image/webp" || blob.size >= file.size) return file;

    const baseName = file.name.replace(/\.[^.]+$/, "");
    return new File([blob], `${baseName}.webp`, {
      type: "image/webp",
      lastModified: file.lastModified,
    });
  } catch {
    return file;
  } finally {
    bitmap?.close();
  }
}
