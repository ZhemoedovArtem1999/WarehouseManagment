import { useContext, useEffect, useState } from "react";
import { BalanceApi, useWarehouseManagmentApi } from "../../../api";
import { BalanceDto, BalanceFilterData } from "../../../api/api/data-contracts";
import { Button } from "primereact/button";
import LoadingSpinner from "../../core/LoadingSpinner";
import { nameof } from "ts-simple-nameof";
import { MultiSelect } from "primereact/multiselect";
import { ToastContext } from "../../../contexts/ToastContext";
import { Helmet } from "react-helmet";

interface BalanceFilter {
  Units: number[] | [];
  Resources: number[] | [];
}

const Balance = () => {
  const abortController = new AbortController();
  const toastContext = useContext(ToastContext);

  const balanceApi = useWarehouseManagmentApi(BalanceApi);

  const [loading, setLoading] = useState(false);
  const [dataFilter, setDataFilter] = useState<BalanceFilterData>();
  const [filter, setFilter] = useState<BalanceFilter>({
    Resources: [],
    Units: [],
  });
  const [data, setData] = useState<BalanceDto[]>();

  useEffect(() => {
    const loadData = async () => {
      setLoading(true);

      await balanceApi
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
    await balanceApi
      .getBalance(filter, { signal: abortController.signal })
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

  if (loading) return <LoadingSpinner />;
  return (
    <>
      <Helmet>
        <title>Баланс</title>
      </Helmet>
      <p className="content-header">Баланс</p>
      <div className="filter">
        <div className="filter-field">
          <label htmlFor={nameof<BalanceFilter>((_) => _.Resources)}>
            Ресурс
          </label>
          <MultiSelect
            id={nameof<BalanceFilter>((_) => _.Resources)}
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
          <label htmlFor={nameof<BalanceFilter>((_) => _.Units)}>
            Единица измерения
          </label>
          <MultiSelect
            id={nameof<BalanceFilter>((_) => _.Units)}
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
        <Button
          style={{ height: "46px", alignSelf: "end" }}
          className="button-apply"
          label="Применить"
          onClick={getData}
        />
      </div>

      <table className="table">
        <thead className="table-header">
          <tr>
            <th>Ресурс</th>
            <th>Единица измерения</th>
            <th>Количество</th>
          </tr>
        </thead>
        <tbody>
          {data?.map((item) => (
            <tr key={item.resourceName}>
              <td>{item.resourceName}</td>
              <td>{item.unitName}</td>
              <td>{item.count}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </>
  );
};

export default Balance;
