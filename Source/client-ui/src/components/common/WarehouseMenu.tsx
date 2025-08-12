import React, { useEffect, useState } from "react";
import { PanelMenu } from "primereact/panelmenu";
import { useLocation, useNavigate } from "react-router-dom";
import { MenuItem } from "primereact/menuitem";
import { Menu } from "primereact/menu";

const WarehouseMenu = () => {
  const navigate = useNavigate();
  const location = useLocation();
  const [expandedKeys, setExpandedKeys] = useState<{ [key: string]: boolean }>({
    Склад: true,
    Справочники: true,
  });
  const [itemSelected, setItemSelected] = useState("/balance");

  useEffect(() => {
    setItemSelected(location.pathname);
  }, [location]);

  const items: MenuItem[] = [
    {
      label: "Склад",
      expanded: expandedKeys["Склад"],
      items: [
        {
          id: "balance",
          label: "Баланс",
          command: () => navigate("/balance"),
          className: itemSelected === "/balance" ? "active-menu-item" : "",
        },
        {
          id: "incoming",
          label: "Поступления",
          command: () => navigate("/receiptDocuments"),
          className: itemSelected === "/receiptDocuments" ? "active-menu-item" : "",
        },
        {
          id: "shipments",
          label: "Отгрузки",
          command: () => navigate("/snipmentDocuments"),
          className: itemSelected === "/snipmentDocuments" ? "active-menu-item" : "",
        },
      ],
    },
    {
      label: "Справочники",
      expanded: expandedKeys["Справочники"],
      items: [
        {
          id: "clients",
          label: "Клиенты",
          command: () => navigate("/clients"),
          className: itemSelected === "/clients" ? "active-menu-item" : "",
        },
        {
          id: "units",
          label: "Единицы измерения",
          command: () => navigate("/units"),
          className: itemSelected === "/units" ? "active-menu-item" : "",
        },
        {
          id: "resources",
          label: "Ресурсы",
          command: () => navigate("/resources"),
          className: itemSelected === "/resources" ? "active-menu-item" : "",
        },
      ],
    },
  ];

  return (
    <>
      <div className="container-menu">
        <div className="logo">
          <div className="logo-text">Управление складом</div>
        </div>
        <div className="warehouse-menu">
          <Menu
            model={items}
            style={{ width: "250px" }}
            className="transparent-menu"
          />
        </div>
      </div>
    </>
  );
};

export default WarehouseMenu;
