export interface BalanceDto {
  /** @format double */
  count?: number;
  /** @format int32 */
  resourceId?: number;
  resourceName?: string | null;
  /** @format int32 */
  unitId?: number;
  unitName?: string | null;
}

export interface BalanceFilterData {
  resources?: DropDownItem[] | null;
  units?: DropDownItem[] | null;
}

export interface ClientDto {
  items?: ClientItem[] | null;
}

export interface ClientInsertRequestDto {
  address?: string | null;
  /** @format int32 */
  id?: number | null;
  name?: string | null;
}

export interface ClientItem {
  address?: string | null;
  /** @format int32 */
  id?: number;
  isArchive?: boolean;
  name?: string | null;
}

export interface ClientUpdateRequestDto {
  address?: string | null;
  /** @format int32 */
  id?: number | null;
  name?: string | null;
}

export interface ClientUpdateStateRequestDto {
  /** @format int32 */
  id?: number;
  isArchive?: boolean;
}

export interface DocumentResource {
  /** @format double */
  count?: number;
  resourceName?: string | null;
  unitName?: string | null;
}

export interface DropDownItem {
  code?: string | null;
  name?: string | null;
}

export interface ReceiptDocumentDto {
  /** @format date */
  date?: string;
  /** @format int32 */
  id?: number;
  number?: string | null;
  resources?: ReceiptResourceDto[] | null;
}

export interface ReceiptDocumentViewDto {
  /** @format date */
  date?: string;
  documentResources?: DocumentResource[] | null;
  /** @format int32 */
  id?: number;
  number?: string | null;
}

export interface ReceiptFilterData {
  numbers?: DropDownItem[] | null;
  resources?: DropDownItem[] | null;
  units?: DropDownItem[] | null;
}

export interface ReceiptResourceDto {
  /** @format double */
  count?: number;
  /** @format int32 */
  id?: number | null;
  isChange?: boolean;
  isDelete?: boolean;
  /** @format int32 */
  resourceId?: number;
  /** @format int32 */
  unitId?: number;
}

export interface ReceiptResourceReferences {
  resources?: DropDownItem[] | null;
  units?: DropDownItem[] | null;
}

export interface ResourceDto {
  items?: ResourceItem[] | null;
}

export interface ResourceItem {
  /** @format int32 */
  id?: number;
  isArchive?: boolean;
  name?: string | null;
}

export interface ResourceUpdateRequestDto {
  /** @format int32 */
  id?: number;
  name?: string | null;
}

export interface ResourceUpdateStateRequestDto {
  /** @format int32 */
  id?: number;
  isArchive?: boolean;
}

export interface SnipmentDocumentDto {
  /** @format int32 */
  clientId?: number;
  /** @format date */
  date?: string;
  /** @format int32 */
  id?: number;
  isSign?: boolean;
  number?: string | null;
  resources?: SnipmentResourceDto[] | null;
}

export interface SnipmentDocumentViewDto {
  client?: string | null;
  /** @format date */
  date?: string;
  documentResources?: DocumentResource[] | null;
  /** @format int32 */
  id?: number;
  number?: string | null;
  sign?: boolean;
}

export interface SnipmentFilterData {
  clients?: DropDownItem[] | null;
  numbers?: DropDownItem[] | null;
  resources?: DropDownItem[] | null;
  units?: DropDownItem[] | null;
}

export interface SnipmentResourceDto {
  /** @format double */
  count?: number;
  /** @format double */
  countBalance?: number;
  /** @format int32 */
  id?: number | null;
  isChange?: boolean;
  /** @format int32 */
  resourceId?: number;
  resourceName?: string | null;
  /** @format int32 */
  unitId?: number;
  unitName?: string | null;
}

export interface SnipmentResourceReferences {
  clients?: DropDownItem[] | null;
}

export interface UnitDto {
  items?: UnitItem[] | null;
}

export interface UnitItem {
  /** @format int32 */
  id?: number;
  isArchive?: boolean;
  name?: string | null;
}

export interface UnitUpdateRequestDto {
  /** @format int32 */
  id?: number;
  name?: string | null;
}

export interface UnitUpdateStateRequestDto {
  /** @format int32 */
  id?: number;
  isArchive?: boolean;
}

export interface ValidationProblemDetails {
  detail?: string | null;
  errors?: Record<string, string[]>;
  instance?: string | null;
  /** @format int32 */
  status?: number | null;
  title?: string | null;
  type?: string | null;
  [key: string]: any;
}
