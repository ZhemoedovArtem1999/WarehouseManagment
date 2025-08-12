import {
  SnipmentDocumentDto,
  SnipmentDocumentViewDto,
  SnipmentFilterData,
  SnipmentResourceDto,
  SnipmentResourceReferences,
  ValidationProblemDetails,
} from "./data-contracts";
import { ContentType, HttpClient, RequestParams } from "./http-client";

export class SnipmentDocumentApi<SecurityDataType = unknown> extends HttpClient<SecurityDataType> {
  /**
   * No description
   *
   * @tags SnipmentDocument
   * @name CreateDocument
   * @request POST:/api/SnipmentDocument/create
   */
  createDocument = (data: SnipmentDocumentDto, params: RequestParams = {}) =>
    this.request<string, ValidationProblemDetails>({
      path: `/api/SnipmentDocument/create`,
      method: "POST",
      body: data,
      type: ContentType.Json,
      format: "json",
      ...params,
    });
  /**
   * No description
   *
   * @tags SnipmentDocument
   * @name DeleteDocument
   * @request POST:/api/SnipmentDocument/delete
   */
  deleteDocument = (data: number, params: RequestParams = {}) =>
    this.request<string, ValidationProblemDetails>({
      path: `/api/SnipmentDocument/delete`,
      method: "POST",
      body: data,
      type: ContentType.Json,
      format: "json",
      ...params,
    });
  /**
   * No description
   *
   * @tags SnipmentDocument
   * @name GetDataFilter
   * @request GET:/api/SnipmentDocument/getFilter
   */
  getDataFilter = (params: RequestParams = {}) =>
    this.request<SnipmentFilterData, ValidationProblemDetails>({
      path: `/api/SnipmentDocument/getFilter`,
      method: "GET",
      format: "json",
      ...params,
    });
  /**
   * No description
   *
   * @tags SnipmentDocument
   * @name GetDocument
   * @request GET:/api/SnipmentDocument/document/{id}
   */
  getDocument = (id: number, params: RequestParams = {}) =>
    this.request<SnipmentDocumentDto, ValidationProblemDetails>({
      path: `/api/SnipmentDocument/document/${id}`,
      method: "GET",
      format: "json",
      ...params,
    });
  /**
   * No description
   *
   * @tags SnipmentDocument
   * @name GetDocuments
   * @request GET:/api/SnipmentDocument/filter
   */
  getDocuments = (
    query?: {
      Clients?: number[];
      Numbers?: string[];
      /** @format date */
      PeriodPo?: string;
      /** @format date */
      PeriodS?: string;
      Resources?: number[];
      Units?: number[];
    },
    params: RequestParams = {},
  ) =>
    this.request<SnipmentDocumentViewDto[], ValidationProblemDetails>({
      path: `/api/SnipmentDocument/filter`,
      method: "GET",
      query: query,
      format: "json",
      ...params,
    });
  /**
   * No description
   *
   * @tags SnipmentDocument
   * @name GetReferences
   * @request GET:/api/SnipmentDocument/getReferences
   */
  getReferences = (
    query?: {
      /** @format int32 */
      documentId?: number;
    },
    params: RequestParams = {},
  ) =>
    this.request<SnipmentResourceReferences, ValidationProblemDetails>({
      path: `/api/SnipmentDocument/getReferences`,
      method: "GET",
      query: query,
      format: "json",
      ...params,
    });
  /**
   * No description
   *
   * @tags SnipmentDocument
   * @name GetResourcesBalance
   * @request GET:/api/SnipmentDocument/getResourcesBalance
   */
  getResourcesBalance = (params: RequestParams = {}) =>
    this.request<SnipmentResourceDto[], ValidationProblemDetails>({
      path: `/api/SnipmentDocument/getResourcesBalance`,
      method: "GET",
      format: "json",
      ...params,
    });
  /**
   * No description
   *
   * @tags SnipmentDocument
   * @name RevokeDocument
   * @request POST:/api/SnipmentDocument/revoke
   */
  revokeDocument = (data: number, params: RequestParams = {}) =>
    this.request<string, ValidationProblemDetails>({
      path: `/api/SnipmentDocument/revoke`,
      method: "POST",
      body: data,
      type: ContentType.Json,
      format: "json",
      ...params,
    });
  /**
   * No description
   *
   * @tags SnipmentDocument
   * @name UpdateDocument
   * @request POST:/api/SnipmentDocument/update
   */
  updateDocument = (data: SnipmentDocumentDto, params: RequestParams = {}) =>
    this.request<string, ValidationProblemDetails>({
      path: `/api/SnipmentDocument/update`,
      method: "POST",
      body: data,
      type: ContentType.Json,
      format: "json",
      ...params,
    });
}
