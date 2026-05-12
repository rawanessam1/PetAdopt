import api from "./axiosInstance";

export const reviewApi = {
  add: (dto) => api.post("/reviews", dto),
  getByOwner: (ownerId) => api.get(`/reviews/${ownerId}`),
};
