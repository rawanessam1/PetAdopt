import api from "./axiosInstance";

export const favoriteApi = {
  add: (petId) => api.post("/favorites", { petId }),
  remove: (petId) => api.delete(`/favorites/${petId}`),
  getMyFavorites: () => api.get("/favorites"),
};
