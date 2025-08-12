import { useContext, useEffect, useState } from "react";
import { ReceiptDocumentApi, useWarehouseManagmentApi } from "../../../api";
import { Button } from "primereact/button";
import { useNavigate, useParams } from "react-router-dom";
import { InputText } from "primereact/inputtext";
import { nameof } from "ts-simple-nameof";
import LoadingSpinner from "../../core/LoadingSpinner";
import {
  ReceiptDocumentDto,
  ReceiptResourceDto,
  ReceiptResourceReferences,
} from "../../../api/api/data-contracts";
import { EditPageProps } from "../../core/EditPageProps";
import { Calendar } from "primereact/calendar";
import { format } from "date-fns";
import { Dropdown } from "primereact/dropdown";
import { InputNumber } from "primereact/inputnumber";
import { ToastContext } from "../../../contexts/ToastContext";
import { Helmet } from "react-helmet";

const EditReceiptDocument: React.FC<EditPageProps> = ({ mode }) => {
  const navigate = useNavigate();
  const abortController = new AbortController();
  const toastContext = useContext(ToastContext);

  const receiptDocumentApi = useWarehouseManagmentApi(ReceiptDocumentApi);

  const [loading, setLoading] = useState(false);
  const { id } = useParams<{ id: string | undefined }>();
  const [data, setData] = useState<ReceiptDocumentDto>({
    date: format(new Date(), "yyyy-MM-dd"),
  });
  const [reference, setReference] = useState<ReceiptResourceReferences>();

  useEffect(() => {
    const fetchData = async () => {
      setLoading(true);
      await loadReference();
      if (mode === "edit") {
        await loadData();
      }
      setLoading(false);
    };
    fetchData();
  }, [id]);

  const loadReference = async () => {
    await receiptDocumentApi
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
    await receiptDocumentApi
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

  const handleSave = async () => {
    if (!data?.number || data?.number?.length === 0 || !data.date) {
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
        await receiptDocumentApi.createDocument(
          {
            ...data,
            date: format(new Date(data.date), "yyyy-MM-dd"),
          },
          { signal: abortController.signal }
        );

        toastContext?.showToast({
          severity: "success",
          summary: "Успешно выполнено",
          detail: "Документ добавлен",
          life: 5000,
        });

        navigate("/receiptDocuments");
      } else {
        await receiptDocumentApi.updateDocument(
          {
            ...data,
            date: format(new Date(data.date), "yyyy-MM-dd"),
          },
          { signal: abortController.signal }
        );

        toastContext?.showToast({
          severity: "success",
          summary: "Успешно выполнено",
          detail: "Документ обновлен",
          life: 5000,
        });

        navigate("/receiptDocuments");
      }
    } catch (error: any) {
      toastContext?.showToast({
        severity: "error",
        summary: "Ошибка",
        detail: error.error?.title,
        life: 5000,
      });
      if (error.error.detail) handleRecoverResource(Number(error.error.detail));
    } finally {
      setLoading(false);
    }
  };

  const handleDelete = async () => {
    setLoading(true);
    await receiptDocumentApi
      .deleteDocument(Number(id!), { signal: abortController.signal })
      .then(() => {
        toastContext?.showToast({
          severity: "success",
          summary: "Успешно выполнено",
          detail: "Документ удален",
          life: 5000,
        });

        navigate("/receiptDocuments");
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

  const handleAddRow = () => {
    try {
      if (!reference?.resources?.length) {
        throw new Error("Нет рабочих ресурсов для добавления");
      }
      if (!reference?.units?.length) {
        throw new Error("Нет рабочих единиц измерения для добавления");
      }

      setData((prev) => ({
        ...prev,
        resources: [
          ...(prev.resources || []),
          {
            resourceId: Number(reference.resources![0].code),
            unitId: Number(reference.units![0].code),
            count: 0,
            isChange: true,
          },
        ],
      }));
    } catch (error) {
      const errorMessage =
        error instanceof Error ? error.message : "Произошла неизвестная ошибка";

      toastContext?.showToast({
        severity: "error",
        summary: "Ошибка",
        detail: errorMessage,
        life: 5000,
      });
    }
  };

  const handleDeleteRow = (index: number) => {
    setData((prev) => ({
      ...prev,
      resources: prev.resources?.filter((_, i) => i !== index),
    }));
  };

  const handleDeleteRowResource = (index: number) => {
    setData((prev) => ({
      ...prev,
      resources: prev.resources?.map((item, i) =>
        i === index ? { ...item, isDelete: true, isChange: false } : item
      ),
    }));
  };

  const handleRecoverResource = (id: number) => {
    setData((prev) => ({
      ...prev,
      resources: prev.resources?.map((item) =>
        item.id === id ? { ...item, isDelete: false } : item
      ),
    }));
  };

  const handleResourceChange = (index: number, field: string, value: any) => {
    setData((prev) => ({
      ...prev,
      resources: prev.resources?.map((item, i) =>
        i === index ? { ...item, [field]: value, isChange: true } : item
      ),
    }));
  };

  if (loading) return <LoadingSpinner />;

  return (
    <>
      <Helmet>
        <title>Поступление</title>
      </Helmet>
      <p className="content-header">Поступление</p>
      <div className="button-container">
        <Button className="button-add" label="Сохранить" onClick={handleSave} />
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
            htmlFor={nameof<ReceiptDocumentDto>((_) => _.number)}
          >
            Номер
          </label>
          <InputText
            id={nameof<ReceiptDocumentDto>((_) => _.number)}
            value={data?.number}
            onChange={(e) => {
              setData((prev) => ({
                ...prev,
                [nameof<ReceiptDocumentDto>((_) => _.number)]: e.target.value,
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
            htmlFor={nameof<ReceiptDocumentDto>((_) => _.date)}
          >
            Дата
          </label>
          <Calendar
            id={nameof<ReceiptDocumentDto>((_) => _.date)}
            className={`calendar-no-button-bg ${!data?.date ? "p-invalid" : ""}`}
            value={new Date(data?.date!)}
            onChange={(e) => {
              setData((prev) => ({
                ...prev,
                [nameof<ReceiptDocumentDto>((_) => _.date)]: e.value,
              }));
            }}
            showIcon
          />
        </div>
      </div>

      <div className="resources-data">
        <table className="table">
          <thead className="table-header">
            <tr>
              <th>
                <Button
                  className="button-add"
                  label="+"
                  onClick={handleAddRow}
                />
              </th>
              <th>Ресурс</th>
              <th>Единица измерения</th>
              <th>Количество</th>
            </tr>
          </thead>
          <tbody>
            {data.resources?.map((item, index) =>
              item.isDelete ? (
                <></>
              ) : (
                <tr key={index}>
                  <td>
                    <Button
                      className="button-delete"
                      label="X"
                      onClick={() => {
                        item.id
                          ? handleDeleteRowResource(index)
                          : handleDeleteRow(index);
                      }}
                    />
                  </td>
                  <td>
                    <Dropdown
                      value={item.resourceId?.toString() || null}
                      options={reference?.resources || []}
                      optionLabel="name"
                      optionValue="code"
                      style={{ width: "100%" }}
                      onChange={(e) =>
                        handleResourceChange(
                          index,
                          nameof<ReceiptResourceDto>((_) => _.resourceId),
                          e.value
                        )
                      }
                    />
                  </td>
                  <td>
                    <Dropdown
                      value={item.unitId?.toString() || null}
                      options={reference?.units || []}
                      optionLabel="name"
                      optionValue="code"
                      style={{ width: "100%" }}
                      onChange={(e) =>
                        handleResourceChange(
                          index,
                          nameof<ReceiptResourceDto>((_) => _.unitId),
                          e.value
                        )
                      }
                    />
                  </td>
                  <td>
                    <InputNumber
                      value={item.count}
                      onChange={(e) =>
                        handleResourceChange(
                          index,
                          nameof<ReceiptResourceDto>((_) => _.count),
                          e.value
                        )
                      }
                      mode="decimal"
                      maxFractionDigits={5}
                      className={item.count! <= 0 ? "p-invalid" : ""}
                    />
                  </td>
                </tr>
              )
            )}
          </tbody>
        </table>
      </div>
    </>
  );
};

export default EditReceiptDocument;
