import axios, { AxiosResponse } from "axios";
import {
  PagedResult,
  PaginationParams,
  Permission,
  PermissionPost,
  PermissionType,
} from "./models";

const API_URL = process.env.REACT_APP_API_URL;
const PERMISSION_URL = `${API_URL}/permission`;

const requestPermission = (permission: PermissionPost): Promise<void> => {
  return axios.post(PERMISSION_URL, permission);
};

const modifyPermission = (permission: PermissionPost): Promise<void> => {
  return axios.put(PERMISSION_URL, permission);
};

const getPermissions = (
  paginationParams: PaginationParams
): Promise<PagedResult<Permission>> => {
  return axios
    .get(PERMISSION_URL, { params: paginationParams })
    .then((response: AxiosResponse<PagedResult<Permission>>) => response.data);
};

const getTypes = (): Promise<PermissionType[]> => {
  return axios
    .get(`${PERMISSION_URL}/types`)
    .then((response: AxiosResponse<PermissionType[]>) => response.data);
};

export { requestPermission, modifyPermission, getPermissions, getTypes };
