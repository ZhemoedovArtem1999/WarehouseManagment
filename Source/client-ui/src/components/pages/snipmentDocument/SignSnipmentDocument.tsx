import { useContext, useState } from "react";
import { SnipmentDocumentApi, useWarehouseManagmentApi } from "../../../api";
import { Button } from "primereact/button";
import { useNavigate } from "react-router-dom";
import { nameof } from "ts-simple-nameof";
import LoadingSpinner from "../../core/LoadingSpinner";
import {
  SnipmentDocumentDto,
} from "../../../api/api/data-contracts";
import { ToastContext } from "../../../contexts/ToastContext";
import { Helmet } from "react-helmet";

interface SignSnipmentDocumentProps {
  data: SnipmentDocumentDto;
  clientName: string;
}

const SignSnipmentDocument: React.FC<SignSnipmentDocumentProps> = ({
  data,
  clientName,
}) => {
  const navigate = useNavigate();
  const abortController = new AbortController();
  const toastContext = useContext(ToastContext);

  const snipmentDocumentApi = useWarehouseManagmentApi(SnipmentDocumentApi);

  const [loading, setLoading] = useState(false);

  const handleRevoke = async () => {
    setLoading(true);
    await snipmentDocumentApi
      .revokeDocument(data.id!, { signal: abortController.signal })
      .then(() => {
        toastContext?.showToast({
          severity: "success",
          summary: "Успешно выполнено",
          detail: "Документ отозван",
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

  if (loading) return <LoadingSpinner />;
  return (
    <>
      <div className="button-container">
        <Button
          className="button-delete"
          label="Отозвать"
          onClick={handleRevoke}
        />
      </div>
      <div className="input-container">
        <div className="input-field">
          <label
            className="input-label"
            htmlFor={nameof<SnipmentDocumentDto>((_) => _.number)}
          >
            Номер
          </label>
          <label>{data.number}</label>
        </div>
        <div className="input-field">
          <label
            className="input-label"
            htmlFor={nameof<SnipmentDocumentDto>((_) => _.date)}
          >
            Дата
          </label>
          <label>{data.date}</label>
        </div>
        <div className="input-field">
          <label
            className="input-label"
            htmlFor={nameof<SnipmentDocumentDto>((_) => _.clientId)}
          >
            Клиент
          </label>
          <label>{clientName}</label>
        </div>
      </div>

      <div className="resources-data">
          <table className="table">
            <thead className="table-header">
              <tr>
                <th>Ресурс</th>
                <th>Единица измерения</th>
                <th>Количество</th>
              </tr>
            </thead>
            <tbody>
              {data.resources
                ?.filter((item) => item.count! > 0)
                .map((item) => (
                  <tr>
                    <td>{item.resourceName}</td>
                    <td>{item.unitName}</td>
                    <td>{item.count}</td>
                  </tr>
                ))}
            </tbody>
          </table>
      </div>
    </>
  );
};

export default SignSnipmentDocument;
