import api from "./axiosInstance";

export const requestApi = {
  send: (dto) => api.post("/requests", dto),
  getForOwner: () => api.get("/requests"),
  getMine: () => api.get("/requests/my"),
  getHistory: () => api.get("/requests/history"),
  accept: (id) => api.put(`/requests/${id}/accept`),
  reject: (id) => api.put(`/requests/${id}/reject`),
};
