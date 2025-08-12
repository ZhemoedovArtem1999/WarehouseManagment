import { createContext, ReactNode, useContext, useMemo } from "react";
import * as ApiTypes from "./api/data-contracts";
import { ApiConfig, HttpClient } from "./api/http-client";

const WarehouseManagmentApiConfigContext = createContext<ApiConfig | undefined>(undefined);

const useWarehouseManagmentApiConfig = () => useContext(WarehouseManagmentApiConfigContext);

function useWarehouseManagmentApi<ApiType extends HttpClient>(
  apiType: new (apiConfig: ApiConfig) => ApiType,
  apiConfig: ApiConfig = {},
): ApiType {
  const baseConfig = useWarehouseManagmentApiConfig();

  return useMemo(() => {
    return new apiType({
      ...baseConfig,
      ...apiConfig,
    } as ApiConfig);
  }, [baseConfig, apiConfig]);
}

const WarehouseManagmentApi = ({ apiAddress, children }: { apiAddress: string; children: ReactNode }) => {
  const apiConfig = useMemo(() => {
    return {
      baseUrl: apiAddress,
      baseApiParams: {
        secure: true,
      },
    };
  }, [apiAddress]);

  return (
    <WarehouseManagmentApiConfigContext.Provider value={apiConfig}>
      {children}
    </WarehouseManagmentApiConfigContext.Provider>
  );
};

export { useWarehouseManagmentApi, WarehouseManagmentApi, ApiTypes as WarehouseManagmentApiTypes };

export { BalanceApi } from "./api/BalanceApi";
export { ClientApi } from "./api/ClientApi";
export { ReceiptDocumentApi } from "./api/ReceiptDocumentApi";
export { ResourceApi } from "./api/ResourceApi";
export { SnipmentDocumentApi } from "./api/SnipmentDocumentApi";
export { UnitApi } from "./api/UnitApi";
