import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import { useAuth } from "../context/AuthContext";
import { favoriteApi } from "../api/favoriteApi";

export default function Favorites() {
  const { user } = useAuth();
  const [favorites, setFavorites] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    if (user?.role !== "Adopter") {
      setLoading(false);
      return;
    }
    favoriteApi
      .getMyFavorites()
      .then((res) => setFavorites(res.data))
      .catch(() => setFavorites([]))
      .finally(() => setLoading(false));
  }, [user]);

  async function handleRemove(petId) {
    try {
      await favoriteApi.remove(petId);
      setFavorites((prev) => prev.filter((f) => f.petId !== petId));
    } catch {}
  }

  if (loading)
    return (
      <div className="min-h-[60vh] flex items-center justify-center">
        <p className="text-on-surface-variant text-xl">Loading...</p>
      </div>
    );

  return (
    <div className="max-w-4xl mx-auto px-6 py-10">
      <h1 className="text-3xl font-bold mb-8">❤️ Your Favorites</h1>

      {favorites.length === 0 ? (
        <div className="text-center py-20 bg-surface rounded-2xl border border-white/10">
          <p className="text-4xl mb-4">🐾</p>
          <p className="text-on-surface-variant text-lg mb-4">
            No favorites yet
          </p>
          <Link
            to="/browse"
            className="bg-primary text-white px-6 py-2.5 rounded-xl hover:bg-primary/90 transition"
          >
            Browse Pets
          </Link>
        </div>
      ) : (
        <div className="flex flex-col gap-4">
          {favorites.map((fav) => (
            <div
              key={fav.petId}
              className="bg-surface rounded-2xl p-5 border border-white/10 flex items-center justify-between"
            >
              <div>
                <h3 className="font-bold text-lg">{fav.petName}</h3>
                <p className="text-on-surface-variant text-sm">
                  {fav.breed} · {fav.location}
                </p>
              </div>
              <div className="flex gap-3">
                <Link
                  to={`/pets/${fav.petId}`}
                  className="px-4 py-2 rounded-xl border border-white/10 text-sm hover:border-primary hover:text-primary transition"
                >
                  View
                </Link>
                <button
                  onClick={() => handleRemove(fav.petId)}
                  className="px-4 py-2 rounded-xl border border-red-500/30 text-red-400 text-sm hover:bg-red-500/10 transition"
                >
                  Remove
                </button>
              </div>
            </div>
          ))}
        </div>
      )}
    </div>
  );
}
