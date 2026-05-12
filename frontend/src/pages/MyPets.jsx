import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import { petApi } from "../api/petApi";

export default function MyPets() {
  const [pets, setPets] = useState([]);
  const [loading, setLoading] = useState(true);
  const [msg, setMsg] = useState("");

  useEffect(() => {
    petApi
      .getMine()
      .then((res) => setPets(res.data))
      .catch(() => setPets([]))
      .finally(() => setLoading(false));
  }, []);

  async function handleDelete(id) {
    if (!confirm("Delete this pet?")) return;
    try {
      await petApi.delete(id);
      setPets((prev) => prev.filter((p) => p.id !== id));
      setMsg("Pet deleted.");
    } catch {
      setMsg("Could not delete pet.");
    }
  }

  if (loading)
    return (
      <div className="min-h-[60vh] flex items-center justify-center">
        <p className="text-on-surface-variant text-xl">Loading...</p>
      </div>
    );

  return (
    <div className="max-w-5xl mx-auto px-6 py-10">
      <div className="flex items-center justify-between mb-8">
        <h1 className="text-3xl font-bold">🐾 My Pets</h1>
        <Link
          to="/my-pets/new"
          className="bg-primary text-white px-6 py-2.5 rounded-xl font-medium hover:bg-primary/90 transition"
        >
          + Add Pet
        </Link>
      </div>

      {msg && (
        <div className="bg-primary/10 border border-primary/30 text-primary rounded-xl px-4 py-3 mb-6 text-sm">
          {msg}
        </div>
      )}

      {pets.length === 0 ? (
        <div className="text-center py-20 bg-surface rounded-2xl border border-white/10">
          <p className="text-on-surface-variant text-lg mb-4">No pets yet</p>
          <Link
            to="/my-pets/new"
            className="bg-primary text-white px-6 py-2.5 rounded-xl hover:bg-primary/90 transition"
          >
            Add your first pet
          </Link>
        </div>
      ) : (
        <div className="flex flex-col gap-4">
          {pets.map((pet) => (
            <div
              key={pet.id}
              className="bg-surface rounded-2xl p-5 border border-white/10 flex items-center justify-between"
            >
              <div className="flex items-center gap-4">
                <div className="w-14 h-14 rounded-xl bg-background flex items-center justify-center overflow-hidden">
                  {pet.images?.[0]?.url ? (
                    <img
                      src={pet.images[0].url}
                      alt={pet.petName}
                      className="w-full h-full object-cover"
                    />
                  ) : (
                    <span className="text-2xl">🐾</span>
                  )}
                </div>
                <div>
                  <h3 className="font-bold text-lg">{pet.petName}</h3>
                  <p className="text-on-surface-variant text-sm">
                    {pet.breed} · {pet.type}
                  </p>
                </div>
              </div>
              <div className="flex items-center gap-3">
                <span
                  className={`px-3 py-1 rounded-full text-xs font-medium ${
                    pet.status === "Available"
                      ? "bg-green-500/10 text-green-400"
                      : pet.status === "PendingApproval"
                        ? "bg-yellow-500/10 text-yellow-400"
                        : pet.status === "Adopted"
                          ? "bg-blue-500/10 text-blue-400"
                          : "bg-gray-500/10 text-gray-400"
                  }`}
                >
                  {pet.status}
                </span>
                <Link
                  to={`/my-pets/${pet.id}/edit`}
                  className="px-4 py-2 rounded-xl border border-white/10 text-sm hover:border-primary hover:text-primary transition"
                >
                  Edit
                </Link>
                {pet.status !== "Adopted" && (
                  <button
                    onClick={() => handleDelete(pet.id)}
                    className="px-4 py-2 rounded-xl border border-red-500/30 text-red-400 text-sm hover:bg-red-500/10 transition"
                  >
                    Delete
                  </button>
                )}
              </div>
            </div>
          ))}
        </div>
      )}
    </div>
  );
}
