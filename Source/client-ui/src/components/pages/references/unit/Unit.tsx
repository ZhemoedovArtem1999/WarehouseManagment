import { useContext, useEffect, useState } from "react";
import { UnitApi, useWarehouseManagmentApi } from "../../../../api";
import { UnitDto } from "../../../../api/api/data-contracts";
import { Button } from "primereact/button";
import { useNavigate } from "react-router-dom";
import LoadingSpinner from "../../../core/LoadingSpinner";
import { Helmet } from "react-helmet";
import { ToastContext } from "../../../../contexts/ToastContext";

const Unit = () => {
  const navigate = useNavigate();
  const abortController = new AbortController();
  const toastContext = useContext(ToastContext);

  const [loading, setLoading] = useState(false);
  const [isArchive, setIsArchive] = useState(false);
  const [data, setData] = useState<UnitDto>();
  const unitApi = useWarehouseManagmentApi(UnitApi);

  useEffect(() => {
    const loadData = async () => {
      setLoading(true);

      await unitApi
        .get(isArchive, { signal: abortController.signal })
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
        })
        .finally(() => {
          setLoading(false);
        });
    };

    loadData();
  }, [isArchive]);

  const handleAdd = () => {
    navigate("/unit/add");
  };

  const handleEdit = (id: number) => {
    navigate(`/unit/edit/${id}`);
  };

  if (loading) return <LoadingSpinner />;
  return (
    <>
      <Helmet>
        <title>Единицы измерения</title>
      </Helmet>
      <p className="content-header">Единицы измерения</p>
      <div className="button-container">
        {isArchive ? (
          <Button
            className="button-worker"
            label="К рабочим"
            onClick={() => setIsArchive(false)}
          />
        ) : (
          <>
            <Button
              className="button-add"
              label="Добавить"
              onClick={handleAdd}
            />
            <Button
              className="button-archive"
              label="К архиву"
              onClick={() => setIsArchive(true)}
            />
          </>
        )}
      </div>

      <table className="table">
        <thead className="table-header">
          <tr>
            <th>Наименование</th>
          </tr>
        </thead>
        <tbody>
          {data?.items?.map((item) => (
            <tr
              style={{ cursor: "pointer" }}
              key={item.id}
              onClick={() => handleEdit(item.id!)}
            >
              <td>{item.name}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </>
  );
};

export default Unit;
