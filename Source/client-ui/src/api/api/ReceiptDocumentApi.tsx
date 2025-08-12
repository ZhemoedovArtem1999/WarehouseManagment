import {
  ReceiptDocumentDto,
  ReceiptDocumentViewDto,
  ReceiptFilterData,
  ReceiptResourceReferences,
  ValidationProblemDetails,
} from "./data-contracts";
import { ContentType, HttpClient, RequestParams } from "./http-client";

export class ReceiptDocumentApi<SecurityDataType = unknown> extends HttpClient<SecurityDataType> {
  /**
   * No description
   *
   * @tags ReceiptDocument
   * @name CreateDocument
   * @request POST:/api/ReceiptDocument/create
   */
  createDocument = (data: ReceiptDocumentDto, params: RequestParams = {}) =>
    this.request<string, ValidationProblemDetails>({
      path: `/api/ReceiptDocument/create`,
      method: "POST",
      body: data,
      type: ContentType.Json,
      format: "json",
      ...params,
    });
  /**
   * No description
   *
   * @tags ReceiptDocument
   * @name DeleteDocument
   * @request POST:/api/ReceiptDocument/delete
   */
  deleteDocument = (data: number, params: RequestParams = {}) =>
    this.request<string, ValidationProblemDetails>({
      path: `/api/ReceiptDocument/delete`,
      method: "POST",
      body: data,
      type: ContentType.Json,
      format: "json",
      ...params,
    });
  /**
   * No description
   *
   * @tags ReceiptDocument
   * @name GetDataFilter
   * @request GET:/api/ReceiptDocument/getFilter
   */
  getDataFilter = (params: RequestParams = {}) =>
    this.request<ReceiptFilterData, ValidationProblemDetails>({
      path: `/api/ReceiptDocument/getFilter`,
      method: "GET",
      format: "json",
      ...params,
    });
  /**
   * No description
   *
   * @tags ReceiptDocument
   * @name GetDocument
   * @request GET:/api/ReceiptDocument/document/{id}
   */
  getDocument = (id: number, params: RequestParams = {}) =>
    this.request<ReceiptDocumentDto, ValidationProblemDetails>({
      path: `/api/ReceiptDocument/document/${id}`,
      method: "GET",
      format: "json",
      ...params,
    });
  /**
   * No description
   *
   * @tags ReceiptDocument
   * @name GetDocuments
   * @request GET:/api/ReceiptDocument/filter
   */
  getDocuments = (
    query?: {
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
    this.request<ReceiptDocumentViewDto[], ValidationProblemDetails>({
      path: `/api/ReceiptDocument/filter`,
      method: "GET",
      query: query,
      format: "json",
      ...params,
    });
  /**
   * No description
   *
   * @tags ReceiptDocument
   * @name GetReferences
   * @request GET:/api/ReceiptDocument/getReferences
   */
  getReferences = (
    query?: {
      /** @format int32 */
      documentId?: number;
    },
    params: RequestParams = {},
  ) =>
    this.request<ReceiptResourceReferences, ValidationProblemDetails>({
      path: `/api/ReceiptDocument/getReferences`,
      method: "GET",
      query: query,
      format: "json",
      ...params,
    });
  /**
   * No description
   *
   * @tags ReceiptDocument
   * @name UpdateDocument
   * @request POST:/api/ReceiptDocument/update
   */
  updateDocument = (data: ReceiptDocumentDto, params: RequestParams = {}) =>
    this.request<string, ValidationProblemDetails>({
      path: `/api/ReceiptDocument/update`,
      method: "POST",
      body: data,
      type: ContentType.Json,
      format: "json",
      ...params,
    });
}
