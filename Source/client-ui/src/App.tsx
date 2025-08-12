import React, { useRef } from "react";
import { addLocale, PrimeReactProvider } from "primereact/api";
import {
  BrowserRouter as Router,
  Routes,
  Route,
  Navigate,
} from "react-router-dom";
import "primereact/resources/themes/lara-light-indigo/theme.css";
import "primereact/resources/primereact.min.css";
import "primeicons/primeicons.css";
import WarehouseMenu from "./components/common/WarehouseMenu";
import Balance from "./components/pages/balance/Balance";
import Resource from "./components/pages/references/resource/Resource";
import ruLocale from "primelocale/ru.json";
import { WarehouseManagmentApi } from "./api";
import { baseWarehouseManagmentUrl } from "./utils/constants";
import EditResource from "./components/pages/references/resource/EditResource";
import EditUnit from "./components/pages/references/unit/EditUnit";
import Client from "./components/pages/references/client/Client";
import EditClient from "./components/pages/references/client/EditClient";
import Unit from "./components/pages/references/unit/Unit";
import ReceiptDocument from "./components/pages/receiptDocument/ReceiptDocument";
import EditReceiptDocument from "./components/pages/receiptDocument/EditReceiptDocument";
import { Toast } from "primereact/toast";
import { ToastProvider } from "./contexts/ToastContext";
import SnipmentDocument from "./components/pages/snipmentDocument/SnipmentDocument";
import EditSnipmentDocument from "./components/pages/snipmentDocument/EditSnipmentDocument";

addLocale("ru", ruLocale["ru"]);

function App() {
  const locale = {
    locale: "ru",
  };

  const toast = useRef<Toast>(null);

  return (
    <>
      <Toast ref={toast} />
      <PrimeReactProvider value={locale}>
        <WarehouseManagmentApi apiAddress={baseWarehouseManagmentUrl!}>
          <ToastProvider>
            <Router>
              <div className="app-container">
                <WarehouseMenu />
                <div className="content">
                  <Routes>
                    <Route
                      path="/"
                      element={<Navigate to="/balance" replace />}
                    />
                    <Route path="/balance" element={<Balance />} />
                    <Route path="/resources" element={<Resource />} />
                    <Route
                      path="/resource/add"
                      element={<EditResource mode="create" />}
                    />
                    <Route
                      path="/resource/edit/:id"
                      element={<EditResource mode="edit" />}
                    />
                    <Route path="/units" element={<Unit />} />
                    <Route
                      path="/unit/add"
                      element={<EditUnit mode="create" />}
                    />
                    <Route
                      path="/unit/edit/:id"
                      element={<EditUnit mode="edit" />}
                    />
                    <Route path="/clients" element={<Client />} />
                    <Route
                      path="/client/add"
                      element={<EditClient mode="create" />}
                    />
                    <Route
                      path="/client/edit/:id"
                      element={<EditClient mode="edit" />}
                    />
                    <Route
                      path="/receiptDocuments"
                      element={<ReceiptDocument />}
                    />
                    <Route
                      path="/receiptDocument/add"
                      element={<EditReceiptDocument mode="create" />}
                    />
                    <Route
                      path="/receiptDocument/edit/:id"
                      element={<EditReceiptDocument mode="edit" />}
                    />
                    <Route
                      path="/snipmentDocuments"
                      element={<SnipmentDocument />}
                    />
                    <Route
                      path="/snipmentDocument/add"
                      element={<EditSnipmentDocument mode="create" />}
                    />
                    <Route
                      path="/snipmentDocument/edit/:id"
                      element={<EditSnipmentDocument mode="edit" />}
                    />
                  </Routes>
                </div>
              </div>
            </Router>
          </ToastProvider>
        </WarehouseManagmentApi>
      </PrimeReactProvider>
    </>
  );
}

export default App;
