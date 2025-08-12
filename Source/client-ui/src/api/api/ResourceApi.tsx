import {
  ResourceDto,
  ResourceItem,
  ResourceUpdateRequestDto,
  ResourceUpdateStateRequestDto,
  ValidationProblemDetails,
} from "./data-contracts";
import { ContentType, HttpClient, RequestParams } from "./http-client";

export class ResourceApi<SecurityDataType = unknown> extends HttpClient<SecurityDataType> {
  /**
   * No description
   *
   * @tags Resource
   * @name Create
   * @request POST:/api/Resource/create
   */
  create = (data: string, params: RequestParams = {}) =>
    this.request<string, ValidationProblemDetails>({
      path: `/api/Resource/create`,
      method: "POST",
      body: data,
      type: ContentType.Json,
      format: "json",
      ...params,
    });
  /**
   * No description
   *
   * @tags Resource
   * @name Delete
   * @request POST:/api/Resource/delete
   */
  delete = (data: number, params: RequestParams = {}) =>
    this.request<string, ValidationProblemDetails>({
      path: `/api/Resource/delete`,
      method: "POST",
      body: data,
      type: ContentType.Json,
      format: "json",
      ...params,
    });
  /**
   * No description
   *
   * @tags Resource
   * @name Get
   * @request GET:/api/Resource/{isArchive}
   */
  get = (isArchive: boolean, params: RequestParams = {}) =>
    this.request<ResourceDto, ValidationProblemDetails>({
      path: `/api/Resource/${isArchive}`,
      method: "GET",
      format: "json",
      ...params,
    });
  /**
   * No description
   *
   * @tags Resource
   * @name GetById
   * @request GET:/api/Resource/get/{id}
   */
  getById = (id: number, params: RequestParams = {}) =>
    this.request<ResourceItem, ValidationProblemDetails>({
      path: `/api/Resource/get/${id}`,
      method: "GET",
      format: "json",
      ...params,
    });
  /**
   * No description
   *
   * @tags Resource
   * @name Update
   * @request POST:/api/Resource/update
   */
  update = (data: ResourceUpdateRequestDto, params: RequestParams = {}) =>
    this.request<string, ValidationProblemDetails>({
      path: `/api/Resource/update`,
      method: "POST",
      body: data,
      type: ContentType.Json,
      format: "json",
      ...params,
    });
  /**
   * No description
   *
   * @tags Resource
   * @name UpdateState
   * @request POST:/api/Resource/updateState
   */
  updateState = (data: ResourceUpdateStateRequestDto, params: RequestParams = {}) =>
    this.request<string, ValidationProblemDetails>({
      path: `/api/Resource/updateState`,
      method: "POST",
      body: data,
      type: ContentType.Json,
      format: "json",
      ...params,
    });
}
