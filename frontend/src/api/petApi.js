import api from "./axiosInstance";

export const petApi = {
  getAll: () => api.get("/pets"),
  getMine: () => api.get("/pets/mine"),
  getById: (id) => api.get(`/pets/${id}`),
  search: (params) => api.get("/pets/search", { params }),
  create: (data) => api.post("/pets", data),
  update: (id, data) => api.put(`/pets/${id}`, data),
  delete: (id) => api.delete(`/pets/${id}`),
  approve: (id) => api.put(`/pets/${id}/approve`),
  reject: (id) => api.put(`/pets/${id}/reject`),
  getPending: () => api.get("/pets/pending"),
};
