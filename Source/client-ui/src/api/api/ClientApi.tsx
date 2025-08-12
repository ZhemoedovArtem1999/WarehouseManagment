import {
  ClientDto,
  ClientInsertRequestDto,
  ClientItem,
  ClientUpdateRequestDto,
  ClientUpdateStateRequestDto,
  ValidationProblemDetails,
} from "./data-contracts";
import { ContentType, HttpClient, RequestParams } from "./http-client";

export class ClientApi<SecurityDataType = unknown> extends HttpClient<SecurityDataType> {
  /**
   * No description
   *
   * @tags Client
   * @name Create
   * @request POST:/api/Client/create
   */
  create = (data: ClientInsertRequestDto, params: RequestParams = {}) =>
    this.request<string, ValidationProblemDetails>({
      path: `/api/Client/create`,
      method: "POST",
      body: data,
      type: ContentType.Json,
      format: "json",
      ...params,
    });
  /**
   * No description
   *
   * @tags Client
   * @name Delete
   * @request POST:/api/Client/delete
   */
  delete = (data: number, params: RequestParams = {}) =>
    this.request<string, ValidationProblemDetails>({
      path: `/api/Client/delete`,
      method: "POST",
      body: data,
      type: ContentType.Json,
      format: "json",
      ...params,
    });
  /**
   * No description
   *
   * @tags Client
   * @name Get
   * @request GET:/api/Client/{isArchive}
   */
  get = (isArchive: boolean, params: RequestParams = {}) =>
    this.request<ClientDto, ValidationProblemDetails>({
      path: `/api/Client/${isArchive}`,
      method: "GET",
      format: "json",
      ...params,
    });
  /**
   * No description
   *
   * @tags Client
   * @name GetById
   * @request GET:/api/Client/get/{id}
   */
  getById = (id: number, params: RequestParams = {}) =>
    this.request<ClientItem, ValidationProblemDetails>({
      path: `/api/Client/get/${id}`,
      method: "GET",
      format: "json",
      ...params,
    });
  /**
   * No description
   *
   * @tags Client
   * @name Update
   * @request POST:/api/Client/update
   */
  update = (data: ClientUpdateRequestDto, params: RequestParams = {}) =>
    this.request<string, ValidationProblemDetails>({
      path: `/api/Client/update`,
      method: "POST",
      body: data,
      type: ContentType.Json,
      format: "json",
      ...params,
    });
  /**
   * No description
   *
   * @tags Client
   * @name UpdateState
   * @request POST:/api/Client/updateState
   */
  updateState = (data: ClientUpdateStateRequestDto, params: RequestParams = {}) =>
    this.request<string, ValidationProblemDetails>({
      path: `/api/Client/updateState`,
      method: "POST",
      body: data,
      type: ContentType.Json,
      format: "json",
      ...params,
    });
}
