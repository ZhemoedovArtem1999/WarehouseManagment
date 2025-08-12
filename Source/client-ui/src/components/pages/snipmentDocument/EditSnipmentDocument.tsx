import { useContext, useEffect, useState } from "react";
import { SnipmentDocumentApi, useWarehouseManagmentApi } from "../../../api";
import { Button } from "primereact/button";
import { useNavigate, useParams } from "react-router-dom";
import { InputText } from "primereact/inputtext";
import { nameof } from "ts-simple-nameof";
import LoadingSpinner from "../../core/LoadingSpinner";
import {
  SnipmentDocumentDto,
  SnipmentResourceDto,
  SnipmentResourceReferences,
} from "../../../api/api/data-contracts";
import { EditPageProps } from "../../core/EditPageProps";
import { Calendar } from "primereact/calendar";
import { format } from "date-fns";
import { Dropdown } from "primereact/dropdown";
import { InputNumber } from "primereact/inputnumber";
import { ToastContext } from "../../../contexts/ToastContext";
import { Helmet } from "react-helmet";
import SignSnipmentDocument from "./SignSnipmentDocument";

const EditSnipmentDocument: React.FC<EditPageProps> = ({ mode }) => {
  const navigate = useNavigate();
  const abortController = new AbortController();
  const toastContext = useContext(ToastContext);

  const snipmentDocumentApi = useWarehouseManagmentApi(SnipmentDocumentApi);

  const [loading, setLoading] = useState(false);
  const { id } = useParams<{ id: string | undefined }>();
  const [data, setData] = useState<SnipmentDocumentDto>({
    date: format(new Date(), "yyyy-MM-dd"),
  });
  const [reference, setReference] = useState<SnipmentResourceReferences>();

  useEffect(() => {
    const fetchData = async () => {
      setLoading(true);
      await loadReference();
      if (mode === "edit") {
        await loadData();
      } else {
        await loadResourceBalance();
      }
      setLoading(false);
    };
    fetchData();
  }, [id]);

  const loadResourceBalance = async () => {
    await snipmentDocumentApi
      .getResourcesBalance({ signal: abortController.signal })
      .then((result) => {
        setData((prev) => ({ ...prev, resources: result }));
      })
      .catch((ex) => {
        toastContext?.showToast({
          severity: "error",
          summary: "Ошибка",
          detail: ex.error.title,
          life: 5000,
        });
        navigate("/snipmentDocuments");
      });
  };

  const loadReference = async () => {
    await snipmentDocumentApi
      .getReferences(
        { documentId: id ? Number(id) : undefined },
        { signal: abortController.signal }
      )
      .then((result) => {
        setReference(result);
      })
      .catch((ex) => {
        toastContext?.showToast({
          severity: "error",
          summary: "Ошибка",
          detail: ex.error.title,
          life: 5000,
        });
      });
  };

  const loadData = async () => {
    await snipmentDocumentApi
      .getDocument(Number(id))
      .then((result) => {
        setData(result);
      })
      .catch((ex) => {
        toastContext?.showToast({
          severity: "error",
          summary: "Ошибка",
          detail: ex.error.title,
          life: 5000,
        });
      });
  };

  const handleSave = async (isSign: boolean = false) => {
    if (
      !data?.number ||
      data?.number?.length === 0 ||
      !data.date ||
      !data.clientId
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
    try {
      if (mode === "create") {
        await snipmentDocumentApi.createDocument(
          {
            ...data,
            date: format(new Date(data.date), "yyyy-MM-dd"),
            isSign: isSign,
          },
          { signal: abortController.signal }
        );

        toastContext?.showToast({
          severity: "success",
          summary: "Успешно выполнено",
          detail: "Документ добавлен",
          life: 5000,
        });

        navigate("/snipmentDocuments");
      } else {
        await snipmentDocumentApi.updateDocument(
          {
            ...data,
            date: format(new Date(data.date), "yyyy-MM-dd"),
            isSign: isSign,
          },
          { signal: abortController.signal }
        );

        toastContext?.showToast({
          severity: "success",
          summary: "Успешно выполнено",
          detail: "Документ обновлен",
          life: 5000,
        });

        navigate("/snipmentDocuments");
      }
    } catch (error: any) {
      toastContext?.showToast({
        severity: "error",
        summary: "Ошибка",
        detail: error.error?.title,
        life: 5000,
      });
    } finally {
      setLoading(false);
    }
  };

  const handleDelete = async () => {
    setLoading(true);
    await snipmentDocumentApi
      .deleteDocument(Number(id!), { signal: abortController.signal })
      .then(() => {
        toastContext?.showToast({
          severity: "success",
          summary: "Успешно выполнено",
          detail: "Документ удален",
          life: 5000,
        });

        navigate("/snipmentDocuments");
      })
      .catch((error) => {
        toastContext?.showToast({
          severity: "error",
          summary: "Ошибка",
          detail: error.error?.title,
          life: 5000,
        });
      });
    setLoading(false);
  };

  const handleResourceChange = (index: number, value: any | null) => {
    setData((prev) => ({
      ...prev,
      resources: prev.resources?.map((item, i) =>
        i === index
          ? {
              ...item,
              [nameof<SnipmentResourceDto>((_) => _.count)]:
                item.countBalance !== undefined
                  ? Math.min(Number(value), item.countBalance)
                  : value,
              isChange: true,
            }
          : item
      ),
    }));
  };

  if (loading) return <LoadingSpinner />;
  return (
    <>
      <Helmet>
        <title>Отгрузка</title>
      </Helmet>
      {data.isSign ? (
        <SignSnipmentDocument
          data={data}
          clientName={
            reference?.clients?.find((x) => x.code === data.clientId)?.name!
          }
        />
      ) : (
        <>
          <p className="content-header">Отгрузка</p>
          <div className="button-container">
            <Button
              className="button-add"
              label="Сохранить"
              onClick={() => handleSave(false)}
            />
            <Button
              className="button-add"
              label="Сохранить и подписать"
              onClick={() => handleSave(true)}
            />
            {mode === "create" ? (
              <></>
            ) : (
              <Button
                className="button-delete"
                label="Удалить"
                onClick={handleDelete}
              />
            )}
          </div>
          <div className="input-container">
            <div className="input-field">
              <label
                className="input-label"
                htmlFor={nameof<SnipmentDocumentDto>((_) => _.number)}
              >
                Номер
              </label>
              <InputText
                id={nameof<SnipmentDocumentDto>((_) => _.number)}
                value={data?.number}
                onChange={(e) => {
                  setData((prev) => ({
                    ...prev,
                    [nameof<SnipmentDocumentDto>((_) => _.number)]:
                      e.target.value,
                  }));
                }}
                className={
                  !data?.number || data?.number.length === 0 ? "p-invalid" : ""
                }
                style={{ width: "250px" }}
              />
            </div>
            <div className="input-field">
              <label
                className="input-label"
                htmlFor={nameof<SnipmentDocumentDto>((_) => _.date)}
              >
                Дата
              </label>
              <Calendar
                id={nameof<SnipmentDocumentDto>((_) => _.date)}
                className={`calendar-no-button-bg ${!data?.date ? "p-invalid" : ""}`}
                value={new Date(data?.date!)}
                onChange={(e) => {
                  setData((prev) => ({
                    ...prev,
                    [nameof<SnipmentDocumentDto>((_) => _.date)]: e.value,
                  }));
                }}
                showIcon
              />
            </div>
            <div className="input-field">
              <label
                className="input-label"
                htmlFor={nameof<SnipmentDocumentDto>((_) => _.clientId)}
              >
                Клиент
              </label>
              <Dropdown
                id={nameof<SnipmentDocumentDto>((_) => _.clientId)}
                value={data.clientId?.toString() || null}
                options={reference?.clients || []}
                optionLabel="name"
                optionValue="code"
                onChange={(e) => {
                  setData((prev) => ({
                    ...prev,
                    [nameof<SnipmentDocumentDto>((_) => _.clientId)]: e.value,
                  }));
                }}
                style={{ width: "250px" }}
                className={!data.clientId ? "p-invalid" : ""}
              />
            </div>
          </div>

          <div className="resources-data">
            <table className="table">
              <thead className="table-header">
                <tr>
                  <th>Ресурс</th>
                  <th>Единица измерения</th>
                  <th>Количество</th>
                  <th>Доступно</th>
                </tr>
              </thead>
              <tbody>
                {data.resources?.map((item, index) => (
                  <tr key={index}>
                    <td>{item.resourceName}</td>
                    <td>{item.unitName}</td>
                    <td>
                      <InputNumber
                        value={item.count}
                        onChange={(e) => handleResourceChange(index, e.value)}
                        mode="decimal"
                        maxFractionDigits={5}
                        max={
                          item.countBalance !== 0
                            ? item.countBalance
                            : undefined
                        }
                      />
                    </td>
                    <td>{item.countBalance}</td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        </>
      )}
    </>
  );
};

export default EditSnipmentDocument;
