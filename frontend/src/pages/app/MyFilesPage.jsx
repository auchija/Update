import { useState } from "react";
import { Download, FileText, Trash2 } from "lucide-react";
import Alert from "../../components/Alert.jsx";
import Badge from "../../components/Badge.jsx";
import Button from "../../components/Button.jsx";
import Card from "../../components/Card.jsx";
import Spinner from "../../components/Spinner.jsx";
import { usePaginatedList } from "../../hooks/usePaginatedList.js";
import {
  ARCHIVOS_PAGE_SIZE,
  eliminarArchivo,
  getArchivoUrl,
  getMisArchivos,
} from "../../services/archivosService.js";
import { formatFileSize, isImage } from "../../utils/fileValidation.js";
import styles from "./MyFilesPage.module.css";

const PURPOSE_LABELS = {
  foto_perfil: "Foto de perfil",
  portada: "Emprendimiento",
  publicacion: "Publicación",
  producto: "Producto",
};

const TYPE_LABELS = {
  "image/jpeg": "JPG",
  "image/png": "PNG",
  "image/webp": "WEBP",
  "application/pdf": "PDF",
};

const dateFormat = new Intl.DateTimeFormat("es-CO", { dateStyle: "medium", timeStyle: "short" });

export default function MyFilesPage() {
  const list = usePaginatedList(getMisArchivos, ARCHIVOS_PAGE_SIZE);
  const [notice, setNotice] = useState(null);
  const [deletingId, setDeletingId] = useState(null);

  async function handleDelete(file) {
    const name = file.nombreOriginal ?? "este archivo";
    if (!window.confirm(`¿Quieres eliminar "${name}"? Se borrará del almacenamiento.`)) return;

    setDeletingId(file.id);
    setNotice(null);
    try {
      await eliminarArchivo(file.id);
      list.removeItem(file.id);
      setNotice({ variant: "success", text: `Eliminamos "${name}".` });
    } catch (error) {
      setNotice({ variant: "danger", text: error.message });
    } finally {
      setDeletingId(null);
    }
  }

  async function handleLoadMore() {
    setNotice(null);
    try {
      await list.loadMore();
    } catch (error) {
      setNotice({ variant: "danger", text: error.message });
    }
  }

  return (
    <>
      <title>Mis archivos · UpDate</title>
      <div className={styles.page}>
        <header>
          <h1>Mis archivos</h1>
          <p className="text-muted">
            Las imágenes y documentos que has subido en publicaciones, perfil y emprendimientos.
          </p>
        </header>

        {notice && <Alert variant={notice.variant}>{notice.text}</Alert>}

        {list.loadError && (
          <Card className="stack">
            <Alert variant="danger">{list.loadError}</Alert>
            <div>
              <Button variant="secondary" onClick={list.retry}>
                Reintentar
              </Button>
            </div>
          </Card>
        )}

        {!list.loadError && !list.items && <Spinner label="Cargando archivos" />}

        {list.items?.length === 0 && (
          <Card className={styles.empty}>
            <h2>Aún no has subido archivos</h2>
            <p className="text-muted">
              Cuando adjuntes una imagen o un PDF en una publicación, aparecerá aquí.
            </p>
          </Card>
        )}

        {list.items?.length > 0 && (
          <ul className={styles.list} aria-label="Mis archivos">
            {list.items.map((file) => (
              <li key={file.id}>
                <FileRow
                  file={file}
                  onDelete={() => handleDelete(file)}
                  isDeleting={deletingId === file.id}
                />
              </li>
            ))}
          </ul>
        )}

        {list.hasMore && (
          <div className={styles.more}>
            <Button variant="secondary" onClick={handleLoadMore} isLoading={list.isLoadingMore}>
              {list.isLoadingMore ? "Cargando" : "Ver más"}
            </Button>
          </div>
        )}
      </div>
    </>
  );
}

function FileRow({ file, onDelete, isDeleting }) {
  const url = getArchivoUrl(file.url);
  const name = file.nombreOriginal ?? "Archivo sin nombre";

  return (
    <Card compact className={styles.item}>
      {isImage(file.tipoMime) ? (
        <img src={url} alt="" loading="lazy" className={styles.thumb} />
      ) : (
        <div className={styles.fileIcon}>
          <FileText aria-hidden="true" className="icon" />
        </div>
      )}

      <div className={styles.info}>
        <p className={styles.name} title={name}>
          {name}
        </p>
        <p className="text-muted text-sm">
          {TYPE_LABELS[file.tipoMime] ?? file.tipoMime} · {formatFileSize(file.tamanoBytes)} ·{" "}
          <time dateTime={file.creadoEn}>{dateFormat.format(new Date(file.creadoEn))}</time>
        </p>
        {PURPOSE_LABELS[file.proposito] && <Badge>{PURPOSE_LABELS[file.proposito]}</Badge>}
      </div>

      <div className={styles.actions}>
        <a
          href={url}
          target="_blank"
          rel="noopener noreferrer"
          className={styles.action}
          aria-label={`Ver o descargar ${name}`}
        >
          <Download aria-hidden="true" className="icon icon--sm" />
          <span>Ver</span>
        </a>
        <Button
          variant="danger"
          size="sm"
          onClick={onDelete}
          isLoading={isDeleting}
          aria-label={`Eliminar ${name}`}
        >
          {!isDeleting && <Trash2 aria-hidden="true" className="icon icon--sm" />}
          <span>{isDeleting ? "Eliminando" : "Eliminar"}</span>
        </Button>
      </div>
    </Card>
  );
}
