import api from "./axiosInstance";

export const authApi = {
  login: (dto) => api.post("/users/login", dto),
  register: (dto) => api.post("/users/register", dto),
  approveUser: (id) => api.put(`/users/approve/${id}`),
  rejectUser: (id) => api.delete(`/users/reject/${id}`),
  getPending: () => api.get("/users/pending"),
};
