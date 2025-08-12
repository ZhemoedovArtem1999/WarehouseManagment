import { BalanceDto, BalanceFilterData, ValidationProblemDetails } from "./data-contracts";
import { HttpClient, RequestParams } from "./http-client";

export class BalanceApi<SecurityDataType = unknown> extends HttpClient<SecurityDataType> {
  /**
   * No description
   *
   * @tags Balance
   * @name GetBalance
   * @request GET:/api/Balance/filter
   */
  getBalance = (
    query?: {
      Resources?: number[];
      Units?: number[];
    },
    params: RequestParams = {},
  ) =>
    this.request<BalanceDto[], ValidationProblemDetails>({
      path: `/api/Balance/filter`,
      method: "GET",
      query: query,
      format: "json",
      ...params,
    });
  /**
   * No description
   *
   * @tags Balance
   * @name GetDataFilter
   * @request GET:/api/Balance/getFilter
   */
  getDataFilter = (params: RequestParams = {}) =>
    this.request<BalanceFilterData, ValidationProblemDetails>({
      path: `/api/Balance/getFilter`,
      method: "GET",
      format: "json",
      ...params,
    });
}
