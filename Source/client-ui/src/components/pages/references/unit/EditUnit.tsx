import { useContext, useEffect, useState } from "react";
import { UnitApi, useWarehouseManagmentApi } from "../../../../api";
import { Button } from "primereact/button";
import { useNavigate, useParams } from "react-router-dom";
import { InputText } from "primereact/inputtext";
import { nameof } from "ts-simple-nameof";
import LoadingSpinner from "../../../core/LoadingSpinner";
import { UnitItem } from "../../../../api/api/data-contracts";
import { EditPageProps } from "../../../core/EditPageProps";
import { Helmet } from "react-helmet";
import { ToastContext } from "../../../../contexts/ToastContext";

const EditUnit: React.FC<EditPageProps> = ({ mode }) => {
  const navigate = useNavigate();
  const abortController = new AbortController();
  const toastContext = useContext(ToastContext);

  const [loading, setLoading] = useState(false);
  const { id } = useParams<{ id: string }>();
  const [data, setData] = useState<UnitItem>({});
  const unitApi = useWarehouseManagmentApi(UnitApi);

  useEffect(() => {
    if (mode === "edit") loadData();
  }, [id]);

  const loadData = async () => {
    setLoading(true);
    await unitApi
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
      await unitApi
        .create(data?.name!, { signal: abortController.signal })
        .then((result) => {
          toastContext?.showToast({
            severity: "success",
            summary: "Успешно выполнено",
            detail: "Единица измерения добавлена",
            life: 5000,
          });
          navigate("/units");
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
      await unitApi
        .update(
          { id: Number(id), name: data.name },
          { signal: abortController.signal }
        )
        .then((result) => {
          toastContext?.showToast({
            severity: "success",
            summary: "Успешно выполнено",
            detail: "Единица измерения обновлена",
            life: 5000,
          });
          navigate("/units");
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
    await unitApi
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
    await unitApi
      .delete(Number(id!), { signal: abortController.signal })
      .then((result) => {
        toastContext?.showToast({
          severity: "success",
          summary: "Успешно выполнено",
          detail: "Единица измерения удалена",
          life: 5000,
        });
        navigate("/units");
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
        <title>Единица измерения</title>
      </Helmet>
      <p className="content-header">Единица измерения</p>
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
          htmlFor={nameof<UnitItem>((_) => _.name)}
        >
          Наименование
        </label>
        <InputText
          id={nameof<UnitItem>((_) => _.name)}
          value={data?.name}
          onChange={(e) => {
            setData((prev) => ({
              ...prev,
              [nameof<UnitItem>((_) => _.name)]: e.target.value,
            }));
          }}
          className={!data?.name || data?.name.length === 0 ? "p-invalid" : ""}
        />
      </div>
    </>
  );
};

export default EditUnit;
