import { useContext, useEffect, useState } from "react";
import { ReceiptDocumentApi, useWarehouseManagmentApi } from "../../../api";
import {
  ReceiptDocumentViewDto,
  ReceiptFilterData,
} from "../../../api/api/data-contracts";
import { Button } from "primereact/button";
import { useNavigate } from "react-router-dom";
import LoadingSpinner from "../../core/LoadingSpinner";
import { Calendar } from "primereact/calendar";
import { nameof } from "ts-simple-nameof";
import { MultiSelect } from "primereact/multiselect";
import React from "react";
import { format } from "date-fns";
import { ToastContext } from "../../../contexts/ToastContext";
import { Helmet } from "react-helmet";
interface ReceiptFilter {
  PeriodS: Date | null;
  PeriodPo: Date | null;
  Numbers: string[] | [];
  Units: number[] | [];
  Resources: number[] | [];
}

const ReceiptDocument = () => {
  const navigate = useNavigate();
  const abortController = new AbortController();
  const toastContext = useContext(ToastContext);

  const receiptDocumentApi = useWarehouseManagmentApi(ReceiptDocumentApi);

  const [loading, setLoading] = useState(false);
  const [dataFilter, setDataFilter] = useState<ReceiptFilterData>();
  const [filter, setFilter] = useState<ReceiptFilter>({
    PeriodS: null,
    PeriodPo: null,
    Numbers: [],
    Resources: [],
    Units: [],
  });
  const [data, setData] = useState<ReceiptDocumentViewDto[]>();

  useEffect(() => {
    const loadData = async () => {
      setLoading(true);

      await receiptDocumentApi
        .getDataFilter({ signal: abortController.signal })
        .then((result) => {
          setDataFilter(result);
        })
        .catch((ex) => {
          toastContext?.showToast({
            severity: "error",
            summary: "Ошибка",
            detail: ex.error.title,
            life: 5000,
          });
        });

      await getData();
    };

    loadData();
  }, []);

  const getData = async () => {
    setLoading(true);
    await receiptDocumentApi
      .getDocuments(
        {
          ...filter,
          PeriodS:
            filter?.PeriodS === null
              ? undefined
              : format(filter?.PeriodS, "yyyy-MM-dd"),
          PeriodPo:
            filter?.PeriodPo === null
              ? undefined
              : format(filter?.PeriodPo, "yyyy-MM-dd"),
        },
        { signal: abortController.signal }
      )
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

  const handleAdd = () => {
    navigate("/receiptDocument/add");
  };

  const handleEdit = (id: number) => {
    navigate(`/receiptDocument/edit/${id}`);
  };

  if (loading) return <LoadingSpinner />;
  return (
    <>
      <Helmet>
        <title>Поступления</title>
      </Helmet>
      <p className="content-header">Поступления</p>
      <div className="filter">
        <div className="filter-field">
          <label>Период</label>
          <div
            style={{
              display: "flex",
              justifyContent: "flex-start",
              gap: "0px",
            }}
          >
            <Calendar
              id={nameof<ReceiptFilter>((_) => _.PeriodS)}
              className="calendar-no-button-bg p-calendar-filter"
              value={filter?.PeriodS}
              onChange={(e) => {
                setFilter((prev) => ({
                  ...prev,
                  PeriodS: e.value as Date | null,
                }));
              }}
              showIcon
            />
            <Calendar
              id={nameof<ReceiptFilter>((_) => _.PeriodPo)}
              className="calendar-no-button-bg p-calendar-filter"
              value={filter?.PeriodPo}
              onChange={(e) => {
                setFilter((prev) => ({
                  ...prev,
                  PeriodPo: e.value as Date | null,
                }));
              }}
              showIcon
            />
          </div>
        </div>
        <div className="filter-field">
          <label htmlFor={nameof<ReceiptFilter>((_) => _.Numbers)}>
            Номер поступления
          </label>
          <MultiSelect
            id={nameof<ReceiptFilter>((_) => _.Numbers)}
            value={filter.Numbers}
            onChange={(e) => {
              setFilter((prev) => ({
                ...prev,
                Numbers: e.value as string[],
              }));
            }}
            options={dataFilter?.numbers || []}
            optionLabel="name"
            optionValue="code"
            filter
            maxSelectedLabels={3}
            className="w-full md:w-15rem"
          />
        </div>
        <div className="filter-field">
          <label htmlFor={nameof<ReceiptFilter>((_) => _.Resources)}>
            Ресурс
          </label>
          <MultiSelect
            id={nameof<ReceiptFilter>((_) => _.Resources)}
            value={filter.Resources}
            onChange={(e) => {
              setFilter((prev) => ({
                ...prev,
                Resources: e.value as number[],
              }));
            }}
            options={dataFilter?.resources || []}
            optionLabel="name"
            optionValue="code"
            filter
            maxSelectedLabels={3}
            className="w-full md:w-15rem"
          />
        </div>
        <div className="filter-field">
          <label htmlFor={nameof<ReceiptFilter>((_) => _.Units)}>
            Единица измерения
          </label>
          <MultiSelect
            id={nameof<ReceiptFilter>((_) => _.Units)}
            value={filter.Units}
            onChange={(e) => {
              setFilter((prev) => ({
                ...prev,
                Units: e.value as number[],
              }));
            }}
            options={dataFilter?.units || []}
            optionLabel="name"
            optionValue="code"
            filter
            maxSelectedLabels={3}
            className="w-full md:w-15rem"
          />
        </div>
      </div>
      <div className="button-container">
        <Button className="button-apply" label="Применить" onClick={getData} />
        <Button className="button-add" label="Добавить" onClick={handleAdd} />
      </div>

      <table className="table">
        <thead className="table-header">
          <tr>
            <th>Номер</th>
            <th>Дата</th>
            <th>Ресурс</th>
            <th>Единица измерения</th>
            <th>Количество</th>
          </tr>
        </thead>
        <tbody>
          {data?.map((item) => (
            <React.Fragment key={item.id}>
              <tr
                key={item.id}
                style={{ cursor: "pointer" }}
                onClick={() => handleEdit(item.id!)}
              >
                <td
                  rowSpan={
                    item.documentResources?.length
                      ? item.documentResources.length + 1
                      : 1
                  }
                >
                  {item.number}
                </td>
                <td
                  rowSpan={
                    item.documentResources?.length
                      ? item.documentResources.length + 1
                      : 1
                  }
                >
                  {new Date(item.date!).toLocaleDateString()}
                </td>
                {!item.documentResources?.length && (
                  <>
                    <td>-</td>
                    <td>-</td>
                    <td>0</td>
                  </>
                )}
              </tr>

              {item.documentResources?.map((resource, index) => (
                <tr
                  key={`${item.id}-${index}`}
                  style={{ cursor: "pointer" }}
                  onClick={() => handleEdit(item.id!)}
                >
                  {index === 0 && !item.documentResources?.length && (
                    <>
                      <td>-</td>
                      <td>-</td>
                      <td>0</td>
                    </>
                  )}
                  <td>{resource.resourceName || "-"}</td>
                  <td>{resource.unitName || "-"}</td>
                  <td>{resource.count ?? 0}</td>
                </tr>
              ))}
            </React.Fragment>
          ))}
        </tbody>
      </table>
    </>
  );
};

export default ReceiptDocument;
