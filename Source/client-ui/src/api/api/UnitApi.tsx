import {
  UnitDto,
  UnitItem,
  UnitUpdateRequestDto,
  UnitUpdateStateRequestDto,
  ValidationProblemDetails,
} from "./data-contracts";
import { ContentType, HttpClient, RequestParams } from "./http-client";

export class UnitApi<SecurityDataType = unknown> extends HttpClient<SecurityDataType> {
  /**
   * No description
   *
   * @tags Unit
   * @name Create
   * @request POST:/api/Unit/create
   */
  create = (data: string, params: RequestParams = {}) =>
    this.request<string, ValidationProblemDetails>({
      path: `/api/Unit/create`,
      method: "POST",
      body: data,
      type: ContentType.Json,
      format: "json",
      ...params,
    });
  /**
   * No description
   *
   * @tags Unit
   * @name Delete
   * @request POST:/api/Unit/delete
   */
  delete = (data: number, params: RequestParams = {}) =>
    this.request<string, ValidationProblemDetails>({
      path: `/api/Unit/delete`,
      method: "POST",
      body: data,
      type: ContentType.Json,
      format: "json",
      ...params,
    });
  /**
   * No description
   *
   * @tags Unit
   * @name Get
   * @request GET:/api/Unit/{isArchive}
   */
  get = (isArchive: boolean, params: RequestParams = {}) =>
    this.request<UnitDto, ValidationProblemDetails>({
      path: `/api/Unit/${isArchive}`,
      method: "GET",
      format: "json",
      ...params,
    });
  /**
   * No description
   *
   * @tags Unit
   * @name GetById
   * @request GET:/api/Unit/get/{id}
   */
  getById = (id: number, params: RequestParams = {}) =>
    this.request<UnitItem, ValidationProblemDetails>({
      path: `/api/Unit/get/${id}`,
      method: "GET",
      format: "json",
      ...params,
    });
  /**
   * No description
   *
   * @tags Unit
   * @name Update
   * @request POST:/api/Unit/update
   */
  update = (data: UnitUpdateRequestDto, params: RequestParams = {}) =>
    this.request<string, ValidationProblemDetails>({
      path: `/api/Unit/update`,
      method: "POST",
      body: data,
      type: ContentType.Json,
      format: "json",
      ...params,
    });
  /**
   * No description
   *
   * @tags Unit
   * @name UpdateState
   * @request POST:/api/Unit/updateState
   */
  updateState = (data: UnitUpdateStateRequestDto, params: RequestParams = {}) =>
    this.request<string, ValidationProblemDetails>({
      path: `/api/Unit/updateState`,
      method: "POST",
      body: data,
      type: ContentType.Json,
      format: "json",
      ...params,
    });
}
