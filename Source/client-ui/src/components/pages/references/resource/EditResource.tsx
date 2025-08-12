import { useContext, useEffect, useState } from "react";
import { ResourceApi, useWarehouseManagmentApi } from "../../../../api";
import { Button } from "primereact/button";
import { useNavigate, useParams } from "react-router-dom";
import { InputText } from "primereact/inputtext";
import { nameof } from "ts-simple-nameof";
import LoadingSpinner from "../../../core/LoadingSpinner";
import { ResourceItem } from "../../../../api/api/data-contracts";
import { EditPageProps } from "../../../core/EditPageProps";
import { Helmet } from "react-helmet";
import { ToastContext } from "../../../../contexts/ToastContext";

const EditResource: React.FC<EditPageProps> = ({ mode }) => {
  const navigate = useNavigate();
  const abortController = new AbortController();
  const toastContext = useContext(ToastContext);

  const [loading, setLoading] = useState(false);
  const { id } = useParams<{ id: string }>();
  const [data, setData] = useState<ResourceItem>({});
  const resourceApi = useWarehouseManagmentApi(ResourceApi);

  useEffect(() => {
    if (mode === "edit") loadData();
  }, [id]);

  const loadData = async () => {
    setLoading(true);
    await resourceApi
      .getById(Number(id), { signal: abortController.signal })
      .then((result) => {
        setData({ name: result.name!, isArchive: result.isArchive! });
      })
      .catch((ex) => {
        toastContext?.showToast({
          severity: "error",
          summary: "Ошибка",
          detail: ex.error.title,
          life: 5000,
        });
      })
      .finally(() => {
        setLoading(false);
      });
  };

  const handleSave = async () => {
    if (!data?.name || data?.name?.length === 0) {
      toastContext?.showToast({
        severity: "warn",
        summary: "Предупреждение",
        detail: "Заполните обязательные поля",
        life: 5000,
      });
      return;
    }

    setLoading(true);

    if (mode === "create") {
      await resourceApi
        .create(data?.name!, { signal: abortController.signal })
        .then((result) => {
          toastContext?.showToast({
            severity: "success",
            summary: "Успешно выполнено",
            detail: "Ресурс добавлен",
            life: 5000,
          });
          navigate("/resources");
        })
        .catch((error) => {
          toastContext?.showToast({
            severity: "error",
            summary: "Ошибка",
            detail: error.error.title,
            life: 5000,
          });
        });
    } else {
      await resourceApi
        .update(
          { id: Number(id), name: data.name },
          { signal: abortController.signal }
        )
        .then((result) => {
          toastContext?.showToast({
            severity: "success",
            summary: "Успешно выполнено",
            detail: "Ресурс обновлен",
            life: 5000,
          });
          navigate("/resources");
        })
        .catch((error) => {
          toastContext?.showToast({
            severity: "error",
            summary: "Ошибка",
            detail: error.error.title,
            life: 5000,
          });
        });
    }
    setLoading(false);
  };

  const handleEditState = async (isArchive: boolean) => {
    setLoading(true);
    await resourceApi
      .updateState(
        { id: Number(id), isArchive: isArchive },
        { signal: abortController.signal }
      )
      .then((result) => {
        loadData();
      })
      .catch((error) => {
        toastContext?.showToast({
          severity: "error",
          summary: "Ошибка",
          detail: error.error.title,
          life: 5000,
        });
      });
    setLoading(false);
  };

  const handleDelete = async () => {
    setLoading(true);
    await resourceApi
      .delete(Number(id!), { signal: abortController.signal })
      .then((result) => {
        toastContext?.showToast({
          severity: "success",
          summary: "Успешно выполнено",
          detail: "Ресурс удален",
          life: 5000,
        });
        navigate("/resources");
      })
      .catch((error) => {
        toastContext?.showToast({
          severity: "error",
          summary: "Ошибка",
          detail: error.error.title,
          life: 5000,
        });
      });
    setLoading(false);
  };

  if (loading) return <LoadingSpinner />;

  return (
    <>
      <Helmet>
        <title>Ресурс</title>
      </Helmet>
      <p className="content-header">Ресурс</p>
      <div className="button-container">
        {mode === "create" ? (
          <Button
            className="button-add"
            label="Сохранить"
            onClick={handleSave}
          />
        ) : (
          <>
            <Button
              className="button-add"
              label="Сохранить"
              onClick={handleSave}
            />
            <Button
              className="button-delete"
              label="Удалить"
              onClick={handleDelete}
            />

            {id && data.isArchive ? (
              <Button
                className="button-worker"
                label="В работу"
                onClick={() => handleEditState(false)}
              />
            ) : (
              <Button
                className="button-archive"
                label="В архив"
                onClick={() => handleEditState(true)}
              />
            )}
          </>
        )}
      </div>

      <div className="input-field">
        <label
          className="input-label"
          htmlFor={nameof<ResourceItem>((_) => _.name)}
        >
          Наименование
        </label>
        <InputText
          id={nameof<ResourceItem>((_) => _.name)}
          value={data?.name}
          onChange={(e) => {
            setData((prev) => ({
              ...prev,
              [nameof<ResourceItem>((_) => _.name)]: e.target.value,
            }));
          }}
          className={!data?.name || data?.name.length === 0 ? "p-invalid" : ""}
        />
      </div>
    </>
  );
};

export default EditResource;
