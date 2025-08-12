import { useContext, useEffect, useState } from "react";
import { ClientApi, useWarehouseManagmentApi } from "../../../../api";
import { Button } from "primereact/button";
import { useNavigate, useParams } from "react-router-dom";
import { InputText } from "primereact/inputtext";
import { nameof } from "ts-simple-nameof";
import LoadingSpinner from "../../../core/LoadingSpinner";
import { EditPageProps } from "../../../core/EditPageProps";
import { ClientItem } from "../../../../api/api/data-contracts";
import { Helmet } from "react-helmet";
import { ToastContext } from "../../../../contexts/ToastContext";

const EditClient: React.FC<EditPageProps> = ({ mode }) => {
  const navigate = useNavigate();
  const abortController = new AbortController();
  const toastContext = useContext(ToastContext);

  const [loading, setLoading] = useState(false);
  const { id } = useParams<{ id: string }>();
  const [data, setData] = useState<ClientItem>({});
  const clienApi = useWarehouseManagmentApi(ClientApi);

  useEffect(() => {
    if (mode === "edit") loadData();
  }, [id]);

  const loadData = async () => {
    setLoading(true);
    await clienApi
      .getById(Number(id), { signal: abortController.signal })
      .then((result) => {
        setData({
          name: result.name!,
          address: result.address!,
          isArchive: result.isArchive!,
        });
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
    if (
      !data?.name ||
      data?.name?.length === 0 ||
      !data?.address ||
      data?.address?.length === 0
    ) {
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
      await clienApi
        .create(
          { name: data?.name!, address: data?.address! },
          { signal: abortController.signal }
        )
        .then((result) => {
          toastContext?.showToast({
            severity: "success",
            summary: "Успешно выполнено",
            detail: "Клиент добавлен",
            life: 5000,
          });
          navigate("/clients");
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
      await clienApi
        .update(
          { id: Number(id), name: data.name, address: data.address },
          { signal: abortController.signal }
        )
        .then((result) => {
          toastContext?.showToast({
            severity: "success",
            summary: "Успешно выполнено",
            detail: "Клиент обновлен",
            life: 5000,
          });
          navigate("/clients");
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
    await clienApi
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
    await clienApi
      .delete(Number(id!), { signal: abortController.signal })
      .then((result) => {
        toastContext?.showToast({
          severity: "success",
          summary: "Успешно выполнено",
          detail: "Клиент удален",
          life: 5000,
        });
        navigate("/clients");
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
        <title>Клиент</title>
      </Helmet>
      <p className="content-header">Клиент</p>
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
      <div className="input-container">
        <div className="input-field">
          <label
            className="input-label"
            htmlFor={nameof<ClientItem>((_) => _.name)}
          >
            Наименование
          </label>
          <InputText
            id={nameof<ClientItem>((_) => _.name)}
            value={data?.name}
            onChange={(e) => {
              setData((prev) => ({
                ...prev,
                [nameof<ClientItem>((_) => _.name)]: e.target.value,
              }));
            }}
            className={
              !data?.name || data?.name.length === 0 ? "p-invalid" : ""
            }
          />
        </div>
        <div className="input-field">
          <label
            className="input-label"
            htmlFor={nameof<ClientItem>((_) => _.address)}
          >
            Адрес
          </label>
          <InputText
            id={nameof<ClientItem>((_) => _.address)}
            value={data?.address}
            onChange={(e) => {
              setData((prev) => ({
                ...prev,
                [nameof<ClientItem>((_) => _.address)]: e.target.value,
              }));
            }}
            className={
              !data?.address || data?.address.length === 0 ? "p-invalid" : ""
            }
          />
        </div>
      </div>
    </>
  );
};

export default EditClient;
